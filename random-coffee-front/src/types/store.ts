import { TUser } from './user';
import { TGroupShort } from './group';
import { AuthStatus } from './authStatus';
import { store } from '../store';

export type AppState = {
	user: TUser | null,
	authStatus: AuthStatus,
	groups: Array<TGroupShort>
}

export type AppDispatch = typeof store.dispatch;
export type State = ReturnType<typeof store.getState>
