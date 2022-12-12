import { createAction } from '@reduxjs/toolkit';
import { TUser } from '../types/user';
import { AuthStatus } from '../types/authStatus';
import { TGroup } from '../types/group';

export const changeAuthStatus = createAction<{authStatus: AuthStatus}>('changeAuthStatus');
export const setUser = createAction<{user: TUser | null}>('setUser');
export const setGroups = createAction<{groups: Array<TGroup>}>('setGroups');
