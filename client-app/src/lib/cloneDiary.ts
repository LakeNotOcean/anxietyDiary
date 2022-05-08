import { IDiary } from "@src/app/models/diary";

export default function cloneDiary(diary: IDiary) {
  let result = { ...diary } as IDiary;
  result.Columns = new Map(diary.Columns);
  return result;
}
