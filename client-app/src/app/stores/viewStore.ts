import datesFromString from "@src/lib/DatesFromString";
import {
  makeAutoObservable,
  makeObservable,
  observable,
  runInAction,
} from "mobx";
import agent from "../api/agent";
import { UserRoleEnum } from "../enums/UserEnum";
import { IUserView } from "../models/diaryViewDate";
import { UserInfo } from "../models/user";
import { store } from "./store";

export default class ViewStore {
  currUserView: UserInfo | null = null;
  currDate: Date | null = null;
  dates: Date[] | null = null;
  usersViews: IUserView[] = null;

  constructor() {
    makeAutoObservable(this);
  }

  setDates = (dates: Date[]) => {
    this.dates = dates;
  };

  setUserView = (user: UserInfo) => {
    if (store.userStore.isLoggedIn) {
      this.currUserView = user;
    } else {
      this.currUserView = null;
    }
  };

  isAnotherUser = (): boolean => {
    if (
      this.currUserView !== null &&
      store.userStore.user !== null &&
      this.currUserView.userName != store.userStore.user.userName
    ) {
      return true;
    }
    return false;
  };

  getDiaryLastViewDate = (diaryName: string): Date => {
    if (!this.isAnotherUser) {
      return new Date();
    }
    const date = this?.usersViews
      .find((uv) => uv.userName == this.currUserView.userName)
      .diariesViews.find((dv) => dv.diaryName == diaryName).lastViewDate;
    return new Date(date);
  };

  watchDiary = async (diaryName: string) => {
    console.log("watch diary is called");
    if (!store.userStore.isLoggedIn || !this.isAnotherUser()) {
      return;
    }
    try {
      console.log("watch diary in proccess");
      const params = new URLSearchParams();
      params.append("userName", this.currUserView.userName);
      params.append("diaryName", diaryName);
      await agent.dates.viewDiary(params);
    } catch (error) {
      throw error;
    }
  };

  loadUsersViews = async () => {
    if (!store.userStore.isLoggedIn) {
      return;
    }
    if (store.userStore.user.role !== UserRoleEnum.patient) {
      try {
        const res = await agent.dates.notViewList();
        runInAction(() => (this.usersViews = res));
      } catch (error) {
        throw error;
      }
    }
  };

  loadDates = async () => {
    runInAction(() => {
      store.recordsStore.setLoading(true, "Загрузка записей...");
    });
    try {
      const rawRes = await agent.dates.list(this.datesParams);
      runInAction(() => {
        const result = datesFromString(rawRes);
        this.setDates(result);
        store.recordsStore.setLoading(false);
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        store.recordsStore.setLoading(false);
      });
    }
  };

  get datesParams() {
    const params = new URLSearchParams();
    params.append("name", store.recordsStore.diaryDescription.ShortName);
    if (this.isAnotherUser) {
      params.append("userName", store.viewStore.currUserView.userName);
    }
    return params;
  }
}
