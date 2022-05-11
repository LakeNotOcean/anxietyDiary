import { IColumn } from "@src/app/models/column";
import { IDiary } from "@src/app/models/diary";
import { Button, Form, Modal, Segment, TextArea } from "semantic-ui-react";
import React, {
  ChangeEvent,
  ChangeEventHandler,
  useEffect,
  useState,
} from "react";
import { createInitialDiary } from "@src/lib/initialDiary";
import cloneDiary from "@src/lib/cloneDiary";
import FormTemplate from "../templates/forms/formTemplate";

interface Props {
  columns: Array<IColumn>;
  selectedRecord: IDiary | undefined;
  closeForm: () => void;
  diaryName: string;
  createOrEdit: (record: IDiary) => void;
  deleteRecord: (record: IDiary) => void;
}

export default function DiaryForm({
  columns,
  selectedRecord,
  closeForm,
  diaryName,
  createOrEdit,
  deleteRecord,
}: Props): JSX.Element {
  const initialState = selectedRecord ?? createInitialDiary(diaryName);

  const [record, setRecord] = useState(initialState);

  function handleInputChange(event: ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;
    let res = structuredClone(record);
    res.Columns.set(name, value);
    setRecord(res);
  }

  function handleSubmit() {
    createOrEdit(record);
  }
  function handleDeleteRecord() {
    deleteRecord(record);
  }

  var forms = columns.map((value) => {
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
    <Modal dimmer="blurring" open={true}>
      <Modal.Header text>Заполните форму</Modal.Header>
      <Modal.Content>
        <div className="diary-form">
          <div className="diary-input">
            <FormTemplate
              diaryName={diaryName}
              columns={columns}
              record={record}
              handleInputChange={handleInputChange}
            />
          </div>
          <Modal.Actions>
            <Button
              positive
              type="submit"
              content={`${record.Id ? "Изменить" : "Добавить"}`}
              onClick={handleSubmit}
            />
            <Button
              type="button"
              content="Отменить"
              onClick={() => {
                closeForm();
              }}
            />
            <Button
              negative
              type="button"
              content="Удалить"
              onClick={handleDeleteRecord}
            />
          </Modal.Actions>
        </div>
      </Modal.Content>
    </Modal>
  );
}
