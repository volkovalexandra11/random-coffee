import { FC, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Loader } from '@skbkontur/react-ui';
import { useAppSelector } from "../hooks";
import { StubGroupTable } from '../components/stub/stub-group-table/stub-group-table';
import { GroupTable } from '../components/group-table/group-table';
import { AuthStatus } from '../types/authStatus';
import { store } from '../store';
import { fetchGroupsAction, fetchUserAction } from '../store/api-action';
import { changeAuthStatus } from "../store/action";

export const GroupsPage: FC = () => {
	const navigate = useNavigate();
	const { groups } = useAppSelector((state) => state);
	const { isGroupsLoaded } = useAppSelector((state) => state);
	const { user } = useAppSelector((state) => state);
	const { authStatus } = useAppSelector((state) => state);

	const [isFetched, setIsFetched] = useState(false);
	const [isAuthorized, setIsAuthorized] = useState(authStatus === AuthStatus.Logged);
	// useEffect(()=>{
	// 	checkAuth();
	// 	if (!isAuthorized) {
	// 		console.log('login');
	// 		(() => navigate('/login'))();
	// 	} else {
	// 		console.log('fetch');
	// 		fetchData().finally();
	// 		// store.dispatch(fetchUserAction());
	// 		// console.log(user);
	// 		// // @ts-ignore
	// 		// store.dispatch(fetchGroupsAction(user["userId"]))
	// 	}
	// }, [isAuthorized]);

	const checkAuth = () => {
		async function getAuthStatus() {
			const resp = await fetch('/api/account');
			const respStatusCode = resp.status;
			setIsAuthorized(respStatusCode === 200);
			console.log(isAuthorized, respStatusCode);
		}

		console.log('check status');
		getAuthStatus();
	}

	const fetchData = async () => {
		console.log('fetch');
		await store.dispatch(fetchUserAction());
		console.log(user)
		// @ts-ignore
		store.dispatch(fetchGroupsAction(user["userId"]))
	}

	checkAuth();

	if (!isAuthorized) {
		console.log('login');
		navigate('/login');

	} else if (!isFetched) {
		console.log('fetch');
		fetchData().finally(() => setIsFetched(true));
	}


	console.log(groups);
	return (
		<Loader active={!isGroupsLoaded}>
			{isGroupsLoaded && isAuthorized ? <GroupTable groups={groups}/> : <StubGroupTable/>}
		</Loader>
	);
};
