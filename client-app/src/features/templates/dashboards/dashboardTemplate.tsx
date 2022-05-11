import { IDescription } from "@src/app/models/description";
import { IDiary } from "@src/app/models/diary";
import InitialDiaryTemplate from "./initialDiaryTemplate";
import { DiaryNameEnum } from "@src/app/enums/DiaryEnum";
import React from "react";
import EmotionsDiaryTemplate from "./emotionsDiaryTemplate";

interface Props {
  description: IDescription;
  records: Array<IDiary>;
  openForm: (id?: number) => void;
}

export interface dashBoardProps {
  description: IDescription;
  records: Array<IDiary>;
  openForm: (id?: number) => void;
}

export default function DashBoardTemplate({
  records,
  openForm,
  description,
}: Props): JSX.Element {
  const diaryName = description.ShortName as DiaryNameEnum;
  switch (diaryName) {
    case DiaryNameEnum.IntialDiary:
      return (
        <InitialDiaryTemplate
          description={description}
          records={records}
          openForm={openForm}
        />
      );
    case DiaryNameEnum.EmotionsDiary:
      return (
        <EmotionsDiaryTemplate
          description={description}
          records={records}
          openForm={openForm}
        />
      );
  }
}
