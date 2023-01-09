import { FC } from 'react';
import { Loader } from '@skbkontur/react-ui';
import { useAppSelector } from "../hooks";
import { StubGroupTable } from '../components/stub/stub-group-table/stub-group-table';
import { GroupTable } from '../components/group-table/group-table';

export const GroupsPage: FC = () => {
	const { groups } = useAppSelector((state) => state);
	const { isGroupsLoaded } = useAppSelector((state) => state);
	console.log(groups);
	return (
		<Loader active={!isGroupsLoaded}>
			{isGroupsLoaded ? <GroupTable groups={groups}/> : <StubGroupTable/>}
		</Loader>
	);
};
