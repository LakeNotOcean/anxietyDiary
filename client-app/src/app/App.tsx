import Diary from "./layout/Diary";
import NavBar from "./layout/NavBar";
import React, { ChangeEvent, useState } from "react";
import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import { ToastContainer } from "react-toastify";
import { IDescription } from "./models/description";
import { Container } from "semantic-ui-react";
import {
  BrowserRouter,
  Route,
  Router,
  Routes,
  useRoutes,
} from "react-router-dom";
import DiariesList from "./layout/DiariesList";
import { observer } from "mobx-react-lite";
import NotFound from "@src/features/errors/NotFound";
import RegisterForm from "@src/features/users/RegisterForm";

const descriptions = CreateDescriptions();

function App(): JSX.Element {
  console.log("app is active");
  const [activeDiary, setActiveDiary] = useState<IDescription>(null);

  function handleSetActiveDiary(diary: IDescription) {
    setActiveDiary(diary);
  }
  const routes = useRoutes([
    { path: "/", element: <div>HomePage</div> },
    {
      path: "/diary/:name/:dateString",
      element: <Diary setActiveDiary={handleSetActiveDiary} />,
    },
    {
      path: "/diaries",
      element: <DiariesList />,
    },
    {
      path: "/register",
      element: <RegisterForm />,
    },
    {
      path: "*",
      element: <NotFound />,
    },
  ]);

  return (
    <div className="App">
      <ToastContainer position="bottom-right" hideProgressBar />
      <NavBar selectedDiary={activeDiary?.Name} />
      <div className="Content">
        <Container>{routes}</Container>
      </div>
    </div>
  );
}

export default observer(App);
