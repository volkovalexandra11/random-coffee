import { Route, Routes, useNavigate } from 'react-router-dom';

import { GroupsPage } from './pages/groups-page';
import { NavBar } from './components/nav-bar/nav-bar';
import { CreateGroup } from './pages/create-group';
import { LoginWithGoogle } from './pages/login-with-google';
import { Group } from './pages/group-page';
import { useAppDispatch, useAppSelector } from './hooks';
import { UserPage } from './pages/user-page';
import { useEffect } from 'react';
import { AuthStatus } from './types/authStatus';
import { fetchGroupsAction, fetchUserAction } from './store/api-action';
import './App.scss';

function App() {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    const { user } = useAppSelector((state) => state);
    const { authStatus } = useAppSelector((state) => state);

    useEffect(() => {
        if (authStatus === AuthStatus.Unknown) {
            dispatch(fetchUserAction());
        }
        if (authStatus === AuthStatus.NotLogged) {
            navigate('/login');
        }
    }, [dispatch, navigate]);

    useEffect(() => {
        if (authStatus === AuthStatus.Logged) {
            dispatch(fetchGroupsAction(user?.userId));
        }
    }, [authStatus, dispatch, user])

    return (
        <main>
            {authStatus === AuthStatus.Logged && <NavBar/>}
            <Routes>
                <Route path={'/'} element={<GroupsPage/>}/>
                <Route path={'/login'} element={<LoginWithGoogle/>}/>
                <Route path={'/create'} element={<CreateGroup/>}/>
                <Route path={'/group/:groupId'} element={<Group/>}/>
                <Route path={'/user'} element={<UserPage/>}/>
                {/*<Route path={'*'} element={<GroupsPage/>}/>*/}
            </Routes>
        </main>
    );
}

export default App;
