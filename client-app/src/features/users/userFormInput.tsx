import { useField } from "formik";
import { Form, Label, Radio } from "semantic-ui-react";

interface Props {
  placeholder: string;
  name: string;
  type?: string;
  label?: string;
}

export default function UserTextInput(props: Props) {
  const [field, meta] = useField(props.name);
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <input {...field} {...props} />
      {meta.touched && meta.error ? (
        <Label basic color="red" style={{ marginTop: "10px" }}>
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
}

export function UserTextAreaInput(props: Props) {
  const [field, meta] = useField(props.name);
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <textarea {...field} {...props} />
      {meta.touched && meta.error ? (
        <Label basic color="red" style={{ marginTop: "10px" }}>
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
}

export function UserRadioInput(props: Props) {
  const [field, meta, helpers] = useField(props.name);
  const { setValue } = helpers;

  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <Radio
        toggle
        value={field.value}
        onClick={(event, data) => {
          setValue(data.checked);
        }}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red" style={{ marginTop: "10px" }}>
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
}
