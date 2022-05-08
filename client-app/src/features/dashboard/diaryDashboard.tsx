import { IDescription } from "@src/app/models/description";
import { IDiary } from "@src/app/models/diary";
import MapToArray from "@src/lib/MapToArray";
import React, { useState } from "react";
import { Button, Table, TableBody, TableHeader } from "semantic-ui-react";
import { getDescriptionColumnArray } from "@src/lib/CreateDescriptions";
import DiaryForm from "../form/diaryForm";

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

  const tableHeader = descrArray.map((column) => (
    <Table.HeaderCell>{column.Name}</Table.HeaderCell>
  ));

  const rows = records.map((record) => {
    return (
      <Table.Row
        onClick={() => {
          openForm(record.Id);
        }}
      >
        {MapToArray(record.Columns).map((value, key) => {
          return <Table.Cell>{value}</Table.Cell>;
        })}
      </Table.Row>
    );
  });

  console.log("diaryDashBoard render is started");
  return (
    <div className="Diary">
      <div className="DiaryContent">
        {/*
            // @ts-ignore */}
        <Table celled selectable color={"violet"}>
          <TableHeader className="ColumnsNames">
            <Table.Row>{tableHeader}</Table.Row>
          </TableHeader>

          <TableBody>{rows}</TableBody>
        </Table>
      </div>
      <Button
        positive
        content="Добавить запись"
        onClick={() => openForm()}
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
