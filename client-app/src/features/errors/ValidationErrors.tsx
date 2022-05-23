import { Message } from "semantic-ui-react";

interface Props {
  errors: string[] | null;
}

export default function ValidationErrors({ errors }: Props) {
  let errorsList = errors.map((err, key) => {
    if (err !== null) return <Message.Item key={key}>{err}</Message.Item>;
  });
  return (
    <Message
      error
      style={{ display: "block", marginBotton: "10px", marginTop: "10px" }}
    >
      <Message.List>{errors && errorsList}</Message.List>
    </Message>
  );
}
