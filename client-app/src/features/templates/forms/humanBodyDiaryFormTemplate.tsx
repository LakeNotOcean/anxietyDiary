import { ColumnTypeEnum } from "@src/app/enums/ColumnEnum";
import SemanticDatepicker from "react-semantic-ui-datepickers";
import { Form, Icon, Segment, TextArea } from "semantic-ui-react";
import { FormProps } from "./formTemplate";
import { TimeInput } from "semantic-ui-react-datetimeinput";
import React, { ChangeEvent, useState, useRef } from "react";
import { BodyComponent } from "reactjs-human-body";
import { HumanBodyEnum } from "@src/app/enums/HumanBodyEnum";
import useComponentVisible from "@src/lib/componentVisible";
import { toJS } from "mobx";

export default function HumanBodyFormTemplate({
  columns,
  record,
  handleInputChange,
  handleCustomInputChange,
}: FormProps) {
  const [textShow, setTextShow] = useState<string | undefined>(undefined);
  const [bodyPart, setBodypart] = useState<string | undefined>(undefined);
  const [xCoord, setXCoord] = useState(0);
  const [yCoord, setYCoord] = useState(0);
  const ref = useRef<HTMLDivElement>(null);

  const HumanBodyColumn = columns.find(
    (c) => c.ValueType == ColumnTypeEnum.Json
  ).ShortName;

  const humanBodyParams = record.Columns.get(HumanBodyColumn) as Object;

  function handleOnclick(
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    part: HumanBodyEnum,
    x: number,
    y: number
  ) {
    console.log(`part is ${part} with `, humanBodyParams);
    if (humanBodyParams[part]["selected"] == true) {
      humanBodyParams[part]["selected"] = false;

      setBodypart(undefined);
      record.Columns.set(HumanBodyColumn, humanBodyParams);
      handleCustomInputChange(record);
      setTextShow(undefined);

      return;
    } else {
      humanBodyParams[part]["selected"] = true;
      console.log(`${part} is now true`);

      setBodypart(part);
      setTextShow(humanBodyParams[part]["text"]);
      record.Columns.set(HumanBodyColumn, humanBodyParams);
      handleCustomInputChange(record);
    }

    let parent = ref.current?.getBoundingClientRect();
    setXCoord(x - parent.left);
    setYCoord(y - parent.top);
  }

  function handleHumanBodyChange(event: ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;
    humanBodyParams[name]["text"] = value;

    record.Columns.set(HumanBodyColumn, humanBodyParams);
    setTextShow(humanBodyParams[name]["text"]);
    handleCustomInputChange(record);
  }

  function handleTimeChange(date: Date) {
    record.Columns.set("column2", date);
    handleCustomInputChange(record);
  }

  var forms = columns.map((value, key) => {
    if (
      value.ValueType == ColumnTypeEnum.Date &&
      value.ShortName == "column1"
    ) {
      return (
        <div className="date field" key={key}>
          <label>{`${value.Name}`}</label>
          <SemanticDatepicker
            icon={<Icon name="calendar alternate outline" />}
            locale={"ru-RU"}
            onChange={handleInputChange}
            value={record.Columns.get(value.ShortName) as Date}
          />
        </div>
      );
    }
    if (
      value.ValueType == ColumnTypeEnum.Date &&
      value.ShortName == "column2"
    ) {
      return (
        <div className="time field" key={key}>
          <label>{`${value.Name}`}</label>
          <TimeInput
            showTooltips={false}
            dateValue={record.Columns.get(value.ShortName) as Date}
            onDateValueChange={handleTimeChange}
          />
        </div>
      );
    }
    if (value.ValueType == ColumnTypeEnum.Json) {
      return (
        <div className="human-body-content" key={key} ref={ref}>
          <label>{`${value.Name}`}</label>
          <BodyComponent
            partsInput={structuredClone(
              record.Columns.get(value.ShortName) as Object
            )}
            onClick={handleOnclick}
          />
          {textShow !== undefined && (
            <HumanBodyTextInput
              message={textShow}
              xCoord={xCoord}
              yCoord={yCoord}
              bodyPart={bodyPart}
              handleInputChange={handleHumanBodyChange}
            />
          )}
        </div>
      );
    }
    return (
      <Form.Field
        control={TextArea}
        label={value.Name}
        placeholder={value.Name}
        name={value.ShortName}
        value={record.Columns.get(value.ShortName)}
        onChange={handleInputChange}
        key={key}
      />
    );
  });
  return (
    // @ts-ignore
    <Segment clearing>
      {/*
            // @ts-ignore */}
      <Form autoComplete="off">{forms}</Form>
    </Segment>
  );
}

interface HumanFormProps {
  message: string | undefined;
  xCoord: number;
  yCoord: number;
  bodyPart: string | undefined;
  handleInputChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

function HumanBodyTextInput({
  message,
  xCoord,
  yCoord,
  bodyPart,
  handleInputChange,
}: HumanFormProps) {
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
      <Form.Field
        control={TextArea}
        value={message}
        name={bodyPart}
        onChange={handleInputChange}
      />
    </div>
  );
}
