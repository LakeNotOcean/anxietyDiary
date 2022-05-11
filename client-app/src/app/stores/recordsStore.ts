import { diaryDeserialize } from "@src/lib/DiaryDeserialize";
import diarySerialize from "@src/lib/DiarySerialize";
import { makeObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { IDescription } from "../models/description";
import { IDiary } from "../models/diary";

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

  constructor() {
    makeObservable(this, {});
  }

  loadRecords = async () => {
    runInAction(() => {
      this.setLoading(true, "Загрузка записей...");
    });
    try {
      const records = await agent.records.list(
        this.diaryDescription.ShortName,
        this.date,
        1,
        10
      );
      runInAction(() => {
        const deserializedRecords = diaryDeserialize(
          records.data,
          this.diaryDescription
        );
        this.setLoading(false, "");
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.setLoading(false, "");
      });
    }
  };
  setLoading = (isLoading: boolean, message = "") => {
    this.loading = { isLoading: isLoading, message: message } as ILoading;
  };

  selectRecord = (id: number) => {
    this.selectedRecord = this.records.find((r) => r.Id === id);
  };

  cancelSelectedRecord = () => {
    this.selectedRecord = undefined;
  };

  openForm = (id?: number) => {
    id ? this.selectRecord(id) : this.cancelSelectedRecord();
    this.editMode = true;
  };

  closeForm = () => {
    this.cancelSelectedRecord();
    this.editMode = false;
  };

  createOrEditRecord = async (record: IDiary) => {
    this.setLoading(true, "Добавление записи...");
    try {
      if (record.Id) {
        const dto = {} as IUpdateRecord;
        dto.body = diarySerialize(record);
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
        });
      } else {
        const dto = {} as IPostRecord;
        dto.body = diarySerialize(record);
        dto.name = this.diaryDescription.ShortName;
        console.log(dto);
        await agent.records.create(dto);
        runInAction(() => {
          this.records = [...this.records, record];
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

  deleteRecord = async (record: IDiary) => {
    this.setLoading(true, "Удаление записи...");
    try {
      await agent.records.delete(this.diaryDescription.ShortName, record.Id);
      runInAction(() => {
        this.records = [...this.records.filter((r) => r.Id !== record.Id)];
        this.closeForm();
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.setLoading(false);
      });
    }
  };
}
