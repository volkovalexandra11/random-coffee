import { FC, useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import { TGroup } from '../types/group';
import { GroupInfo } from '../components/group-info/group-info';


export const Group: FC = () => {
	const { groupId } = useParams();
	// @ts-ignore
	//const groupList: TGroup[] = useSelector(state => state.groups);
	// @ts-ignore
	//const group = groupList.find(group => group.id == groupId);

	const [group, setGroup] = useState<TGroup>(undefined);
	const [loaded, setLoaded] = useState(false);

	// TODO унести в редакс тулкит
	useEffect(() => {
		async function getGroupInfo() {
			const resp = await fetch(`/api/groups/${groupId}`);
			const respJson = await resp.json();
			setGroup(respJson);
			setLoaded(true);
		}

		getGroupInfo();
	}, [groupId])


	if (!loaded) {
		return <p>Loading</p>
	}

	console.log(group);

	return (
		<GroupInfo group={group}></GroupInfo>
	);
};
