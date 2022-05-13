import { ColumnTypeEnum } from "@src/app/enums/ColumnEnum";
import { HumanBodyEnum } from "@src/app/enums/HumanBodyEnum";
import { ButtonState } from "@src/app/models/buttonState";
import useComponentVisible from "@src/lib/componentVisible";
import { getDescriptionColumnArray } from "@src/lib/CreateDescriptions";
import MapToArray, { MapToArrayKeys } from "@src/lib/MapToArray";
import { toJS } from "mobx";
import moment from "moment";
import React, { useEffect } from "react";
import { useRef, useState } from "react";
import { BodyComponent } from "reactjs-human-body";
import {
  Button,
  ButtonProps,
  Form,
  Message,
  Tab,
  Table,
  TableBody,
  TableHeader,
  TextArea,
} from "semantic-ui-react";
import { dashBoardProps } from "./dashboardTemplate";

export default function HumanBodyDiaryTemplate({
  description,
  records,
  openForm,
}: dashBoardProps) {
  records = toJS(records);
  const [textShow, setTextShow] = useState<string | undefined>(undefined);

  const [xCoord, setXCoord] = useState(0);
  const [yCoord, setYCoord] = useState(0);

  const refs = useRef(new Array<HTMLDivElement>(records.length));
  const { buttonStates, handleOnReactionClick } = useComponentVisible(
    new Array(records.length).fill(new ButtonState())
  );

  const HumanBodyColumn = MapToArray(description.Columns).find(
    (c) => c.ValueType == ColumnTypeEnum.Json
  ).ShortName;

  function handleOnclick(
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    bodyPart: HumanBodyEnum,
    x: number,
    y: number,
    refId: number
  ) {
    event.stopPropagation();

    const value = records[refId].Columns.get(HumanBodyColumn);

    value[bodyPart]["selected"] =
      value[bodyPart]["selected"] == true ? false : true;

    if (value[bodyPart]["selected"] == false) {
      return;
    }
    value[bodyPart]["text"] !== undefined
      ? setTextShow(value[bodyPart]["text"])
      : setTextShow(undefined);
    let parent = refs.current[refId]?.getBoundingClientRect();
    setXCoord(x - parent.left);
    setYCoord(y - parent.top);
  }

  let descrArray = getDescriptionColumnArray(description.ShortName);

  const tableHeader = descrArray.map((column) => (
    <Table.HeaderCell key={column.ShortName}>{column.Name}</Table.HeaderCell>
  ));

  const recordsValueArray = records.map((record) => {
    return MapToArray(record.Columns);
  });
  const recordsKeysArray = MapToArrayKeys(description.Columns);

  const rows = records.map((record, firstKey) => {
    const bodyValue = record.Columns.get(HumanBodyColumn);
    return (
      <>
        <Table.Row
          onClick={() => {
            openForm(record.Id);
          }}
          key={firstKey}
        >
          {recordsValueArray[firstKey].map((value, key) => {
            if (
              value instanceof Date &&
              description.Columns.get(recordsKeysArray[key]).ShortName ==
                "column1"
            ) {
              return (
                <Table.Cell key={key}>
                  {moment(value).locale("ru").format("LL")}
                </Table.Cell>
              );
            }
            if (
              value instanceof Date &&
              description.Columns.get(recordsKeysArray[key]).ShortName ==
                "column2"
            ) {
              return (
                <Table.Cell key={key}>
                  {moment(value).locale("ru").format("LT")}
                </Table.Cell>
              );
            }
            if (
              description.Columns.get(recordsKeysArray[key]).ValueType ==
              ColumnTypeEnum.Json
            ) {
              return (
                <Table.Cell textAlign="center" key={key}>
                  <Button
                    primary
                    key={firstKey}
                    content={buttonStates[firstKey].content}
                    onClick={(event, data) =>
                      handleOnReactionClick(event, data, firstKey)
                    }
                  />
                </Table.Cell>
              );
            }
            return <Table.Cell key={key}>{value}</Table.Cell>;
          })}
        </Table.Row>
        {buttonStates[firstKey].isActive && (
          <Table.Row
            as={"div"}
            style={{ height: "550px" }}
            active={false}
            key={`body-${firstKey}`}
          >
            <div
              className="human-body-content"
              ref={(el) => (refs.current[firstKey] = el)}
              key={firstKey}
            >
              <BodyComponent
                partsInput={bodyValue as Object}
                onClick={(
                  event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                  bodyPart: HumanBodyEnum,
                  x: number,
                  y: number
                ) => handleOnclick(event, bodyPart, x, y, firstKey)}
              />
              {textShow !== undefined && (
                <HumanBodyText
                  message={textShow}
                  xCoord={xCoord}
                  yCoord={yCoord}
                />
              )}
            </div>
          </Table.Row>
        )}
      </>
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

export interface HumanProps {
  message: string | undefined;
  xCoord: number;
  yCoord: number;
}
function HumanBodyText({ message, xCoord, yCoord }: HumanProps) {
  console.log(message);
  return (
    <div
      className="HumanBodyText"
      style={{
        position: "absolute",
        left: `${xCoord}px`,
        top: `${yCoord}px`,
        overflow: "hidden",
        zIndex: "99999",
      }}
    >
      {/*
        // @ts-ignore */}
      <Form>
        <TextArea placeholder="" value={message} />
      </Form>
    </div>
  );
}
