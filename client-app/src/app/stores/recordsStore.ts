import { diaryDeserialize } from "@src/lib/DiaryDeserialize";
import { makeObservable } from "mobx";
import agent from "../api/agent";
import { IDescription } from "../models/description";
import { IDiary } from "../models/diary";

export default class RecordsStore {
  records: IDiary[] = [];
  selectedRecord: IDiary | null = null;
  diaryDescription: IDescription | null = null;
  date: Date | null = null;
  editMode = false;
  loading = false;
  loadingInitial = false;

  constructor() {
    makeObservable(this, {});
  }

  loadRecords = async () => {
    this.loadingInitial = true;
    try {
      const records = await agent.records.list(
        this.diaryDescription.ShortName,
        this.date,
        1,
        10
      );
      const deserializedRecords = diaryDeserialize(
        records.data,
        this.diaryDescription
      );
      this.loadingInitial = false;
    } catch (error) {
      console.log(error);
      this.loadingInitial = false;
    }
  };
}
