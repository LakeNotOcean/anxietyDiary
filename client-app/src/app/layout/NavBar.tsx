import React, { ReactElement, useState } from "react";
import { Button, Container, Header, Menu } from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";
import { NavLink } from "react-router-dom";

interface Props {
  selectedDiary?: string;
}

export default function NavBar({ selectedDiary }: Props): JSX.Element {
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

        {selectedDiary && (
          // @ts-ignore
          <Header className="DiaryHeader">{selectedDiary}</Header>
        )}
      </Container>
    </Menu>
  );
}
