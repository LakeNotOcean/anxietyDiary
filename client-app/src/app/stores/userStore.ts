import { makeObservable, observable, observe, runInAction } from "mobx";
import { Navigate, useNavigate } from "react-router-dom";
import agent from "../api/agent";
import { User, UserLoginFormValues } from "../models/user";
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
      const user = await agent.account.login(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      const navigate = useNavigate();
      navigate("/diaries");
      console.log(creds);
    } catch (error) {
      throw error;
    }
  };

  logout = () => {
    store.commonStore.setToken(null);
    window.localStorage.removeItem("jwt");
    this.user = null;
    const navigate = useNavigate();
    navigate("/");
  };
}
