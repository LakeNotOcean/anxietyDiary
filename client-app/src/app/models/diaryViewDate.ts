export interface IUserView {
  userName: string;
  diariesViews: IDiaryView[];
}

export interface IDiaryView {
  diaryName: string;
  lastViewDate: Date;
  lastModifyDate: Date;
}
