import React from 'react';
import './App.scss';
import { GroupPage } from "./pages/group-page";
import { Route, Routes } from 'react-router-dom';
import { NavBar } from "./components/nav-bar/nav-bar";
import { CreateGroup } from "./pages/create-group";
import LoginWithGoogle from './pages/login-with-google';

function App() {
	return (
		<main>
			<NavBar/>
			<Routes>
				<Route path={'/'} element={<GroupPage/>}/>
				<Route path={'/login'} element={<LoginWithGoogle/>}/>
				<Route path={'/create'} element={<CreateGroup/>}/>
				<Route path={'*'} element={<GroupPage/>}/>
			</Routes>
		</main>
	);
}

export default App;
