import React, { useEffect } from "react";
import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import DiaryDashBoard from "@src/features/dashboard/diaryDashboard";
import LoadingComponent from "./LoadingComponent";
import { useNavigate, useParams } from "react-router-dom";
import { IDescription } from "../models/description";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";
import { PagingParams } from "../models/pagination";

interface Props {
  setActiveDiary: (diary: IDescription) => void;
}

let descriptions = CreateDescriptions();

export function Diary({ setActiveDiary }: Props): JSX.Element {
  const { recordsStore, viewStore } = useStore();

  const { name, dateString } = useParams<{
    name: string;
    dateString: string;
  }>();

  const date = new Date(parseInt(dateString));

  recordsStore.diaryDescription = descriptions.get(name);
  viewStore.currDate = date;
  recordsStore.pagingParams = new PagingParams(1, name);

  useEffect(() => {
    setActiveDiary(recordsStore.diaryDescription);
    viewStore.watchDiary(recordsStore.diaryDescription.ShortName);
  }, []);

  const navigate = useNavigate();

  function onDateClick(date: Date) {
    navigate(`/diary/${name}/${date.getTime()}`, { replace: true });
    viewStore.currDate = date;
    recordsStore.loadRecords();
    viewStore.loadDates();
  }
  console.log("Diary is render");

  useEffect(() => {
    const fetchApi = async () => {
      await recordsStore.loadRecords();
      await viewStore.loadDates();
      await viewStore.watchDiary(recordsStore.diaryDescription.ShortName);
    };
    fetchApi();
  }, [recordsStore]);

  if (recordsStore.loading.isLoading) {
    return <LoadingComponent content={recordsStore.loading.message} />;
  }

  return <DiaryDashBoard onDateClick={onDateClick} />;
}

export default observer(Diary);
