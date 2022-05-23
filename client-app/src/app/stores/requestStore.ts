import { makeObservable, observable, observe, runInAction } from "mobx";
import { toast } from "react-toastify";
import agent from "../api/agent";
import { RequestTypeEnum } from "../enums/RequestEnum";
import { UserRoleEnum } from "../enums/UserEnum";
import { ReqInfo, RequestSend } from "../models/request";
import { store } from "./store";

export default class RequestStore {
  viewAsDoctorRequests: ReqInfo[] = [];
  inviteDoctorRequests: ReqInfo[] = [];
  userRequests: ReqInfo[] = [];

  constructor() {
    makeObservable(this, {
      viewAsDoctorRequests: observable,
      inviteDoctorRequests: observable,
    });
  }

  setInviteDoctorRequests(req: ReqInfo[]) {
    this.inviteDoctorRequests = req;
  }
  setViewAsDoctorRequests(req: ReqInfo[]) {
    this.viewAsDoctorRequests = req;
  }
  setUserRequests(req: ReqInfo[]) {
    this.userRequests = req;
  }

  sendRequestToDoctor = async (userName: string) => {
    try {
      store.commonStore.setAppLoaded(true);
      const req = {
        requestType: RequestTypeEnum.InviteDoctor,
        userName: userName,
      } as RequestSend;
      await agent.requests.sendRequest(req);
      runInAction(() => {
        store.modalStore.closeModal();
        store.commonStore.setAppLoaded(false);
      });
    } catch (error) {
      runInAction(() => {
        store.modalStore.closeModal();
        store.commonStore.setAppLoaded(false);
      });
      toast.error("Не удалось отправить запрос");
    }
  };
  sendRequestToPatient = async (userName: string) => {
    try {
      store.commonStore.setAppLoaded(true);
      const req = {
        requestType: RequestTypeEnum.ViewAsDoctor,
        userName: userName,
      } as RequestSend;
      await agent.requests.sendRequest(req);
      runInAction(() => {
        store.modalStore.closeModal();
        store.commonStore.setAppLoaded(false);
      });
    } catch (error) {
      runInAction(() => {
        store.modalStore.closeModal();
        store.commonStore.setAppLoaded(false);
      });
      toast.error("Не удалось отправить запрос");
    }
  };
  loadViewAsDoctorRequests = async () => {
    try {
      let res = await agent.requests.getRequestDoctors();
      runInAction(() => {
        this.setViewAsDoctorRequests(res);
        store.commonStore.setAppLoaded(false);
      });
    } catch (error) {
      throw error;
    }
  };
  loadInviteDoctorRequests = async () => {
    try {
      let res = await agent.requests.getRequestPatients();
      runInAction(() => {
        this.setInviteDoctorRequests(res);
        store.commonStore.setAppLoaded(false);
      });
    } catch (error) {
      throw error;
    }
  };
  loadUserRequests = async () => {
    try {
      let res = await agent.requests.getUserRequest();
      runInAction(() => {
        this.setUserRequests(res);
        store.commonStore.setAppLoaded(false);
      });
    } catch (error) {
      throw error;
    }
  };

  cancelRequest = async (req: ReqInfo) => {
    try {
      runInAction(() => store.commonStore.setAppLoaded(true));
      await agent.requests.cancelRequest(req.requestId);
      runInAction(() => {
        store.commonStore.setAppLoaded(false);
        if (req.sourceUser.userName == store.userStore.user.userName) {
          this.loadUserRequests();
        } else
          switch (req.requestType) {
            case RequestTypeEnum.InviteDoctor:
              this.loadInviteDoctorRequests();
              break;
            case RequestTypeEnum.ViewAsDoctor:
              this.loadViewAsDoctorRequests();
              break;
          }
      });
    } catch (error) {
      runInAction(() => store.commonStore.setAppLoaded(false));
      toast.error("не удалось удалить запрос");
    }
  };

  acceptRequest = async (req: ReqInfo) => {
    try {
      runInAction(() => store.commonStore.setAppLoaded(true));
      await agent.requests.acceptRequest(req.requestId);
      runInAction(() => {
        store.commonStore.setAppLoaded(false);
        if (req.sourceUser.userName == store.userStore.user.userName) {
          this.loadUserRequests();
        } else
          switch (req.requestType) {
            case RequestTypeEnum.InviteDoctor:
              this.loadInviteDoctorRequests();
              break;
            case RequestTypeEnum.ViewAsDoctor:
              this.loadViewAsDoctorRequests();
              break;
          }
      });
    } catch (error) {
      runInAction(() => store.commonStore.setAppLoaded(false));
      toast.error("не удалось принять запрос");
    }
  };
}
