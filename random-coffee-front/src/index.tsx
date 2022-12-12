import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom'
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';

import {store} from "./store";
import {Provider} from "react-redux";
import {setGroups, setUser} from "./store/action";
import {mockGroupList} from "./mock/mock-group-list";
import {TUser} from "./types/user";

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

let AppStore = store;
const groupList = mockGroupList;
const user: TUser| null = {
    id: 1,
    firstName: "Самсонов",
    lastName: "Иван",
    avatarPath: ""
}
AppStore.dispatch(setGroups({groups: groupList}))
AppStore.dispatch(setUser({user: user}))


root.render(
    <BrowserRouter>
        <Provider store={AppStore}>
        <App />
        </Provider>
    </BrowserRouter>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
