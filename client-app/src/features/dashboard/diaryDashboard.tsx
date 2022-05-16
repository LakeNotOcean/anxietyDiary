import React, { useState } from "react";
import { Button, Pagination, PaginationProps } from "semantic-ui-react";
import { getDescriptionColumnArray } from "@src/lib/CreateDescriptions";
import DiaryForm from "../form/diaryForm";
import DashBoardTemplate from "../templates/dashboards/dashboardTemplate";
import { useStore } from "@src/app/stores/store";
import { observer } from "mobx-react-lite";
import { PagingParams } from "@src/app/models/pagination";
import LoadingComponent from "@src/app/layout/LoadingComponent";
import Calendar from "react-awesome-calendar";

export default observer(function DiaryDashBoard(): JSX.Element {
  const { recordsStore } = useStore();
  const {
    diaryDescription,
    records,
    openForm,
    editMode,
    setPagingParams,
    pagination,
    loadRecords,
  } = recordsStore;

  const [loading, setLoading] = useState(false);

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
        <Calendar />
        <Button
          positive
          content="Добавить запись"
          onClick={() => openForm()}
          size={"medium"}
        ></Button>

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
