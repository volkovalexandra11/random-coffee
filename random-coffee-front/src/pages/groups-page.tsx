import { FC, useEffect } from 'react';
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


	useEffect(() => {
		async function getAuthStatus() {
			const resp = await fetch('/api/account');
			const respStatusCode = resp.status;
			if (respStatusCode === 401) {
				navigate('/login');
			} else {
				store.dispatch(fetchUserAction());
			}
		}
		getAuthStatus();
	}, []);

	useEffect(() => {
		// @ts-ignore
		store.dispatch(fetchGroupsAction(user?.userId));
	}, [user])


	return (
		<Loader active={!isGroupsLoaded}>
			{isGroupsLoaded && (authStatus === AuthStatus.Logged) ? <GroupTable groups={groups}/> : <StubGroupTable/>}
		</Loader>
	);
};
