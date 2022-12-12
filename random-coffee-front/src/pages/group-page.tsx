import { FC } from 'react';
import { GroupTable } from '../components/group-table/group-table';
import { useAppSelector } from '../hooks';

export const GroupPage: FC = () => {
	const { groups } = useAppSelector((state) => state);
	return (
		<GroupTable groups={groups}/>
	);
};
