import LoginForm from "@src/features/users/LoginForm";
import datesFromString from "@src/lib/DatesFromString";
import { diaryDeserialize } from "@src/lib/DiaryDeserialize";
import diarySerialize from "@src/lib/DiarySerialize";
import { makeObservable, observable, observe, runInAction, toJS } from "mobx";
import moment from "moment-timezone";
import { toast } from "react-toastify";
import agent from "../api/agent";
import { IDescription } from "../models/description";
import { IDiary } from "../models/diary";
import { Pagination, PagingParams } from "../models/pagination";
import { store } from "./store";

export interface ILoading {
  isLoading: boolean;
  message: string;
}

export default class RecordsStore {
  records: IDiary[] = [];
  selectedRecord: IDiary | undefined = undefined;
  diaryDescription: IDescription | null = null;
  date: Date | null = null;
  editMode = false;
  loading = { isLoading: false, message: "" } as ILoading;
  pagination: Pagination | null = null;
  pagingParams: PagingParams | null;
  dates: Date[] | null = null;

  constructor() {
    makeObservable(this, {
      records: observable,
      selectedRecord: observable,
      diaryDescription: observable,
      date: observable,
      editMode: observable,
      loading: observable,
      pagination: observable,
      pagingParams: observable,
      dates: observable,
    });
  }

  loadRecords = async () => {
    if (!store.userStore.isLoggedIn) {
      this.setLoginForm();
    }
    runInAction(() => {
      this.setLoading(true, "Загрузка записей...");
    });
    try {
      console.log(this.axiosParam.toString());
      const result = await agent.records.list(this.axiosParam);
      runInAction(() => {
        const deserializedRecords = diaryDeserialize(
          result.data,
          this.diaryDescription
        );
        this.setRecords(deserializedRecords);
        this.setPagination(result.pagination);
        this.setLoading(false);
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.setLoading(false);
      });
    }
  };

  setPagination = (pagination: Pagination) => {
    this.pagination = pagination;
  };
  setPagingParams = (paginParams: PagingParams) => {
    this.pagingParams = paginParams;
    console.log(this.pagingParams.pageNumber, this.pagingParams.pageSize);
  };

  loadDates = async () => {
    runInAction(() => {
      this.setLoading(true, "Загрузка записей...");
    });
    try {
      const rawRes = await agent.dates.list(this.datesParams);
      runInAction(() => {
        const result = datesFromString(rawRes);
        this.setDates(result);
        this.setLoading(false);
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.setLoading(false);
      });
    }
  };
  setDates = (dates: Date[]) => {
    this.dates = dates;
  };

  get axiosParam() {
    const params = new URLSearchParams();
    params.append("name", this.diaryDescription.ShortName);
    params.append("date", this.date.toISOString());
    params.append("pagenumber", this.pagingParams.pageNumber.toString());
    params.append("pagesize", this.pagingParams.pageSize.toString());
    params.append("timezone", moment.tz.guess());
    return params;
  }
  get datesParams() {
    const params = new URLSearchParams();
    params.append("name", this.diaryDescription.ShortName);
    return params;
  }
  setLoading = (isLoading: boolean, message = "") => {
    this.loading = { isLoading: isLoading, message: message } as ILoading;
  };

  selectRecord = (id: number) => {
    this.selectedRecord = this.records.find((r) => r.Id === id);
  };

  setRecords(records: Array<IDiary>) {
    this.records = records;
  }

  cancelSelectedRecord = () => {
    this.selectedRecord = undefined;
  };

  openForm = (id?: number) => {
    if (!store.userStore.isLoggedIn) {
      toast.error("Для добавлении записи необходимо авторизироваться");
      return;
    }
    id ? this.selectRecord(id) : this.cancelSelectedRecord();
    this.editMode = true;
  };

  closeForm = () => {
    this.cancelSelectedRecord();
    this.editMode = false;
  };

  setLoginForm = () => {
    store.userStore.setIsLoginForm(true);
  };
  createOrEditRecord = async (record: IDiary) => {
    if (!store.userStore.isLoggedIn) {
      this.setLoginForm();
      return;
    }

    console.log("Create or edit: ", record);

    this.setLoading(true, "Добавление записи...");
    try {
      if (record.Id) {
        const dto = {} as IUpdateRecord;
        dto.body = diarySerialize(record);
        console.log("dto: ", dto);
        await agent.records.update(
          dto,
          this.diaryDescription.ShortName,
          record.Id
        );
        runInAction(() => {
          this.records = [
            ...this.records.filter((r) => r.Id !== record.Id),
            record,
          ];
          this.closeForm();
          this.setLoading(false);
        });
      } else {
        const dto = {} as IPostRecord;
        dto.body = diarySerialize(record);
        dto.name = this.diaryDescription.ShortName;
        let id = await agent.records.create(dto);
        runInAction(() => {
          if (this.pagination == null) {
            this.loadRecords();
            this.setLoading(false);
            return;
          }
          record.Id = id;
          if (
            this.pagination.currentPage == this.pagination.totalPages &&
            this.pagination.itemsPerPage > this.records.length
          ) {
            console.log("records array is changed");
            this.records = [...this.records, record];
          } else {
            this.changePagOnAdd();
          }
          this.closeForm();
        });
        runInAction(() => {
          this.setLoading(false);
        });
      }
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.setLoading(false);
      });
    }
  };

  private changePagOnAdd = () => {
    this.pagination.totalItems += 1;
    if (
      Math.ceil(this.pagination.totalItems / this.pagination.itemsPerPage) >
      this.pagination.totalPages
    ) {
      this.pagination.totalPages += 1;
    }
  };

  deleteRecord = async (record: IDiary) => {
    if (!store.userStore.isLoggedIn) {
      this.setLoginForm();
      return;
    }

    this.setLoading(true, "Удаление записи...");
    try {
      await agent.records.delete(this.diaryDescription.ShortName, record.Id);
      if (this.records.length == 1) {
        this.pagingParams.setPreviousPage();
      }
      runInAction(() => {
        // this.records = [...this.records.filter((r) => r.Id !== record.Id)];
        this.closeForm();
        this.setLoading(false);
      });
      await this.loadRecords();
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.setLoading(false);
      });
    }
  };
}
