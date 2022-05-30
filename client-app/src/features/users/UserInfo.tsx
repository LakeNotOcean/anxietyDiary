import { UserRoleEnum } from "@src/app/enums/UserEnum";
import { ReqInfo, requestMessage } from "@src/app/models/request";
import { UserInfo } from "@src/app/models/user";
import { useStore } from "@src/app/stores/store";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import {
  Button,
  Card,
  Container,
  Dimmer,
  Grid,
  Item,
  List,
  Loader,
  Segment,
} from "semantic-ui-react";
import FindUserForm from "./FindUserForm";
import UserInfoForm from "./UserInfoForm";

export default observer(function UserInfo() {
  const { userStore, modalStore, requestStore } = useStore();
  const [doctors, setDoctors] = useState<UserInfo[]>(null);
  const [viewAsDoctor, setViewAsDoctor] = useState<ReqInfo[]>(null);
  const [inviteDoctor, setInviteDoctor] = useState<ReqInfo[]>(null);
  const [userReq, setUserReq] = useState<ReqInfo[]>(null);

  useEffect(() => {
    const fetchApi = async () => {
      await userStore.getCurrUser();
      if (!userStore.isLoggedIn) {
        return;
      }
      await userStore.loadDoctors();
      await requestStore.loadViewAsDoctorRequests();
      if (userStore.user?.role != UserRoleEnum.patient) {
        await requestStore.loadInviteDoctorRequests();
        setInviteDoctor(requestStore.inviteDoctorRequests);
      }

      await requestStore.loadUserRequests();
      setDoctors(userStore.userDoctors);
      setViewAsDoctor(requestStore.viewAsDoctorRequests);
      setUserReq(requestStore.userRequests);
    };
    fetchApi();
  }, [userStore, requestStore]);

  if (userStore.user === null) {
    console.log("user is still null");
    return <></>;
  }

  return (
    // @ts-ignore
    <Segment className="user-info">
      <h1>Личный кабинет</h1>
      <div className="user-name">
        <h1>{`Здравствуйте, ${userStore.user.userName}`}</h1>
      </div>
      <div className="user-info-fields">
        <Grid columns={4} divided>
          <Grid.Row>
            <Grid.Column>
              <h5>Имя:</h5>
              <h3>{userStore.user.firstName}</h3>
            </Grid.Column>
            <Grid.Column>
              <h5>Фамилия:</h5>
              <h3>{userStore.user.secondName}</h3>
            </Grid.Column>
            <Grid.Column>
              <h5>Роль:</h5>
              <h3>{userStore.user.role}</h3>
            </Grid.Column>
            <Grid.Column>
              <h5>Виден для поиска?</h5>
              <h3>{userStore.user.isSearching ? "да" : "нет"}</h3>
            </Grid.Column>
          </Grid.Row>
        </Grid>
        <div className="user-info-personal">
          <div className="user-info-info">
            <h5>О себе:</h5>
            <Container>
              <p>{userStore.user.description}</p>
            </Container>
          </div>
          <div className="user-info-doctors">
            <h4>Доктора:</h4>
            <Container>
              <DoctorsComponent
                handleDeleteDoctor={userStore.removeDoctor}
                doctors={doctors}
              />
            </Container>
          </div>
        </div>
      </div>
      <Button
        primary
        content="Изменить информацию"
        style={{ width: "300px" }}
        onClick={() => {
          modalStore.openModal(
            <UserInfoForm userInfo={userStore.user} />,
            "Изменить информацию"
          );
        }}
      />
      <div className="user-info-requests">
        <h4>Запросы на просмотр</h4>
        <Container>
          <RequestsComponent
            requests={viewAsDoctor}
            cancelRequest={requestStore.cancelRequest}
            acceptRequest={requestStore.acceptRequest}
          />
        </Container>
        {userStore.user.role !== UserRoleEnum.patient && (
          <Container>
            <h4>Приглашения на просмотр</h4>
            <RequestsComponent
              requests={inviteDoctor}
              cancelRequest={requestStore.cancelRequest}
              acceptRequest={requestStore.acceptRequest}
            />
          </Container>
        )}
        <Container>
          <h4>Мои запросы</h4>
          <RequestsComponent
            requests={userReq}
            cancelRequest={requestStore.cancelRequest}
          />
        </Container>
        <div className="user-info-add-users">
          <Button
            primary
            content="Добавить доктора"
            style={{ width: "300px" }}
            onClick={() => {
              modalStore.openModal(
                <FindUserForm
                  handleResultSelect={requestStore.sendRequestToDoctor}
                  handleSearchRequest={userStore.findDoctors}
                />,
                "Поиск доктора"
              );
            }}
          />
          {userStore.user.role != UserRoleEnum.patient && (
            <Button
              primary
              content="Добавить пациента"
              style={{ width: "300px" }}
              onClick={() => {
                modalStore.openModal(
                  <FindUserForm
                    handleResultSelect={requestStore.sendRequestToPatient}
                    handleSearchRequest={userStore.findPatients}
                  />,
                  "Поиск пациента"
                );
              }}
            />
          )}
        </div>
      </div>
    </Segment>
  );
});

