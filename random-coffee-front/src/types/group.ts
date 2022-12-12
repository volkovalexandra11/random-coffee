import { TUser } from './user';

export type TGroup = {
	id: number;
	name: string;
	description: string;
	users: TUser[];
	picturePath?: string;
}
