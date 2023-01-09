import { configureStore } from '@reduxjs/toolkit';
import { INITIAL_STATE, globalReducer } from './globalReducer';
import { createAPI } from '../services/api-service';

export const api = createAPI();

export const store = configureStore({
	preloadedState: INITIAL_STATE,
	reducer: globalReducer,
	middleware: (getDefaultMiddleware) =>
		getDefaultMiddleware({
			thunk: {
				extraArgument: api,
			},
		})
})
