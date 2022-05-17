import { ColumnTypeEnum } from "@src/app/enums/ColumnEnum";
import SemanticDatepicker from "react-semantic-ui-datepickers";
import { Form, Icon, Input, Label, Segment, TextArea } from "semantic-ui-react";
import { FormProps } from "./formTemplate";

export default function EmotionsDiaryFormTemplate({
  columns,
  record,
  handleInputChange,
}: FormProps) {
  var forms = columns.map((value) => {
    if (value.ValueType == ColumnTypeEnum.Date) {
      return (
        <div className="date field">
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
    return (
      <Form.Field
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
