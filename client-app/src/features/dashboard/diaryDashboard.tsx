import { IDescription } from "@src/app/models/description";
import { IDiary } from "@src/app/models/diary";
import React, { useState } from "react";
import { Button } from "semantic-ui-react";
import { getDescriptionColumnArray } from "@src/lib/CreateDescriptions";
import DiaryForm from "../form/diaryForm";
import DashBoardTemplate from "../templates/dashboards/dashboardTemplate";

interface Props {
  description: IDescription;
  records: Array<IDiary>;
  selectedRecord: IDiary | undefined;
  selectRecord: (id: number) => void;
  cancelSelectRecord: () => void;
  closeForm: () => void;
  editMode: boolean;
  openForm: (id?: number) => void;
  createOrEdit: (record: IDiary) => void;
  deleteRecord: (record: IDiary) => void;
}

export default function DiaryDashBoard({
  description,
  records,
  selectedRecord,
  openForm,
  closeForm,
  editMode,
  createOrEdit,
  deleteRecord,
}: Props): JSX.Element {
  let descrArray = getDescriptionColumnArray(description.ShortName);

  console.log("diaryDashBoard render is started");
  return (
    <div className="Diary">
      <div className="DiaryContent">
        <DashBoardTemplate
          description={description}
          records={records}
          openForm={openForm}
        />
      </div>
      <Button
        positive
        content="Добавить запись"
        onClick={() => openForm()}
        size={"medium"}
      ></Button>
      {editMode && (
        <DiaryForm
          closeForm={closeForm}
          selectedRecord={selectedRecord}
          columns={descrArray}
          diaryName={description.ShortName}
          createOrEdit={createOrEdit}
          deleteRecord={deleteRecord}
        />
      )}
    </div>
  );
}
