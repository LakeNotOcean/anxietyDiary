import { ColumnTypeEnum } from "../Enum/ColumnEnum";

export interface IColumn {
  ShortName: string;
  Name: string;
  ValueType: ColumnTypeEnum;
  isOptional: boolean;
  isArbitrary: boolean;
}
