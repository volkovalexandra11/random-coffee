import { FC, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Loader } from '@skbkontur/react-ui';
import { useAppSelector } from "../hooks";
import { StubGroupTable } from '../components/stub/stub-group-table/stub-group-table';
import { GroupTable } from '../components/group-table/group-table';
import { AuthStatus } from '../types/authStatus';
import store from '../store';
import { fetchGroupsAction, fetchUserAction } from '../store/api-action';
import {EmptyGroupList} from "../components/empty-group-list/empty-group-list";

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
	}, [navigate]);

	useEffect(() => {
		if (authStatus === AuthStatus.Logged) {
			store.dispatch(fetchGroupsAction(user?.userId));
		}
	}, [authStatus, user])


	return (
		<Loader active={!isGroupsLoaded}>
			{isGroupsLoaded && (authStatus === AuthStatus.Logged) ? groups.length !== 0 ?
				<GroupTable groups={groups}/> : <EmptyGroupList/> : <StubGroupTable/>}
		</Loader>
	);
};
