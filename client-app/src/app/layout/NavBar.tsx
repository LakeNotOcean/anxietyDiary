import React, { ReactElement, useEffect, useState } from "react";
import {
  Button,
  Container,
  Dropdown,
  DropdownMenu,
  Header,
  Icon,
  Menu,
  MenuItem,
} from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import { NavLink, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";
import { toJS } from "mobx";
import { UserRoleEnum } from "../enums/UserEnum";

interface Props {
  fullDiaryName?: string;
  diaryShortName?: string;
}

export default observer(function NavBar({
  fullDiaryName,
  diaryShortName,
}: Props): JSX.Element {
  const { userStore, viewStore } = useStore();
  const navigate = useNavigate();
  const [isNotView, setNotView] = useState(false);

  useEffect(() => {
    const fetchApi = async () => {
      if (userStore.isLoggedIn && userStore.user.role != UserRoleEnum.patient) {
        await viewStore.loadUsersViews().then(() => {
          viewStore.usersViews.some((uv) => {
            return uv.diariesViews.some((dv) => {
              if (dv.lastViewDate < dv.lastModifyDate) {
                setNotView(true);
                return true;
              }
            });
          });
        });
      }
    };
    fetchApi();
  }, [userStore.isLoggedIn, viewStore, userStore]);

  function handleExistFromUserView() {
    viewStore.setUserView(null);
    viewStore.setDates(null);
    navigate("/account/patients");
  }

  return (
    <Menu inverted fixed="top">
      <Container>
        <div className="menu-header">
          <Menu.Item header as={NavLink} to={"/"}>
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
        </div>
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
              <Dropdown
                as={Button}
                loading={
                  userStore.isLoggedIn &&
                  userStore.user.role != UserRoleEnum.patient &&
                  viewStore.usersViews === null
                    ? true
                    : false
                }
                primary
                text="Меню"
              >
                <DropdownMenu>
                  <Dropdown.Item
                    text="Личный кабинет"
                    as={NavLink}
                    to={"/account"}
                  ></Dropdown.Item>
                  {userStore.user?.role != UserRoleEnum.patient && (
                    <>
                      <Dropdown.Item
                        text="Пациенты"
                        as={NavLink}
                        to={"/patients"}
                      ></Dropdown.Item>
                      {isNotView && <div className="redicon-patients"></div>}
                    </>
                  )}
                </DropdownMenu>
              </Dropdown>
              {isNotView && <div className="redicon"></div>}
            </Menu.Item>
            {viewStore.isAnotherUser() && (
              <Menu.Item>
                <Button
                  primary
                  labelPosition="right"
                  onClick={handleExistFromUserView}
                >
                  <Icon name="x" />
                  {viewStore.currUserView.userName}
                </Button>
              </Menu.Item>
            )}
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
          <Header
            className="DiaryHeader"
            as={NavLink}
            to={`/diary/${diaryShortName}/${Date.now()}`}
          >
            {fullDiaryName}
          </Header>
        )}
      </Container>
    </Menu>
  );
});
