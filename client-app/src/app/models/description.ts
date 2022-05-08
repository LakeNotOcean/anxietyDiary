import { IColumn } from "./column";

export interface IDescription {
  ShortName: string;
  Name: string;
  DiaryType: DiaryEnum;
  Description: string;
  CategoryId: number;
  Columns: Map<string, IColumn>;
}
