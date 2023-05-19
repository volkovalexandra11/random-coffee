import { AuthStatus } from '../types/authStatus';
import { AppState } from '../types/store';
import { createReducer } from '@reduxjs/toolkit';
import {
	changeAuthStatus,
	deleteGroupFromUser,
	deleteUserFromGroup,
	setCurrentGroup,
	setGroups,
	setIsGroupsLoaded,
	setUser
} from './action';
import { TGroupShort } from '../types/group';
import { fetchUserAction } from './api-action';

export const INITIAL_STATE: AppState = {
	user: null,
	authStatus: AuthStatus.NotLogged,
	groups: new Array<TGroupShort>(),
	isGroupsLoaded: false,
	currentGroup: null
};

export const globalReducer = createReducer(INITIAL_STATE, (builder) => {
	builder
		.addCase(changeAuthStatus, (state, action) => {
			state.authStatus = action.payload.authStatus;
		})
		.addCase(setUser, (state, action) => {
			state.user = action.payload.user
		})
		.addCase(setGroups, (state, action) => {
			state.groups = action.payload.groups
		})
		.addCase(setIsGroupsLoaded, (state, action) => {
			state.isGroupsLoaded = action.payload;
		})
		.addCase(setCurrentGroup, (state, action) => {
			state.currentGroup = action.payload.currentGroup
		})
		.addCase(fetchUserAction.fulfilled, (state, action) => {
			state.user = action.payload;
		})
		.addCase(fetchUserAction.rejected, (state) => {
			state.authStatus = AuthStatus.NotLogged;
		})
		.addCase(deleteUserFromGroup, (state, action) => {
			// @ts-ignore
			state.currentGroup.participants = state.currentGroup.participants.filter(user => user.userId !== action.payload.userToDelete.userId);
		})
		.addCase(deleteGroupFromUser, (state) => {
			// @ts-ignore
			state.groups = state.groups.filter(group => group.groupId !== state.currentGroup.groupId);
		})
});

