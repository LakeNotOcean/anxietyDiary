import { Form, Formik } from "formik";
import { Button } from "semantic-ui-react";
import UserTextInput from "./userFormInput";

export default function RegisterForm() {
  return (
    <Formik
      initialValues={{
        email: "",
        password: "",
        repeatPassword: "",
        userName: "",
        firstName: "",
        secondName: "",
      }}
      onSubmit={(values) => console.log(values)}
    >
      {({ handleSubmit }) => (
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <UserTextInput
            name="userName"
            placeholder="Введите пользователя"
            label="Имя пользователя"
          />
          <UserTextInput name="email" placeholder="email" label="Email" />
          <UserTextInput
            name="password"
            placeholder="пароль"
            type="password"
            label="Введите пароль"
          />
          <UserTextInput
            name="repeatPassword"
            placeholder="пароль"
            type="password"
            label="Повторите пароль"
          />
          <UserTextInput
            name="firstName"
            placeholder="Необязательно"
            label="Имя"
          />
          <UserTextInput
            name="secondName"
            placeholder="Необязательно"
            label="Фамилия"
          />
          <Button positive content="Создать пользователя" type="submit" fuild />
        </Form>
      )}
    </Formik>
  );
}
