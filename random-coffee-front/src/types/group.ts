import { TUser } from './user';

export type TGroupShort = {
	groupId: string;
	name: string;
	participantsCount: number;
	picturePath?: string;
	nextRoundDate: string;
}

export type TGroup = {
	groupId: string;
	name: string;
	admin: TUser;
	participantsCount: number;
	picturePath?: string;
	participants: TUser[];
	description: string;
	isPrivate: boolean;
}
