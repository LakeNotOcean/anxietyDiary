import { UserRoleEnum } from "../enums/UserEnum";

export interface UserInfo {
  userName: string;
  firstName: string;
  secondName: string;
  role: UserRoleEnum;
  description: string;
  isSearching: boolean;
}

export interface User extends UserInfo {
  token: string;
}

export interface UserRegisterFormValues {
  email: string;
  password: string;
  userName: string;
  firstName?: string;
  secondName?: string;
}

export interface UserRegisterRepFormValues extends UserRegisterFormValues {
  repeatPassword: string;
  errors?: string[];
}

export interface UserLoginFormValues {
  email: string;
  password: string;
}
