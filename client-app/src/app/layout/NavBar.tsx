import React, { ReactElement, useEffect, useState } from "react";
import {
  Button,
  Container,
  Dropdown,
  DropdownMenu,
  Header,
  Menu,
  MenuItem,
} from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import { NavLink } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";
import { toJS } from "mobx";

interface Props {
  fullDiaryName?: string;
  diaryShortName?: string;
}

export default observer(function NavBar({
  fullDiaryName,
  diaryShortName,
}: Props): JSX.Element {
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
              <Dropdown text="Меню">
                <DropdownMenu>
                  <Dropdown.Item
                    text="Личный кабинет"
                    as={NavLink}
                    to={"/account"}
                  ></Dropdown.Item>
                </DropdownMenu>
              </Dropdown>
            </Menu.Item>
          </>
        )}
        {!userStore.isLoggedIn && (
          <Menu.Item>
            <Button
              onClick={() => userStore.setIsLoginForm(true)}
              primary
              content="Войти"
            />
          </Menu.Item>
        )}
        {diaryShortName && (
          <MenuItem>
            <Button
              positive
              content="Подробнее"
              as={NavLink}
              to={`/diaryinfo/${diaryShortName}`}
            />
          </MenuItem>
        )}

        {fullDiaryName && (
          // @ts-ignore
          <Header className="DiaryHeader">{fullDiaryName}</Header>
        )}
      </Container>
    </Menu>
  );
});
