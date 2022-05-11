import React, { Fragment, useEffect, useRef, useState } from "react";
import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import DiaryDashBoard from "@src/features/dashboard/diaryDashboard";
import LoadingComponent from "./LoadingComponent";
import { useParams } from "react-router-dom";
import { IDescription } from "../models/description";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";
import { PagingParams } from "../models/pagination";

interface Props {
  setActiveDiary: (diary: IDescription) => void;
}

let descriptions = CreateDescriptions();

export function Diary({ setActiveDiary }: Props): JSX.Element {
  const { recordsStore } = useStore();

  const { name, dateString } = useParams<{
    name: string;
    dateString: string;
  }>();

  const date = new Date(parseInt(dateString));

  recordsStore.diaryDescription = descriptions.get(name);
  recordsStore.date = date;
  recordsStore.pagingParams = new PagingParams(1, name);

  useEffect(() => {
    setActiveDiary(recordsStore.diaryDescription);
  }, []);

  useEffect(() => {
    recordsStore.loadRecords();
  }, [recordsStore]);

  if (recordsStore.loading.isLoading) {
    return <LoadingComponent content={recordsStore.loading.message} />;
  }

  return <DiaryDashBoard />;
}

export default observer(Diary);
