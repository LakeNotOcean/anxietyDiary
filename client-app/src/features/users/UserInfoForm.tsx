import { UserInfo } from "@src/app/models/user";
import { useStore } from "@src/app/stores/store";
import {
  Form,
  Formik,
  FormikErrors,
  FormikHelpers,
  FormikValues,
} from "formik";
import { observer } from "mobx-react-lite";
import { Button } from "semantic-ui-react";
import UserTextInput, {
  UserRadioInput,
  UserTextAreaInput,
} from "./userFormInput";

interface Props {
  userInfo: UserInfo;
}
export default observer(function UserInfoForm({ userInfo }: Props) {
  const { userStore } = useStore();
  const validate = (values: UserInfo) => {
    let errors: FormikErrors<FormikValues> = {};
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
    values: UserInfo,
    formikHelpers: FormikHelpers<UserInfo>
  ) => {
    formikHelpers.setSubmitting(true);
    userStore.changeUserInfo(values);
    formikHelpers.setSubmitting(false);
  };
  return (
    <Formik
      validate={validate}
      initialValues={userInfo}
      onSubmit={handleSubmit}
    >
      {({ handleSubmit, isSubmitting, dirty, isValid, errors }) => {
        return (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <UserTextInput name={"firstName"} placeholder="Имя" label="Имя" />
            <UserTextInput
              name={"secondName"}
              placeholder="Фамилия"
              label="Фамилия"
            />
            <UserRadioInput
              name={"isSearching"}
              label="Видимый для поиска"
              placeholder=""
            />
            <UserTextAreaInput
              name={"description"}
              label="О себе: "
              placeholder="Напишите полезную для психотерапевта информацию"
            />
            <Button
              disable={!isValid || !dirty || isSubmitting}
              positive
              content={isSubmitting ? "Изменение..." : "Изменить"}
              type="submit"
              fuild
            />
          </Form>
        );
      }}
    </Formik>
  );
});
