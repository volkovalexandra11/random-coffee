import React from 'react';
import './App.scss';
import {GroupPage} from "./pages/group-page";
import { Route, Routes} from 'react-router-dom';

import { TGroup } from './types/group';
import {mockGroupList} from "./mock/mock-group-list";
import {NavBar} from "./components/nav-bar/nav-bar";
import {CreateGroup} from "./pages/create-group";

type Props = {
  groupList: TGroup[];
}

function App() {
  let groupList = mockGroupList;
  return (
      <main>
      <NavBar id={1} avatarPath={''} firstName={"Самсонов"} lastName={"Иван"}/>
          <Routes>
              <Route path={'/'} element={<GroupPage groupList={groupList}/>}/>
              <Route path={'*'} element={<GroupPage groupList={groupList}/>}/>
              <Route path={'/create'} element={<CreateGroup/>}/>
          </Routes>
      </main>
  );
}

export default App;
