export interface UserInfo {
  userName: string;
  firstName: string;
  secondName: string;
  description: string;
}

export interface User extends UserInfo {
  isSerching: string;
  token: string;
}

export interface UserRegisterFormValues {
  email: string;
  password: string;
  userName: string;
  firstName?: string;
  secodName?: string;
}

export interface UserLoginFormValues {
  email: string;
  password: string;
}
