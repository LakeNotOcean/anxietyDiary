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
  const [loading, setLoading] = useState({
    isLoading: true,
    message: "Загрузка записей...",
  });
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
    setLoading({ isLoading: true, message: "Удаление записи..." });
    agent.records.delete(name, record.Id).then(() => {
      record.Id
        ? setRecords([...records.filter((r) => r.Id !== record.Id)])
        : {};
      setLoading({ isLoading: false, message: "" });
      handleFormClose();
    });
  }

  function handleCreateOrEditRecord(record: IDiary) {
    if (record.Id) {
      setLoading({ isLoading: true, message: "Изменение записи..." });
      const dto = {} as IUpdateRecord;
      dto.body = diarySerialize(record);
      agent.records.update(dto, name, record.Id).then(() => {
        setRecords([...records.filter((r) => r.Id !== record.Id), record]);
        setLoading({ isLoading: false, message: "" });
        handleFormClose();
      });
    } else {
      setLoading({ isLoading: true, message: "Добавление записи..." });
      const dto = {} as IPostRecord;
      dto.body = diarySerialize(record);
      dto.name = name;
      console.log(dto);
      agent.records.create(dto).then((response) => {
        record.Id = response;
        setRecords([...records, record]);
        handleFormClose();
        setLoading({ isLoading: false, message: "" });
      });
    }
  }

  useEffect(() => {
    agent.records.list(name, date, 1, 10).then((response) => {
      setRecords(diaryDeserialize(response.data, descriptions.get(name)));
      setLoading({ isLoading: false, message: "" });
    });
  }, []);

  if (loading.isLoading) {
    return <LoadingComponent content={loading.message} />;
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