interface ReqProps {
  request: ReqInfo;
  id: number;
  handleDeleteRequest: (req: ReqInfo) => void;
  handleAcceptRequest?: (req: ReqInfo) => void;
}

function RequestElement({
  request,
  id,
  handleDeleteRequest,
  handleAcceptRequest,
}: ReqProps) {
  return (
    <div className="request-element">
      <Card key={id}>
        <Card.Content>
          <Card.Header>{`Запрос на ${requestMessage.get(
            request.requestType
          )}`}</Card.Header>
          <Card.Description>{`Отправитель: ${request.sourceUser.userName} (${request.sourceUser.firstName} ${request.sourceUser.secondName})`}</Card.Description>
          {request.targetUser && (
            <Card.Description>{`Получатель: ${request.sourceUser.userName} (${request.sourceUser.firstName} ${request.sourceUser.secondName})`}</Card.Description>
          )}
          <Card.Content extra>
            <Button
              basic
              color="red"
              onClick={() => handleDeleteRequest(request)}
            >
              Удалить
            </Button>
            {handleAcceptRequest && (
              <Button
                basic
                color="green"
                onClick={() => handleAcceptRequest(request)}
              >
                Добавить
              </Button>
            )}
          </Card.Content>
        </Card.Content>
      </Card>
    </div>
  );
}

interface DProps {
  handleDeleteDoctor: (doctorName: string) => void;
  doctors: UserInfo[] | null;
}
function DoctorsComponent({ handleDeleteDoctor, doctors }: DProps) {
  const { userStore } = useStore();

  if (userStore.userDoctors === null) {
    return <Loader active />;
  }
  const res = userStore.userDoctors.map((value, key) => {
    return (
      <Card key={key}>
        <Card.Content>
          <Card.Header>{value.userName}</Card.Header>
          <Card.Meta>{`${value.secondName} ${value.firstName}`}</Card.Meta>
          <Card.Description>{value.description}</Card.Description>
          <Card.Content extra>
            <Button
              basic
              color="red"
              onClick={() => {
                handleDeleteDoctor(value.userName);
              }}
            >
              Удалить
            </Button>
          </Card.Content>
        </Card.Content>
      </Card>
    );
  });

  return <Card.Group>{res}</Card.Group>;
}

interface RProps {
  requests: ReqInfo[] | null;
  cancelRequest: (req: ReqInfo) => void;
  acceptRequest?: (req: ReqInfo) => void;
}
function RequestsComponent({ requests, cancelRequest, acceptRequest }: RProps) {
  if (requests === null) {
    return <Loader active />;
  }
  const res = requests.map((value, key) => {
    console.log("render is called");
    return (
      <List.Item>
        <RequestElement
          request={value}
          id={key}
          handleDeleteRequest={cancelRequest}
          handleAcceptRequest={acceptRequest}
        />
      </List.Item>
    );
  });

  return (
    <List horizontal relaxed="very">
      {res}
    </List>
  );
}
