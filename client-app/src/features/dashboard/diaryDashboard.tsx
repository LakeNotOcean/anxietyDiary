import React, { useEffect, useState } from "react";
import { Button, Pagination, PaginationProps } from "semantic-ui-react";
import { getDescriptionColumnArray } from "@src/lib/CreateDescriptions";
import DiaryForm from "../form/diaryForm";
import DashBoardTemplate from "../templates/dashboards/dashboardTemplate";
import { useStore } from "@src/app/stores/store";
import { observer } from "mobx-react-lite";
import { PagingParams } from "@src/app/models/pagination";
import LoadingComponent from "@src/app/layout/LoadingComponent";
import Calendar, { CalendarTileProperties } from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { isSameDay } from "date-and-time";

interface DashBoardProps {
  onDateClick: (date: Date) => void;
}
export default observer(function DiaryDashBoard({
  onDateClick,
}: DashBoardProps): JSX.Element {
  const { recordsStore, viewStore } = useStore();
  const {
    diaryDescription,
    records,
    openForm,
    editMode,
    setPagingParams,
    pagination,
    loadRecords,
  } = recordsStore;

  const { dates, currDate, isAnotherUser, getDiaryLastViewDate } = viewStore;

  const [loading, setLoading] = useState(false);

  const diaryName = diaryDescription.ShortName;

  function tileClassName({ date, view }: CalendarTileProperties) {
    // Add class to tiles in month view only
    if (dates === null) {
      return;
    }
    const dateString = date.toDateString();
    if (view === "month") {
      if (dateString == new Date().toDateString())
        return "calendar-day current-day";
      if (dateString == currDate.toDateString())
        return "calendar-day chosen-day";
      // Check if a date React-Calendar wants to check is on the list of dates to add class to
      if (dates.find((d) => d.toDateString() == date.toDateString())) {
        let dateClassStr = "calendar-day record-day";
        if (isAnotherUser()) {
          const lastViewDate = getDiaryLastViewDate(diaryName);
          console.log("lastViewDate", lastViewDate);
          if (date > lastViewDate) {
            dateClassStr = dateClassStr.concat(" ", "notview-day");
            console.log(dateClassStr);
          }
        }
        return dateClassStr;
      }
    }
  }

  function handleLoadPage(
    event: React.MouseEvent<HTMLAnchorElement, MouseEvent>,
    data: PaginationProps
  ) {
    setLoading(true);
    setPagingParams(
      new PagingParams(+data.activePage, diaryDescription.ShortName)
    );
    loadRecords().then(() => setLoading(false));
  }

  let descrArray = getDescriptionColumnArray(diaryDescription.ShortName);

  if (loading) {
    return <LoadingComponent content={"Загрузка записей..."} />;
  }

  return (
    <>
      <div className="Diary">
        <div className="DiaryContent">
          <DashBoardTemplate
            description={diaryDescription}
            records={records}
            openForm={openForm}
          />
        </div>
        <div className="content-controller">
          <Calendar tileClassName={tileClassName} onClickDay={onDateClick} />
          {!viewStore.isAnotherUser() && (
            <Button
              positive
              content="Добавить запись"
              onClick={() => openForm()}
              size={"medium"}
            ></Button>
          )}
        </div>

        {editMode && (
          <DiaryForm
            columns={descrArray}
            diaryName={diaryDescription.ShortName}
          />
        )}
      </div>
      <div className="pagination">
        {pagination && (
          <Pagination
            activePage={pagination.currentPage}
            boundaryRange={1}
            onPageChange={handleLoadPage}
            siblingRange={1}
            totalPages={pagination.totalPages}
          />
        )}
      </div>
    </>
  );
});
