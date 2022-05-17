import { Form, Formik } from "formik";
import { Button } from "semantic-ui-react";
import UserTextInput from "./userFormInput";

export default function LoginForm() {
  return (
    <Formik
      initialValues={{
        email: "",
        password: "",
      }}
      onSubmit={(values) => console.log(values)}
    >
      {({ handleSubmit }) => (
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <UserTextInput name="email" placeholder="email" label="Email" />
          <UserTextInput
            name="password"
            placeholder="пароль"
            type="password"
            label="Введите пароль"
          />
          <Button positive content="Создать пользователя" type="submit" fuild />
        </Form>
      )}
    </Formik>
  );
}
