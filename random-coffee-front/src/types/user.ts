export type TUser = {
	userId: string;
	firstName: string;
	lastName: string;
	tag: string[];
	email?: string;
	telegram?: string;
	profilePictureUrl?: string;
}

export type TUpdateUserDto = {
	email: string,
	firstName: string,
	lastName: string,
	profilePictureUrl: string,
}
