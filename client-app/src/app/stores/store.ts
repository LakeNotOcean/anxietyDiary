import { createContext, useContext } from "react";
import RecordsStore from "./recordsStore";

interface Store {
  recordsStore: RecordsStore;
}

export const store: Store = {
  recordsStore: new RecordsStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
