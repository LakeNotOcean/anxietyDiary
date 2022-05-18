import { history } from "@src/index";
import { makeObservable, observable, observe, runInAction, values } from "mobx";
import agent from "../api/agent";
import {
  User,
  UserLoginFormValues,
  UserRegisterFormValues,
} from "../models/user";
import { store } from "./store";

export default class UserStore {
  user: User | null = null;

  constructor() {
    makeObservable(this, {
      user: observable,
    });
  }

  get isLoggedIn() {
    return !!this.user;
  }
  login = async (creds: UserLoginFormValues) => {
    try {
      runInAction(() => store.commonStore.setAppLoaded(true));
      const user = await agent.account.login(creds);
      store.commonStore.setToken(user.token);
      history.push("/");
      store.modalStore.closeModal();
      runInAction(() => store.commonStore.setAppLoaded(false));
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

  getUser = async () => {
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
      runInAction(() => store.commonStore.setAppLoaded(false));
    } catch (error) {
      //runInAction(() => store.commonStore.setAppLoaded(false));
      throw error;
    }
  };
}
