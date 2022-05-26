import LoginForm from "@src/features/users/LoginForm";
import { history } from "@src/index";
import {
  action,
  makeAutoObservable,
  makeObservable,
  observable,
  observe,
  runInAction,
  values,
} from "mobx";
import agent from "../api/agent";
import { UserRoleEnum } from "../enums/UserEnum";
import {
  User,
  UserInfo,
  UserLoginFormValues,
  UserRegisterFormValues,
} from "../models/user";
import { store } from "./store";

export default class UserStore {
  user: User | null = null;
  isLoginForm: boolean | null = null;
  userDoctors: UserInfo[] | null = null;
  patientList: UserInfo[] | null = null;
  refreshTokenTimeout: any;

  constructor() {
    makeObservable(this, {
      user: observable,
      isLoginForm: observable,
      userDoctors: observable,
      patientList: observable,
      refreshTokenTimeout: observable,
      setIsLoginForm: action,
    });
  }

  setIsLoginForm(value: boolean) {
    runInAction(() => (this.isLoginForm = value));
  }
  get isLoggedIn() {
    return !!this.user && this.user.role != UserRoleEnum.guest;
  }

  getCurrUser = async () => {
    if (this.user == null) {
      await this.loadUser();
      return this.user;
    }
    return this.user;
  };

  login = async (creds: UserLoginFormValues) => {
    try {
      const user = await agent.account.login(creds);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
      runInAction(() => {
        this.user = user;
        this.isLoginForm = false;
        history.push("/");
      });
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };
  logout = () => {
    runInAction(() => store.commonStore.setAppLoaded(true));
    store.commonStore.setToken(null);
    window.localStorage.removeItem("jwt");
    this.user = null;
    history.push("/");
    runInAction(() => store.commonStore.setAppLoaded(false));
  };

  loadUser = async () => {
    try {
      const user = await agent.account.current();
      runInAction(() => (this.user = user));
    } catch (error) {}
  };

  registerUser = async (creds: UserRegisterFormValues) => {
    try {
      const user = await agent.account.register(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      this.startRefreshTokenTimer(user);
      history.push("/");
      runInAction(() => {
        this.isLoginForm = false;
      });
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };

  changeUserInfo = async (creds: UserInfo) => {
    try {
      runInAction(() => store.commonStore.setAppLoaded(true));
      await agent.account.changeInfo(creds);
      await this.loadUser();
      store.modalStore.closeModal();
      runInAction(() => store.commonStore.setAppLoaded(false));
    } catch (error) {
      runInAction(() => store.commonStore.setAppLoaded(false));
      throw error;
    }
  };

  loadDoctors = async () => {
    console.log("loadDoctors is called");
    try {
      const doctors = await agent.users.getDoctors();
      runInAction(() => {
        if (doctors === null) {
          this.userDoctors = [];
        } else {
          this.userDoctors = doctors;
        }
      });
    } catch (error) {
      throw error;
    }
  };

  removeDoctor = async (doctorName: string) => {
    try {
      runInAction(() => store.commonStore.setAppLoaded(true));
      await agent.users.removeDoctor(doctorName);
      runInAction(() => {
        this.userDoctors = this.userDoctors.filter(
          (d) => d.userName != doctorName
        );
        store.commonStore.setAppLoaded(false);
      });
    } catch (error) {
      runInAction(() => store.commonStore.setAppLoaded(false));
      throw error;
    }
  };

  removePatient = async (patientName: string) => {
    try {
      runInAction(() => store.commonStore.setAppLoaded(true));
      await agent.users.removePatient(patientName);
      runInAction(() => {
        this.patientList = this.patientList.filter(
          (p) => p.userName != patientName
        );
        store.commonStore.setAppLoaded(false);
      });
    } catch (error) {
      runInAction(() => store.commonStore.setAppLoaded(false));
      throw error;
    }
  };

  findDoctors = async (searchStr: string): Promise<UserInfo[]> => {
    try {
      const res = await agent.users.findDoctors(searchStr);
      return res;
    } catch (error) {
      throw error;
    }
  };
  findPatients = async (searchStr: string): Promise<UserInfo[]> => {
    try {
      const res = await agent.users.findPatients(searchStr);
      return res;
    } catch (error) {
      throw error;
    }
  };

  getPatientList = async () => {
    try {
      const res = await agent.users.getPatients();
      runInAction(() => {
        this.patientList = res;
      });
    } catch (error) {
      throw error;
    }
  };

  refreshToken = async () => {
    this.stopRefreshTokenTimer();
    try {
      const user = await agent.account.refreshToken();
      runInAction(() => (this.user = user));
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
    } catch (error) {
      console.log(error);
    }
  };

  private startRefreshTokenTimer(user: User) {
    const jwtToken = JSON.parse(window.atob(user.token.split(".")[1]));
    console.log("jwtToken", jwtToken);
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - 60 * 1000;
    console.log("timeout", timeout);
    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
