import React from 'react';
import './App.scss';
import { GroupsPage } from "./pages/groups-page";
import { Route, Routes } from 'react-router-dom';

import { TGroup } from './types/group';
import { NavBar } from "./components/nav-bar/nav-bar";
import { CreateGroup } from "./pages/create-group";
import { LoginWithGoogle } from './pages/login-with-google';
import { Group } from "./pages/group-page";

function App() {
    return (
		<main>
			<NavBar/>
			<Routes>
				<Route path={'/'} element={<GroupsPage/>}/>
				<Route path={'/login'} element={<LoginWithGoogle/>}/>
				<Route path={'/create'} element={<CreateGroup/>}/>
				<Route path={'/group/:groupId'} element={<Group/>}/>
				<Route path={'*'} element={<GroupsPage/>}/>
            </Routes>
        </main>
    );
}

export default App;
