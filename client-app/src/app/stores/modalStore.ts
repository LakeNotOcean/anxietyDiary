import { makeAutoObservable, observable } from "mobx";

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
    makeAutoObservable(this, {
      modal: observable,
    });
  }

  openModal = (content: JSX.Element, header: string) => {
    this.modal.open = true;
    this.modal.body = content;
    this.modal.header = header;
  };
  closeModal = () => {
    this.modal.open = false;
    this.modal.body = null;
    this.modal.header = null;
  };
}
