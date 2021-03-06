import { getDescriptionColumnArray } from "@src/lib/CreateDescriptions";
import React, { useState } from "react";
import { dashBoardProps } from "./dashboardTemplate";

export default function InitialDiaryTemplate({
  description,
  records,
  openForm,
}: dashBoardProps) {
  let descrArray = getDescriptionColumnArray(description.ShortName);
  const column = descrArray.map((column) => {
    return (
      <div className="initial-list-field" key={column.ShortName}>
        <div className="initial-list-header">
          <h5>{column.Name}</h5>
        </div>
        <div className="initial-list-content">
          {records[0]?.Columns.get(column.ShortName) as string}
        </div>
      </div>
    );
  });
  return (
    <div className="initial-list" onClick={() => openForm(records[0]?.Id)}>
      {column}
    </div>
  );
}
