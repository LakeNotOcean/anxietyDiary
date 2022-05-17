import { makeObservable, observable } from "mobx";

export default class CommonStore {
  token: string | null = null;
  appLoaded = false;

  constructor() {
    makeObservable(this, {
      token: observable,
    });
  }

  setToken = (token: string | null) => {
    if (token) window.localStorage.setItem("jwt", token);
    this.token = token;
  };
  setAppLoaded = () => {
    this.appLoaded = true;
  };
}
