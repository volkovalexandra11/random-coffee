import React from 'react';
import './App.scss';
import { GroupsPage } from "./pages/groups-page";
import { Route, Routes } from 'react-router-dom';

import { NavBar } from "./components/nav-bar/nav-bar";
import { CreateGroup } from "./pages/create-group";
import { LoginWithGoogle } from './pages/login-with-google';
import { Group } from "./pages/group-page";
import { useAppSelector } from './hooks';
import { Mock } from './pages/mock';
import {UserPage} from "./pages/user-page";

function App() {
	const { user } = useAppSelector((state) => state);
	return (
		<main>
			{user && <NavBar/>}
			<Routes>
				<Route path={'/'} element={<GroupsPage/>}/>
				<Route path={'/login'} element={<LoginWithGoogle/>}/>
				<Route path={'/create'} element={<CreateGroup/>}/>
				<Route path={'/group/:groupId'} element={<Group/>}/>
				<Route path={'/mock'} element={<Mock/>}/>
				<Route path={'/user'} element={<UserPage/>}/>
				{/*<Route path={'*'} element={<GroupsPage/>}/>*/}
			</Routes>
		</main>
	);
}

export default App;
