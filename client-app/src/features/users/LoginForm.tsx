import { UserLoginFormValues } from "@src/app/models/user";
import { useStore } from "@src/app/stores/store";
import { ErrorMessage, Form, Formik, FormikErrors, FormikValues } from "formik";
import { observer } from "mobx-react-lite";
import { useSyncExternalStore } from "react";
import { Button, Label } from "semantic-ui-react";
import RegisterForm from "./RegisterForm";
import UserTextInput from "./userFormInput";

export default observer(function LoginForm() {
  const { modalStore, userStore } = useStore();
  const validate = (values: UserLoginFormValues) => {
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
    }
    return errors;
  };
  return (
    <Formik
      validate={validate}
      initialValues={{
        email: "",
        password: "",
      }}
      onSubmit={(values, { setErrors, setSubmitting }) => {
        setSubmitting(true);
        userStore.login(values).catch((error) => {
          setErrors({
            email: "Неверный email или пароль",
            password: "Неверный email или пароль",
          });
        });
        setSubmitting(false);
      }}
    >
      {({ handleSubmit, isSubmitting, dirty, isValid, errors }) => {
        return (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <UserTextInput name="email" placeholder="email" label="Email" />
            <UserTextInput
              name="password"
              placeholder="пароль"
              type="password"
              label="Введите пароль"
            />
            <ErrorMessage
              name="error"
              render={() => (
                <Label
                  style={{ marginBotton: "10px", marginTop: "10px" }}
                  basic
                  color="red"
                  content={
                    (errors.email || errors.password) &&
                    "Неверный логин или пароль"
                  }
                ></Label>
              )}
            />

            <Label
              onClick={() =>
                modalStore.openModal(<RegisterForm />, "Создание аккаунта")
              }
            >
              Нет аккаунта?
            </Label>
            <Button
              disabled={!isValid || isSubmitting}
              positive
              content={isSubmitting ? "Выполняется вход" : "Войти"}
              type="submit"
              fuild
            />
          </Form>
        );
      }}
    </Formik>
  );
});
