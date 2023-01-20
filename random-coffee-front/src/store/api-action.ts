import { AxiosInstance } from 'axios';
import { createAsyncThunk } from '@reduxjs/toolkit';

import { AppDispatch, State } from '../types/store';
import { TGroup, TGroupShort } from '../types/group';
import { changeAuthStatus, setCurrentGroup, setGroups, setIsGroupsLoaded, setUser } from './action';
import { TUser } from "../types/user";
import { AuthStatus } from '../types/authStatus';

export const fetchGroupsAction = createAsyncThunk<void, string | undefined, {
	dispatch: AppDispatch,
	state: State,
	extra: AxiosInstance
}>(
	'/data/groups',
	async (userId, { dispatch, extra: api }) => {
		dispatch(setIsGroupsLoaded(false));
		const { data } = await api.get<TGroupShort[]>(`/api/groups?userId=${userId}`);
		dispatch(setGroups({ groups: data }));
		dispatch(setIsGroupsLoaded(true));
	}
)

export const fetchGroupByIdAction = createAsyncThunk<void, string | undefined, {
	dispatch: AppDispatch,
	state: State,
	extra: AxiosInstance
}>(
	'/data/groups',
	async (groupId: string | undefined, { dispatch, extra: api }) => {
		dispatch(setIsGroupsLoaded(false));
		dispatch(setCurrentGroup({ currentGroup: null }));
		const { data } = await api.get<TGroup>(`/api/groups/${groupId}`);
		dispatch(setCurrentGroup({ currentGroup: data }));
		dispatch(setIsGroupsLoaded(true));
	}
)

export const fetchUserAction = createAsyncThunk<void, undefined, {
	dispatch: AppDispatch,
	state: State,
	extra: AxiosInstance
}>(
	'/user/info',
	async (_args, { dispatch, extra: api }) => {
		dispatch(changeAuthStatus({authStatus: AuthStatus.NotLogged}));
		const { data } = await api.get<TUser>(`/api/account`);
		dispatch(setUser({ user: data }));
		dispatch(changeAuthStatus({authStatus: AuthStatus.Logged}));
	}
)

export const PostGroupsAction = createAsyncThunk<void, TGroup, {
	dispatch: AppDispatch,
	state: State,
	extra: AxiosInstance
}>(
	'/data/groups',
	async (data: TGroup, { dispatch, extra: api }) => {
		await api.post<TGroup>(`/api/groups`, data);

	}
)
