import {
  makeAutoObservable,
  makeObservable,
  observable,
  runInAction,
} from "mobx";

interface Modal {
  open: boolean;
  header: string | null;
  body: JSX.Element | null;
}
export default class ModalStore {
  modal: Modal = {
    open: false,
    body: null,
    header: null,
  };

  constructor() {
    makeObservable(this, {
      modal: observable,
    });
  }

  openModal = (content: JSX.Element, header: string) => {
    runInAction(() => {
      this.modal.open = true;
      this.modal.body = content;
      this.modal.header = header;
    });
  };
  closeModal = () => {
    runInAction(() => {
      this.modal.open = false;
      this.modal.body = null;
      this.modal.header = null;
    });
  };
}
