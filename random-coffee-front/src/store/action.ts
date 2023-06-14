import { createAction } from '@reduxjs/toolkit';
import { TUser } from '../types/user';
import { AuthStatus } from '../types/authStatus';
import { TGroup, TGroupShort } from '../types/group';

export const changeAuthStatus = createAction<{authStatus: AuthStatus}>('changeAuthStatus');
export const setTags = createAction<{tags: Array<string>}>('setTags');
export const setUser = createAction<{user: TUser | null }>('setUser');
export const setGroups = createAction<{groups: Array<TGroupShort>}>('setGroups');
export const setIsGroupsLoaded = createAction<boolean>('setIsGroupsLoaded');
export const setCurrentGroup = createAction<{currentGroup: TGroup | null}>('setCurrentGroup');
export const deleteUserFromGroup = createAction<{userToDelete: TUser}>('deleteUserFromGroup');
export const deleteGroupFromUser = createAction<{userId: string}>('deleteGroupFromUser');
