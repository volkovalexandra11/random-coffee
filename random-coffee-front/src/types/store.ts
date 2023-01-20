import { TUser } from './user';
import { TGroupShort, TGroup } from './group';
import { AuthStatus } from './authStatus';
import store from '../store';

export type AppState = {
	user: TUser | null,
	authStatus: AuthStatus,
	groups: Array<TGroupShort>,
	isGroupsLoaded: boolean,
	currentGroup: TGroup | null
}

export type AppDispatch = typeof store.dispatch;
export type State = ReturnType<typeof store.getState>
