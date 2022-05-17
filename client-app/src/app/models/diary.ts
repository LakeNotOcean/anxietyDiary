export interface IDiary {
  Id: number;
  DateTime: Date;
  Columns: Map<string, RecordValueType | Array<RecordValueType>>;
}

export type RecordValueType = string | number | Object | boolean | Date;
