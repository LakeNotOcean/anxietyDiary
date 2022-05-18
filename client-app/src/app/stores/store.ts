import { createContext, useContext, useEffect } from "react";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import RecordsStore from "./recordsStore";
import UserStore from "./userStore";

interface Store {
  recordsStore: RecordsStore;
  userStore: UserStore;
  commonStore: CommonStore;
  modalStore: ModalStore;
}

export const store: Store = {
  recordsStore: new RecordsStore(),
  userStore: new UserStore(),
  commonStore: new CommonStore(),
  modalStore: new ModalStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
