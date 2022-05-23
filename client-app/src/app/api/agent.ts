import axios, { Axios, AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { PaginatedResult } from "../models/pagination";
import {
  User,
  UserInfo,
  UserLoginFormValues,
  UserRegisterFormValues,
} from "../models/user";
import { store } from "../stores/store";
import { history } from "@src/index";
import { ReqInfo, RequestSend } from "../models/request";
import { IUserView } from "../models/diaryViewDate";

axios.defaults.baseURL = "http://localhost:5000/api";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.interceptors.response.use(
  async (response) => {
    await sleep(2000);
    const pagination = response.headers["pagination"];
    console.log(response);
    if (pagination) {
      response.data = new PaginatedResult(
        response.data as JSON[],
        JSON.parse(pagination)
      );
      return response as AxiosResponse<PaginatedResult<any>>;
    }
    return response;
  },
  (error: AxiosError) => {
    const { data, status, headers } = error.response!;
    switch (status) {
      case 400:
        toast.error("bad request");
        break;
      case 401:
        if (
          status == 401 &&
          headers["www-authenticate"].startsWith('Bearer error="invalid_token"')
        ) {
          store.userStore.logout();
          toast.error("Время сессии вышло");
          break;
        }
        store.userStore.logout();
        store.userStore.setIsLoginForm(true);
        break;
      case 404:
        history.push("/not-found");
        break;
      case 500:
        toast.error("server error");
        break;
      case 405:
        toast.error("method not allowed");
        break;
    }
    return Promise.reject(data);
  }
);

axios.interceptors.request.use((config) => {
  const token = store.commonStore.token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
    return config;
  }
  return config;
});

const request = {
  get: <T>(url: string, params?: URLSearchParams) =>
    axios.get<T>(url, { params }).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const records = {
  list: (params: URLSearchParams) =>
    request.get<PaginatedResult<JSON[]>>("/diary", params),
  create: (record: IPostRecord) => request.post<number>("/diary", record),
  update: (record: IUpdateRecord, name: string, id: number) =>
    request.put<void>(`/diary?name=${name}&id=${id}`, record),
  delete: (name: string, id: number) =>
    request.delete<void>(`/diary?name=${name}&id=${id}`),
};

const dates = {
  list: (params: URLSearchParams) =>
    request.get<string[]>("/dates/diary", params),
  notViewList: () => request.get<IUserView[]>("dates/notview"),
  viewDiary: (params: URLSearchParams) =>
    request.get<void>("/dates/view", params),
};

const account = {
  current: () => request.get<User>("/account/user"),
  login: (user: UserLoginFormValues) =>
    request.post<User>("/account/login", user),
  register: (user: UserRegisterFormValues) =>
    request.post<User>("/account/register", user),
  changeInfo: (userInfo: UserInfo) =>
    request.post<UserInfo>("/account/changeinfo", userInfo),
  refreshToken: () => request.post<User>("/account/refreshToken", {}),
};

const users = {
  getDoctors: () => request.get<UserInfo[]>("/user/doctors"),
  removeDoctor: (userName: string) =>
    request.delete<void>(`/user/remove?name=${userName}&isDoctor=true`),
  findDoctors: (str: string) =>
    request.get<UserInfo[]>(`/user/searchDoctors?str=${str}`),
  getPatients: () => request.get<UserInfo[]>("/user/patients"),
  removePatient: (userName: string) =>
    request.delete<void>(`/user/remove?name=${userName}&isDoctor=false`),
  findPatients: (str: string) =>
    request.get<UserInfo[]>(`/user/searchUsers?str=${str}`),
};

const requests = {
  sendRequest: (userRequest: RequestSend) =>
    request.post<RequestSend>("/request/request", userRequest),
  getRequestDoctors: () => request.get<ReqInfo[]>("/request/doctors"),
  getRequestPatients: () => request.get<ReqInfo[]>("/request/patients"),
  getUserRequest: () => request.get<ReqInfo[]>("/request/user"),
  cancelRequest: (requestId: number) =>
    request.delete<number>(`/request/cancel?requestId=${requestId}`),
  acceptRequest: (requestId: number) =>
    request.get<number>(`/request/accept?requestId=${requestId}`),
};

const agent = {
  records,
  dates,
  account,
  requests,
  users,
};

export default agent;
