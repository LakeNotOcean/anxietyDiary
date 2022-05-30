import Diary from "./layout/Diary";
import NavBar from "./layout/NavBar";
import React, { ChangeEvent, useEffect, useState } from "react";
import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import { ToastContainer } from "react-toastify";
import { IDescription } from "./models/description";
import { Container } from "semantic-ui-react";
import { useRoutes } from "react-router-dom";
import DiariesList from "./layout/DiariesList";
import { observer } from "mobx-react-lite";
import NotFound from "@src/features/errors/NotFound";
import { useStore } from "./stores/store";
import LoadingComponent from "./layout/LoadingComponent";
import ModalContainer from "./layout/ModalContainer";
import LoginForm from "@src/features/users/LoginForm";
import DiaryInfo from "./layout/DiaryInfo";
import UserInfo from "@src/features/users/UserInfo";
import PatientList from "@src/features/users/PatientList";
import HomePage from "./layout/HomePage";

function App(): JSX.Element {
  console.log("app is active");
  const { commonStore, userStore, modalStore } = useStore();

  useEffect(() => {
    if (commonStore.token) {
      commonStore.setAppLoaded(true);
      userStore.loadUser().finally(() => {
        commonStore.setAppLoaded(false);
      });
    } else {
      commonStore.setAppLoaded(false);
    }
    if (userStore.isLoginForm) {
      modalStore.openModal(<LoginForm />, "Войти на сайт");
      userStore.setIsLoginForm(false);
    }
  }, [commonStore, userStore, modalStore.modal.open, userStore.isLoginForm]);

  if (commonStore.appLoaded) return <LoadingComponent content="Загрузка..." />;

  const routes = useRoutes([
    { path: "/", element: <HomePage /> },
    {
      path: "/diary/:name/:dateString",
      element: <Diary />,
    },
    {
      path: "/diaries",
      element: <DiariesList />,
    },
    {
      path: "*",
      element: <NotFound />,
    },
    {
      path: "/diaryinfo/:diaryName",
      element: <DiaryInfo />,
    },
    {
      path: "/account",
      element: <UserInfo />,
    },
    {
      path: "/patients",
      element: <PatientList />,
    },
  ]);

  return (
    <div className="App">
      <ModalContainer />
      <ToastContainer position="bottom-right" hideProgressBar />
      <NavBar />
      <div className="Content">
        <Container>{routes}</Container>
      </div>
    </div>
  );
}

export default observer(App);
