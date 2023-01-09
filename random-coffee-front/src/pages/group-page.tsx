import { FC } from 'react';
import { GroupInfo } from '../components/group-info/group-info';
import { useAppSelector } from "../hooks";
import { Loader } from "@skbkontur/react-ui";
import { StubGroupInfo } from '../components/stub/stub-group-info/stub-group-info';


export const Group: FC = () => {
	const { currentGroup } = useAppSelector((state) => state);
	const { isGroupsLoaded } = useAppSelector((state) => state);
	console.log(" asdasd " + currentGroup);

	return (
		<Loader active={!isGroupsLoaded}>
			{isGroupsLoaded && currentGroup !== null ? <GroupInfo group={currentGroup}/> : <StubGroupInfo/>}
		</Loader>

	);
};
