import { IDiary } from "@src/app/models/diary";

export default function diarySerialize(record: IDiary): JSON {
  let body = {} as JSON;
  record.Columns.forEach((value, key) => {
    if (Array.isArray(value)) {
      body[key] = value;
    } else {
      body[key] = JSON.stringify(value);
    }
  });
  return body;
}
