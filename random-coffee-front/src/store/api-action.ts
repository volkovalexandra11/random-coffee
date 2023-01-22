import { AxiosInstance } from 'axios';
import { createAsyncThunk } from '@reduxjs/toolkit';

import { AppDispatch, State } from '../types/store';
import { TGroup, TGroupShort } from '../types/group';
import { setCurrentGroup, setGroups, setIsGroupsLoaded } from './action';
import { TUser } from '../types/user';

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


export const fetchUserAction = createAsyncThunk<TUser | null, undefined, {
    dispatch: AppDispatch,
    state: State,
    extra: AxiosInstance
}>(
    'user/info',
    async (_arg, { extra: api }) => {
        console.log('fetch');
        const { data } = await api.get<TUser>(`/api/account`);

        return data;
    },
);

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
