import { UserRegisterRepFormValues } from "@src/app/models/user";
import { useStore } from "@src/app/stores/store";
import {
  ErrorMessage,
  Form,
  Formik,
  FormikErrors,
  FormikHelpers,
  FormikValues,
} from "formik";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Button } from "semantic-ui-react";
import ValidationErrors from "../errors/ValidationErrors";
import UserTextInput from "./userFormInput";

export default observer(function RegisterForm() {
  const { userStore } = useStore();
  const validate = (values: UserRegisterRepFormValues) => {
    let errors: FormikErrors<FormikValues> = {};
    if (!values.email) {
      errors.email = "Необходим email";
    } else if (
      !/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.email)
    ) {
      errors.email = "Неверный email";
    }
    if (!values.password) {
      errors.password = "Необходим пароль";
    } else if (values.password.length < 5) {
      errors.password = "Пароль слишком короткий";
    } else if (values.password.length > 60) {
      errors.password = "Пароль слишком длинный";
    } else if (!/^([\x00-\x7F]){5,60}$/i.test(values.password)) {
      errors.password =
        "Пароль должен состоять из латинских букв, цифр или специальных символов";
    } else if (values.password != values.repeatPassword) {
      errors.repeatPassword = "Пароли не совпадают";
    }
    if (!values.userName) {
      errors.userName = "Необходимо имя пользователя";
    } else if (values.userName.length < 3) {
      errors.userName = "Имя пользователя слишком короткое";
    } else if (values.userName.length > 10) {
      errors.userName = "Имя пользователя слишком длинное";
    } else if (!/^([A-Za-z0-9]){3,10}$/i.test(values.userName)) {
      errors.userName =
        "Имя пользователя должно состоять из латинских букв и цифр";
    }
    if (
      values.firstName.length != 0 &&
      !/(^[\u0400-\u04FF]+$)|(^[a-zA-Z]+$)/i.test(values.firstName)
    ) {
      errors.firstName =
        "Имя должно состоять только из латинских или кириллических букв";
    }
    if (
      values.secondName.length != 0 &&
      !/(^[\u0400-\u04FF]+$)|(^[a-zA-Z]+$)/i.test(values.secondName)
    ) {
      errors.secondName =
        "Фамилия должна состоять только из латинских или кириллических букв";
    }

    return errors;
  };

  const handleSubmit = async (
    values: UserRegisterRepFormValues,
    formikHelpers: FormikHelpers<UserRegisterRepFormValues>
  ) => {
    let emailError = null;
    let userNameError = null;
    formikHelpers.setSubmitting(true);
    await userStore
      .registerUser(values)
      .catch((error) => {
        const errors = error.errors;
        if (errors?.email !== undefined) emailError = errors?.email[0];
        if (errors?.userName !== undefined) userNameError = errors?.userName[0];
      })
      .finally(() => formikHelpers.setSubmitting(false));
    formikHelpers.setErrors({
      errors: [emailError, userNameError],
    } as FormikErrors<UserRegisterRepFormValues>);
  };
  return (
    <Formik
      validate={validate}
      initialValues={{
        email: "",
        password: "",
        repeatPassword: "",
        userName: "",
        firstName: "",
        secondName: "",
        errors: null,
      }}
      onSubmit={handleSubmit}
    >
      {({ handleSubmit, isSubmitting, dirty, isValid, errors }) => {
        return (
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
            <Button
              disable={!isValid || !dirty || isSubmitting}
              positive
              content={
                isSubmitting ? "Создание аккаунта..." : "Создать аккаунт"
              }
              type="submit"
              fuild
            />
            <ErrorMessage
              name="errors"
              render={() => {
                return <ValidationErrors errors={errors.errors as string[]} />;
              }}
            ></ErrorMessage>
          </Form>
        );
      }}
    </Formik>
  );
});
