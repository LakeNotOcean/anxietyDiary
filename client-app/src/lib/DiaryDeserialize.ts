import { ColumnEnum, ColumnTypeEnum } from "@src/app/enums/ColumnEnum";
import { IDescription } from "@src/app/models/description";
import { IDiary, RecordValueType } from "@src/app/models/diary";

export function diaryDeserialize(
  data: Array<JSON>,
  diaryDescription: IDescription
): Array<IDiary> {
  let result = new Array<IDiary>();
  for (let record of data) {
    let newRecord = {} as IDiary;
    newRecord.Id = record[ColumnEnum.Id];
    newRecord.DateTime = record[ColumnEnum.DateTime];
    newRecord.Columns = new Map<
      string,
      RecordValueType | Array<RecordValueType>
    >();
    diaryDescription.Columns.forEach((column) => {
      newRecord.Columns.set(
        column.ShortName,
        column.isArbitrary
          ? parseValueArrayFromString(
              record[column.ShortName],
              column.ValueType
            )
          : parseValueFromString(record[column.ShortName], column.ValueType)
      );
    });
    result.push(newRecord);
  }

  return result;
}

function parseValueFromString(
  value: string,
  valueType: ColumnTypeEnum
): RecordValueType {
  console.log("value: ", value, "type: ", typeof value);
  switch (valueType) {
    case ColumnTypeEnum.Bool:
      return (value === "true") as boolean;
    case ColumnTypeEnum.Int:
      return parseInt(value);
    case ColumnTypeEnum.String:
      return value;
    case ColumnTypeEnum.Date:
      return new Date(value);
    case ColumnTypeEnum.Json:
      return JSON.parse(value);
    default:
      return value;
  }
}

function parseValueArrayFromString(
  value: string[],
  valueType: ColumnTypeEnum
): Array<RecordValueType> {
  let result = new Array<RecordValueType>();
  for (let record of value) {
    result.push(parseValueFromString(record, valueType));
  }
  return result;
}
