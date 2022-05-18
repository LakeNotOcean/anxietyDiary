import React, { ReactElement, useState } from "react";
import { Button, Container, Header, Menu, Modal } from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import { NavLink } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";
import LoginForm from "@src/features/users/LoginForm";
import RegisterForm from "@src/features/users/RegisterForm";

interface Props {
  selectedDiary?: string;
}

export default observer(function NavBar({ selectedDiary }: Props): JSX.Element {
  const { userStore, modalStore } = useStore();

  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item header>
          <img
            src="./images/psychology.svg"
            alt="logo"
            style={{
              marginRight: "15px",
              transform: "scale(1.5)",
              filter: "invert(100%)",
            }}
          />
          Healthy Mind Project
        </Menu.Item>
        <Menu.Item>
          <Button positive content="Дневники" as={NavLink} to={"/diaries"} />
        </Menu.Item>
        {userStore.isLoggedIn && (
          <>
            <Menu.Item>
              <Button
                onClick={() => userStore.logout()}
                primary
                content="Выйти"
              />
            </Menu.Item>
            <Menu.Item>
              <Button primary content="Личный кабинет" />
            </Menu.Item>
          </>
        )}
        {!userStore.isLoggedIn && (
          <Menu.Item>
            <Button
              onClick={() =>
                modalStore.openModal(<LoginForm />, "Войти на сайт")
              }
              primary
              content="Войти"
            />
          </Menu.Item>
        )}

        {selectedDiary && (
          // @ts-ignore
          <Header className="DiaryHeader">{selectedDiary}</Header>
        )}
      </Container>
    </Menu>
  );
});
