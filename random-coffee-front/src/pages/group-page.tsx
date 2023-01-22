import {FC, useEffect} from 'react';
import { GroupInfo } from '../components/group-info/group-info';
import { useAppSelector } from "../hooks";
import { Loader } from "@skbkontur/react-ui";
import { StubGroupInfo } from '../components/stub/stub-group-info/stub-group-info';
import {useParams} from "react-router-dom";
import store from "../store";
import {fetchGroupByIdAction} from "../store/api-action";
import {TUser} from "../types/user";


export const Group: FC = () => {
	const { groupId } = useParams();
	useEffect(() => {
		store.dispatch(fetchGroupByIdAction(groupId));
	}, [])
	const { currentGroup } = useAppSelector((state) => state);
	const { isGroupsLoaded } = useAppSelector((state) => state);
	const { user } = useAppSelector((state) => state);

	return (
		<Loader active={!isGroupsLoaded}>
			{isGroupsLoaded && currentGroup !== null ?
				<GroupInfo group={currentGroup} adminView={user?.userId === currentGroup.admin.userId} user={user as TUser}/> :
				<StubGroupInfo/>}
		</Loader>
	);
};
