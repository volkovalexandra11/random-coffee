import { TGroup } from '../types/group';

export const mockGroupList : TGroup[] = [
	{
		id: 1,
		name: 'Test',
		description: 'Test group',
		users: [
			{
				id: 1,
				firstName: 'Vasya',
				lastName: 'Pupkin',
				avatarPath: '/img/avatar/jpg'
			},
			{
				id: 2,
				firstName: 'Putya',
				lastName: 'Ivanov',
				avatarPath: '/img/avatar.jpg'
			}
		],
		picturePath: '/img/picture.jpg'
	}
];
