import { AuthStatus } from '../types/authStatus';
import { AppState } from '../types/store';
import { createReducer } from '@reduxjs/toolkit';
import { changeAuthStatus, setGroups, setUser } from './action';
import { TGroup } from '../types/group';

export const INITIAL_STATE: AppState = {
	user: null,
	authStatus: AuthStatus.Unknown,
	groups: new Array<TGroup>(),
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
});

