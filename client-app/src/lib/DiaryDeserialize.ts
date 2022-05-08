import { ColumnEnum } from "@src/app/Enum/ColumnEnum";
import { IDescription } from "@src/app/models/description";
import { IDiary } from "@src/app/models/diary";

export function diaryDeserialize(
  data: Array<JSON>,
  diaryDescription: IDescription
): Array<IDiary> {
  let result = new Array<IDiary>();
  for (let record of data) {
    let newRecord = {} as IDiary;
    newRecord.Id = record[ColumnEnum.Id];
    newRecord.DateTime = record[ColumnEnum.DateTime];
    newRecord.Columns = new Map<string, string | string[]>();
    diaryDescription.Columns.forEach((column) => {
      newRecord.Columns.set(column.ShortName, record[column.ShortName]);
    });
    result.push(newRecord);
  }

  return result;
}
