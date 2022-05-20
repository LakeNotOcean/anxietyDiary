import { RequestTypeEnum } from "../enums/RequestEnum";
import { UserInfo } from "./user";

export interface RequestSend {
  requestType: RequestTypeEnum;
  userName?: string;
}

export interface ReqInfo {
  sourceUser: UserInfo;
  targetUser?: UserInfo;
  requestId: number;
  requestType: RequestTypeEnum;
}

export const requestMessage: Map<RequestTypeEnum, string> = new Map([
  [RequestTypeEnum.BecomeDoctor, "стать доктором"],
  [RequestTypeEnum.InviteDoctor, "пригласить доктора"],
  [RequestTypeEnum.ViewAsDoctor, "просмотр доктором"],
]);
