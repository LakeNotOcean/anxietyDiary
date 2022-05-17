import React, { ReactElement, useState } from "react";
import { Button, Container, Header, Menu } from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import { NavLink } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";

interface Props {
  selectedDiary?: string;
}

export default observer(function NavBar({ selectedDiary }: Props): JSX.Element {
  const { userStore } = useStore();
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
          {userStore.isLoggedIn && (
            <Menu.Item>
              <Button primary content="Выйти" />
              <Button primary content="Личный кабинет" />
            </Menu.Item>
          )}
          {!userStore.isLoggedIn && (
            <Menu.Item>
              <Button primary content="Войти" />
              <Button primary content="Личный кабинет" />
            </Menu.Item>
          )}
        </Menu.Item>

        {selectedDiary && (
          // @ts-ignore
          <Header className="DiaryHeader">{selectedDiary}</Header>
        )}
      </Container>
    </Menu>
  );
});
