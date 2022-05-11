import axios, { Axios, AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { PaginatedResult } from "../models/pagination";

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
    if (pagination) {
      response.data = new PaginatedResult(
        response.data,
        JSON.parse(pagination)
      );
      return response as AxiosResponse<PaginatedResult<any>>;
    }
    return response;
  },
  (error: AxiosError) => {
    const { data, status } = error.response!;
    switch (status) {
      case 400:
        toast.error("bad request");
        break;
      case 401:
        toast.error("unauthorised");
      case 404:
        toast.error("not found");
      case 500:
        toast.error("server error");
    }
    return Promise.reject(error);
  }
);

const request = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const records = {
  list: (name: string, date: Date, pagenumber: number, pagesize: number) =>
    request.get<PaginatedResult<JSON[]>>(
      `/diary?name=${name}&date=${date.toISOString()}&pagenumber=${pagenumber}&pagesize=${pagesize}`
    ),
  create: (record: IPostRecord) => request.post<number>("/diary", record),
  update: (record: IUpdateRecord, name: string, id: number) =>
    request.put<void>(`/diary?name=${name}&id=${id}`, record),
  delete: (name: string, id: number) =>
    request.delete<void>(`/diary?name=${name}&id=${id}`),
};

const agent = {
  records,
};

export default agent;
