import { FC, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Loader } from '@skbkontur/react-ui';
import { useAppSelector } from "../hooks";
import { StubGroupTable } from '../components/stub/stub-group-table/stub-group-table';
import { GroupTable } from '../components/group-table/group-table';
import { AuthStatus } from '../types/authStatus';
import { store } from '../store';
import { fetchGroupsAction, fetchUserAction } from '../store/api-action';

export const GroupsPage: FC = () => {
	const navigate = useNavigate();

	const { groups } = useAppSelector((state) => state);
	const { isGroupsLoaded } = useAppSelector((state) => state);
	const { user } = useAppSelector((state) => state);
	const { authStatus } = useAppSelector((state) => state);

	const [isAuthorized, setIsAuthorized] = useState(authStatus === AuthStatus.Logged);

	const checkAuth = () => {
		async function getAuthStatus() {
			const resp = await fetch('/api/account');
			const respStatusCode = resp.status;
			setIsAuthorized(respStatusCode === 200);
		}

		getAuthStatus();
	}


	checkAuth();
	console.log(isAuthorized);
	console.log('useEffect');

	if (!isAuthorized) {
		console.log('login');
		navigate('/login');
	} else {
		console.log('fetch');
		store.dispatch(fetchUserAction());
		store.dispatch(fetchGroupsAction(user?.id))
	}


	console.log(groups);
	return (
		<Loader active={!isGroupsLoaded}>
			{isGroupsLoaded && isAuthorized ? <GroupTable groups={groups}/> : <StubGroupTable/>}
		</Loader>
	);
};
