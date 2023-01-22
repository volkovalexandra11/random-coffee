import { AuthStatus } from '../types/authStatus';
import { AppState } from '../types/store';
import { createReducer } from '@reduxjs/toolkit';
import { changeAuthStatus, setCurrentGroup, setGroups, setIsGroupsLoaded, setUser } from './action';
import { TGroupShort } from '../types/group';
import { fetchUserAction } from './api-action';

export const INITIAL_STATE: AppState = {
	user: null,
	authStatus: AuthStatus.Unknown,
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
});

