import { UserInfo } from "@src/app/models/user";
import { useStore } from "@src/app/stores/store";
import { toJS } from "mobx";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Card, Loader, Segment } from "semantic-ui-react";

interface UserView {
  [key: string]: boolean;
}
export default function PatientList() {
  const { userStore, viewStore } = useStore();
  const navigate = useNavigate();

  const [patinetList, setPatientList] = useState<UserInfo[]>(null);
  const [patientNotView, setPatientNotView] = useState<UserView>({});

  useEffect(() => {
    const fetchApi = async () => {
      await userStore.getCurrUser();
      await userStore.getPatientList();
      setPatientList(userStore.patientList);
      setPatientViews();
    };
    fetchApi();
  }, [userStore, viewStore]);

  if (userStore.user === null || patinetList === null) {
    return <Loader active />;
  }

  function setPatientViews() {
    let notView: UserView = {};
    if (viewStore.usersViews === null) {
      return;
    }
    for (const uv of viewStore.usersViews) {
      for (const diaryView of uv.diariesViews) {
        if (diaryView.lastModifyDate > diaryView.lastViewDate) {
          console.log(`user if true`);
          notView[uv.userName] = true;
          break;
        }
      }
      if (notView[uv.userName] === null) {
        notView[uv.userName] = false;
      }
    }
    setPatientNotView(notView);
  }

  function handlePatientClick(key: number) {
    viewStore.setUserView(patinetList[key]);
    navigate(`/diaries`);
  }

  const patients = patinetList?.map((value, key) => {
    return (
      <Card
        key={key}
        className="patient-card"
        onClick={() => handlePatientClick(key)}
      >
        <Card.Content>
          {patientNotView[value.userName] !== null &&
            patientNotView[value.userName] != false && (
              <div className="redicon-user"></div>
            )}
          <Card.Header>{value.userName}</Card.Header>
          <Card.Meta>{`${value.firstName} ${value.secondName}`}</Card.Meta>
          <Card.Description>{value.description}</Card.Description>
          <Card.Content extra>
            <Button
              basic
              color="red"
              onClick={(event) => {
                event.stopPropagation();
                userStore.removePatient(value.userName);
              }}
            >
              Удалить
            </Button>
          </Card.Content>
        </Card.Content>
      </Card>
    );
  });

  return (
    //@ts-ignore
    <Segment>
      <h2>Список пациентов:</h2>
      <div className="patient-list-grid">{patients}</div>
    </Segment>
  );
}
