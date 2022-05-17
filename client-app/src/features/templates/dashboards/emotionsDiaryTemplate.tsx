import { dashBoardProps } from "./dashboardTemplate";
import React from "react";
import { Table, TableBody, TableHeader } from "semantic-ui-react";
import MapToArray from "@src/lib/MapToArray";
import { getDescriptionColumnArray } from "@src/lib/CreateDescriptions";
import moment from "moment";

export default function ActivityDiaryTempalte({
  description,
  records,
  openForm,
}: dashBoardProps) {
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
          if (value instanceof Date) {
            return (
              <Table.Cell>{moment(value).locale("ru").format("LL")}</Table.Cell>
            );
          }
          return <Table.Cell>{value}</Table.Cell>;
        })}
      </Table.Row>
    );
  });
  return (
    // @ts-ignore
    <Table celled selectable color={"violet"}>
      <TableHeader className="ColumnsNames">
        <Table.Row>{tableHeader}</Table.Row>
      </TableHeader>

      <TableBody>{rows}</TableBody>
    </Table>
  );
}
