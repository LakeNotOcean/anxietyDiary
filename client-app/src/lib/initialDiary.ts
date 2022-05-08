import { ColumnTypeEnum } from "@src/app/Enum/ColumnEnum";
import { IDiary } from "@src/app/models/diary";
import { getDescriptionColumnArray } from "./CreateDescriptions";

export function createInitialDiary(diaryName: string) {
  const description = getDescriptionColumnArray(diaryName);
  let columns = new Map<
    string,
    | (string | number | JSON | boolean | Date)
    | Array<string | number | JSON | boolean | Date>
  >();
  description.forEach((column) => {
    if (column.isOptional) {
      columns.set(
        column.ShortName,
        new Array<string | number | boolean | Date | JSON>()
      );
    } else {
      columns.set(column.ShortName, getInitialValue(column.ValueType));
    }
  });
  return {
    DateTime: new Date(),
    Columns: columns,
  } as IDiary;
}

function getInitialValue(
  type: ColumnTypeEnum
): string | number | boolean | Date | JSON {
  switch (type) {
    case ColumnTypeEnum.String:
      return "";
      break;
    case ColumnTypeEnum.Bool:
      return false;
      break;
    case ColumnTypeEnum.Int:
      return 0;
      break;
    case ColumnTypeEnum.Json:
      return JSON;
      break;
    case ColumnTypeEnum.Date:
      return new Date();
      break;
  }
}
