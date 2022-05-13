import { ColumnTypeEnum } from "@src/app/enums/ColumnEnum";
import { IDiary, RecordValueType } from "@src/app/models/diary";
import { getDescriptionColumnArray } from "./CreateDescriptions";

export function createInitialDiary(diaryName: string) {
  const description = getDescriptionColumnArray(diaryName);
  let columns = new Map<string, RecordValueType | Array<RecordValueType>>();
  description.forEach((column) => {
    if (column.isOptional) {
      columns.set(column.ShortName, new Array<RecordValueType>());
    } else {
      columns.set(
        column.ShortName,
        getInitialValue(diaryName, column.ValueType, column.ShortName)
      );
    }
  });
  return {
    DateTime: new Date(),
    Columns: columns,
  } as IDiary;
}

function getInitialValue(
  diaryName: string,
  type: ColumnTypeEnum,
  columnName: string
): RecordValueType {
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
      if (diaryName == "diary2" && columnName == "column8") {
        return createHumanBodyObject();
      }
      return new Object();
      break;
    case ColumnTypeEnum.Date:
      return new Date();
      break;
  }
}

function createHumanBodyObject(): Object {
  return {
    head: { text: "" },
    left_shoulder: { text: "" },
    right_shoulder: { text: "" },
    left_arm: { text: "" },
    right_arm: { text: "" },
    chest: { text: "" },
    stomach: { text: "" },
    left_leg: { text: "" },
    right_leg: { text: "" },
    left_hand: { text: "" },
    right_hand: { text: "" },
    left_foot: { text: "" },
    right_foot: { text: "" },
  };
}
