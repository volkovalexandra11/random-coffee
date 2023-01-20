import { configureStore } from '@reduxjs/toolkit';
import { INITIAL_STATE, globalReducer } from './globalReducer';
import { createAPI } from '../services/api-service';
import {
	persistStore,
	persistReducer,
	FLUSH,
	REHYDRATE,
	PAUSE,
	PERSIST,
	PURGE,
	REGISTER
} from 'redux-persist';
import storage from 'redux-persist/lib/storage';

export const api = createAPI();

const persistConfig = {
	key: 'root',
	storage,
}
const persistedReducer = persistReducer(persistConfig, globalReducer)
const store = configureStore({
	preloadedState: INITIAL_STATE,
	reducer: persistedReducer,
	middleware: (getDefaultMiddleware) =>
		getDefaultMiddleware({
			thunk: {
				extraArgument: api,
			},
			serializableCheck: {
				ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
			},
		})
});
export const persistor = persistStore(store);
export default store;
