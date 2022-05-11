import { DiaryNameEnum } from "@src/app/enums/DiaryEnum";
import { IColumn } from "@src/app/models/column";
import { IDiary } from "@src/app/models/diary";
import { ChangeEvent } from "react";
import EmotionsDiaryFormTemplate from "./emotionsDiaryFormTemplate";
import InitialDiaryFormTemplate from "./initialDiaryFormTemplate";

interface Props {
  columns: Array<IColumn>;
  record: IDiary;
  handleInputChange: (event: ChangeEvent<HTMLInputElement>) => void;
  diaryName: string;
}

export interface FormProps {
  columns: Array<IColumn>;
  record: IDiary;
  handleInputChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

export default function FormTemplate({
  diaryName,
  columns,
  record,
  handleInputChange,
}: Props) {
  const diaryNameEnum = diaryName as DiaryNameEnum;

  switch (diaryNameEnum) {
    case DiaryNameEnum.IntialDiary:
      return (
        <InitialDiaryFormTemplate
          columns={columns}
          record={record}
          handleInputChange={handleInputChange}
        />
      );
    case DiaryNameEnum.EmotionsDiary:
      return (
        <EmotionsDiaryFormTemplate
          columns={columns}
          record={record}
          handleInputChange={handleInputChange}
        />
      );
  }
}
