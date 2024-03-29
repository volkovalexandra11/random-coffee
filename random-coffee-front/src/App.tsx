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
import { fetchUserAction } from './store/api-action';
import './App.scss';
import { changeAuthStatus } from './store/action';
import { ErrorPage } from './components/error/error-page';
import {ChangeUserNamePage} from "./pages/change-user-name-page";
import {TagsPage} from "./pages/tags-page";

function App() {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const { authStatus } = useAppSelector((state) => state);

  useEffect(() => {
    if (authStatus === AuthStatus.Unknown) {
      dispatch(fetchUserAction()).then(() =>
        dispatch(changeAuthStatus({ authStatus: AuthStatus.Logged }))
      );
    }
    if (authStatus === AuthStatus.NotLogged) {
      navigate('/login');
    }
  }, [dispatch, navigate]);

  return (
    <main>
      {authStatus === AuthStatus.Logged && <NavBar />}
      <Routes>
        <Route path={'/'} element={<GroupsPage />} />
        <Route path={'/login'} element={<LoginWithGoogle />} />
        <Route path={'/create'} element={<CreateGroup />} />
        <Route path={'/group/:groupId'} element={<Group />} />
        <Route path={'/user'} element={<UserPage />} />
        <Route path={'/error/:errorCode'} element={<ErrorPage />} />
        <Route path={'/user/change'} element={<ChangeUserNamePage/>}/>
        <Route path={'/user/tags'} element={<TagsPage/>}/>
        {/*<Route path={'*'} element={<GroupsPage/>}/>*/}
      </Routes>
    </main>
  );
}

export default App;
