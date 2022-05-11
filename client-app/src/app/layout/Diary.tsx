import React, { Fragment, useEffect, useState } from "react";
import { IDiary } from "@src/app/models/diary";
import { diaryDeserialize } from "@src/lib/DiaryDeserialize";
import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import DiaryDashBoard from "@src/features/dashboard/diaryDashboard";
import agent from "../api/agent";
import LoadingComponent from "./LoadingComponent";
import diarySerialize from "@src/lib/DiarySerialize";
import { useParams } from "react-router-dom";
import { IDescription } from "../models/description";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";
import recordsStore, { ILoading } from "../stores/recordsStore";

interface Props {
  setActiveDiary: (diary: IDescription) => void;
}

let descriptions = CreateDescriptions();

export function Diary({ setActiveDiary }: Props): JSX.Element {
  const { recordsStore } = useStore();

  const [records, setRecords] = useState<IDiary[]>([]);
  const [selectedRecord, setSelectedRecord] = useState<IDiary | undefined>(
    undefined
  );
  const [editMode, setEditMode] = useState(false);
  const { name, dateString } = useParams<{
    name: string;
    dateString: string;
  }>();

  const date = new Date(parseInt(dateString));

  function handleCancelSelectedRecord() {
    setSelectedRecord(undefined);
  }

  function handleSelectRecord(id: number) {
    setSelectedRecord(records.find((r) => r.Id === id));
  }
  function handleFormOpen(id?: number) {
    id ? handleSelectRecord(id) : handleCancelSelectedRecord();
    setEditMode(true);
  }

  function handleFormClose() {
    handleCancelSelectedRecord();
    setEditMode(false);
  }

  function handleDeleteRecord(record: IDiary) {
    agent.records.delete(name, record.Id).then(() => {
      record.Id
        ? setRecords([...records.filter((r) => r.Id !== record.Id)])
        : {};
      recordsStore.closeForm();
      handleFormClose();
    });
  }

  function handleCreateOrEditRecord(record: IDiary) {
    if (record.Id) {
      const dto = {} as IUpdateRecord;
      dto.body = diarySerialize(record);
      agent.records.update(dto, name, record.Id).then(() => {
        setRecords([...records.filter((r) => r.Id !== record.Id), record]);
        handleFormClose();
      });
    } else {
      const dto = {} as IPostRecord;
      dto.body = diarySerialize(record);
      dto.name = name;
      console.log(dto);
      agent.records.create(dto).then((response) => {
        record.Id = response;
        setRecords([...records, record]);
        handleFormClose();
      });
    }
  }

  useEffect(() => {
    recordsStore.loadRecords();
  }, [recordsStore]);

  if (recordsStore.loading.isLoading) {
    return <LoadingComponent content={recordsStore.loading.message} />;
  }

  setActiveDiary(descriptions.get(name));

  return (
    <DiaryDashBoard
      description={descriptions.get(name)}
      selectRecord={handleSelectRecord}
      records={records}
      selectedRecord={selectedRecord}
      cancelSelectRecord={handleCancelSelectedRecord}
      editMode={editMode}
      openForm={handleFormOpen}
      closeForm={handleFormClose}
      createOrEdit={handleCreateOrEditRecord}
      deleteRecord={handleDeleteRecord}
    />
  );
}

export default observer(Diary);
