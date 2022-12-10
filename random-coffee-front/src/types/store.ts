import { TUser } from './user';
import { TGroup } from './group';
import { AuthStatus } from './authStatus';
import { store } from '../store';

export type AppState = {
	user: TUser | null,
	authStatus: AuthStatus,
	groups: Array<TGroup>
}

export type AppDispatch = typeof store.dispatch;
export type State = ReturnType<typeof store.getState>
