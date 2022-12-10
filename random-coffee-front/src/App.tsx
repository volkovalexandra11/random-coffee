import React from 'react';
import './App.scss';
import { GroupPage } from "./pages/group-page";
import { Route, Routes } from 'react-router-dom';

import { TGroup } from './types/group';
import { mockGroupList } from "./mock/mock-group-list";
import { NavBar } from "./components/nav-bar/nav-bar";
import { CreateGroup } from "./pages/create-group";
import LoginWithGoogle from './pages/login-with-google';

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
				<Route path={'/login'} element={<LoginWithGoogle/>}></Route>
				<Route path={'/create'} element={<CreateGroup/>}/>
				<Route path={'*'} element={<GroupPage groupList={groupList}/>}/>
			</Routes>
		</main>
	);
}

export default App;
