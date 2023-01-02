import { AxiosInstance } from 'axios';
import { createAsyncThunk } from '@reduxjs/toolkit';

import { AppDispatch, State } from '../types/store';
import {TGroup, TGroupShort} from '../types/group';
import {setCurrentGroup, setGroups, setIsGroupsLoaded, setUser} from './action';
import {TUser} from "../types/user";

export const fetchGroupsAction = createAsyncThunk<void, undefined, {
	dispatch: AppDispatch,
	state: State,
	extra: AxiosInstance
}>(
	'/data/groups',
	async (_arg, {dispatch, extra: api}) => {
		dispatch(setIsGroupsLoaded(false));
		const { data } = await api.get<TGroupShort[]>('/api/groups?userId=43ef1000-0000-0000-0000-000000000000');
		dispatch(setGroups({groups: data}));
		dispatch(setIsGroupsLoaded(true));
	}
)

export const fetchGroupByIdAction = createAsyncThunk<void, string | undefined, {
	dispatch: AppDispatch,
	state: State,
	extra: AxiosInstance
}>(
	'/data/groups',
	async (groupId : string | undefined, {dispatch, extra: api}) => {
		dispatch(setIsGroupsLoaded(false));
		dispatch(setCurrentGroup({ currentGroup: null}));
		const { data } = await api.get<TGroup>(`/api/groups/${groupId}`);
		dispatch(setCurrentGroup({ currentGroup: data}));
		dispatch(setIsGroupsLoaded(true));
	}
)

export const fecthUserAction = createAsyncThunk<void, undefined, {
	dispatch: AppDispatch,
	state: State,
	extra: AxiosInstance
}>(
	'/data/groups',
	async (_args, {dispatch, extra: api}) => {
		dispatch(setIsGroupsLoaded(false));
		const { data } = await api.get<TUser>(`/api/users/9f048120-0000-0000-0000-000000000000`);
		dispatch(setUser({ user: data}));
		dispatch(setIsGroupsLoaded(true));
	}
)