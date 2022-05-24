import {
  action,
  makeAutoObservable,
  makeObservable,
  observable,
  reaction,
  runInAction,
} from "mobx";

export default class CommonStore {
  token: string | null = window.localStorage.getItem("jwt");
  appLoaded = false;

  constructor() {
    makeObservable(this, {
      token: observable,
      appLoaded: observable,
      setToken: action,
      setAppLoaded: action,
    });
    reaction(
      () => this.token,
      (token) => {
        if (token) {
          window.localStorage.setItem("jwt", token);
        } else {
          window.localStorage.removeItem("jwt");
        }
      }
    );
  }

  setToken = (token: string | null) => {
    this.token = token;
  };
  setAppLoaded = (value: boolean) => {
    this.appLoaded = value;
  };
}
