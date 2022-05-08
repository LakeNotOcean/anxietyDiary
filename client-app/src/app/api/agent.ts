import axios, { Axios, AxiosResponse } from "axios";

axios.defaults.baseURL = "http://localhost:5000/api";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.interceptors.response.use(async (response) => {
  try {
    await sleep(2000);
    return response;
  } catch (error) {
    return await Promise.reject(error);
  }
});

const request = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const records = {
  list: (name: string, date: Date, pagenumber: number, pagesize: number) =>
    request.get<JSON[]>(
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
