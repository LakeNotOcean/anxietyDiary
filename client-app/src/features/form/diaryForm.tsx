import { IColumn } from "@src/app/models/column";
import { Button, Form, Modal, Segment, TextArea } from "semantic-ui-react";
import React, {
  ChangeEvent,
  ChangeEventHandler,
  useEffect,
  useState,
} from "react";
import { createInitialDiary } from "@src/lib/initialDiary";
import FormTemplate from "../templates/forms/formTemplate";
import { useStore } from "@src/app/stores/store";
import { observer } from "mobx-react-lite";
import { IDiary } from "@src/app/models/diary";
import { toJS } from "mobx";

interface Props {
  columns: Array<IColumn>;
  diaryName: string;
}

export default observer(function DiaryForm({
  columns,
  diaryName,
}: Props): JSX.Element {
  const { recordsStore } = useStore();
  const { selectedRecord, closeForm, createOrEditRecord, deleteRecord } =
    recordsStore;

  const initialState = selectedRecord ?? createInitialDiary(diaryName);

  const [record, setRecord] = useState(initialState);

  function handleInputChange(event: ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;
    if (name == "" || value == ";") {
      return;
    }

    const res = structuredClone(toJS(record));
    res.Columns.set(name, value);
    console.log("result", res);
    setRecord(res);
  }
  function handleCustomInputChange(data: IDiary) {
    data = toJS(data);
    const res = structuredClone(data);
    setRecord(res);
  }

  function handleSubmit() {
    createOrEditRecord(record);
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
    <Modal dimmer="blurring" open={true} closeIcon onClose={closeForm}>
      <Modal.Header text>Заполните форму</Modal.Header>
      <Modal.Content>
        <div className="diary-form">
          <div className="diary-input">
            <FormTemplate
              diaryName={diaryName}
              columns={columns}
              record={record}
              handleInputChange={handleInputChange}
              handleCustomInputChange={handleCustomInputChange}
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
});
