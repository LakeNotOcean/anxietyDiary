import { IColumn } from "@src/app/models/column";
import { IDiary } from "@src/app/models/diary";
import { ChangeEvent } from "react";
import { Form, Segment, TextArea } from "semantic-ui-react";
import React, { useState } from "react";
import { FormProps } from "./formTemplate";

export default function InitialDiaryFormTemplate({
  columns,
  record,
  handleInputChange,
}: FormProps) {
  var forms = columns.map((value) => {
    return (
      <Form.Field
        key={value.ShortName}
        control={TextArea}
        label={value.Name}
        placeholder={value.Name}
        name={value.ShortName}
        value={record.Columns.get(value.ShortName)}
        onChange={handleInputChange}
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
