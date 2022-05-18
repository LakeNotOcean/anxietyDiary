import ReactDOM from "react-dom";
import React from "react";
import "@src/scss/index.scss";
import App from "./app/App";
import "react-toastify/dist/ReactToastify.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { store, StoreContext } from "./app/stores/store";
import { createBrowserHistory } from "history";

export const history = createBrowserHistory();

ReactDOM.render(
  <StoreContext.Provider value={store}>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StoreContext.Provider>,
  document.getElementById("root")
);
