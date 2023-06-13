import { AxiosInstance } from 'axios';
import { createAsyncThunk } from '@reduxjs/toolkit';

import { AppDispatch, State } from '../types/store';
import { TGroup, TGroupDto, TGroupShort } from '../types/group';
import { setCurrentGroup, setGroups, setIsGroupsLoaded } from './action';
import { TUser } from '../types/user';

type Ids = {
    groupId: string | undefined;
    userId: string;
}

export const fetchGroupsAction = createAsyncThunk<void, string | undefined, {
    dispatch: AppDispatch,
    state: State,
    extra: AxiosInstance
}>(
    '/data/groups',
    async (userId, { dispatch, extra: api }) => {
        dispatch(setIsGroupsLoaded(false));
        try {
            const response = await api.get<TGroupShort[]>(`/api/groups?userId=${userId}`);
            // @ts-ignore
            const groups: TGroupShort[] = response.data.items.map((g: TGroupShort) => ({
                groupId: g.groupId,
                name: g.name,
                participantsCount: g.participantsCount,
                tag: g.tag === null ? [] : Array.isArray(g.tag) ? [...g.tag] : [g.tag],
                groupPictureUrl: g?.groupPictureUrl,
                nextRoundDate: g.nextRoundDate,
            } as TGroupShort));
            dispatch(setGroups({ groups }));
            dispatch(setIsGroupsLoaded(true));
        } catch (error) {
            console.error(error);
            dispatch(setIsGroupsLoaded(true));
        }
    }
);

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
);

export const kickUserFromGroupAction = createAsyncThunk<void, Ids, {
    dispatch: AppDispatch,
    state: State,
    extra: AxiosInstance
}>(
    'group/kick',
    async (ids: Ids, { extra: api }) => {
        const { userId, groupId } = ids;
        await api.post(`/api/groups/${groupId}/kick`, { userId });
    }
);

export const leaveGroupAction = createAsyncThunk<void, string, {
    dispatch: AppDispatch,
    state: State,
    extra: AxiosInstance
}>(
    'group/leave',
    async (groupId, { extra: api }) => {
        await api.post(`/api/groups/${groupId}/leave`);
    }
);

export const makeRoundAction = createAsyncThunk<void, string, {
  dispatch: AppDispatch,
  state: State,
  extra: AxiosInstance
}>(
  'group/makeround',
  async (groupId, { extra: api }) => {
    await api.post(`/api/groups/${groupId}/make-round`);
  }
);

export const fetchUserAction = createAsyncThunk<TUser | null, undefined, {
    dispatch: AppDispatch,
    state: State,
    extra: AxiosInstance
}>(
    'user/info',
    async (_arg, { extra: api }) => {
        const { data } = await api.get<TUser>(`/api/account`);

        return data;
    },
);

export const postGroupAction = createAsyncThunk<void, TGroupDto, {
    dispatch: AppDispatch,
    state: State,
    extra: AxiosInstance
}>(
    '/data/groups',
    async (groupDto: TGroupDto, { dispatch, extra: api }) => {
        console.log(groupDto);
        const { data } = await api.post<TGroup>(`/api/groups`, groupDto);
        dispatch(setCurrentGroup({ currentGroup: data }));
    }
)

export const joinAGroup = createAsyncThunk<void, string | undefined, {
    dispatch: AppDispatch,
    state: State,
    extra: AxiosInstance
}>(
    '/data/groups',
    async (groupId: string | undefined, { dispatch, extra: api }) => {
        await api.post<TGroup>(`/api/groups/${groupId}/join`);
    }
)
