import { ColumnTypeEnum } from "../enums/ColumnEnum";

export interface IColumn {
  ShortName: string;
  Name: string;
  ValueType: ColumnTypeEnum;
  isOptional: boolean;
  isArbitrary: boolean;
}
