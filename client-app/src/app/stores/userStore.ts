import { history } from "@src/index";
import { makeObservable, observable, observe, runInAction, values } from "mobx";
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

  constructor() {
    makeObservable(this, {
      user: observable,
      isLoginForm: observable,
      userDoctors: observable,
    });
  }

  setIsLoginForm(value: boolean) {
    this.isLoginForm = true;
  }
  get isLoggedIn() {
    console.log("isLoggedIn is called");
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
      runInAction(() => store.commonStore.setAppLoaded(true));
      const user = await agent.account.login(creds);
      store.commonStore.setToken(user.token);
      history.push("/");
      store.modalStore.closeModal();
      runInAction(() => {
        this.user = user;
        store.commonStore.setAppLoaded(false);
        this.isLoginForm = false;
      });
    } catch (error) {
      runInAction(() => store.commonStore.setAppLoaded(false));
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
      //runInAction(() => store.commonStore.setAppLoaded(true));
      const user = await agent.account.register(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      history.push("/");
      store.modalStore.closeModal();
      runInAction(() => {
        store.commonStore.setAppLoaded(false);
        this.isLoginForm = false;
      });
    } catch (error) {
      //runInAction(() => store.commonStore.setAppLoaded(false));
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
        runInAction(() => store.commonStore.setAppLoaded(false));
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
}
