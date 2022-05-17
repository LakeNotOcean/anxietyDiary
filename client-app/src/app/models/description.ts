import { DiaryTypeEnum } from "../enums/DiaryEnum";
import { IColumn } from "./column";

export interface IDescription {
  ShortName: string;
  Name: string;
  DiaryType: DiaryTypeEnum;
  Description: string;
  CategoryId: number;
  Columns: Map<string, IColumn>;
}
