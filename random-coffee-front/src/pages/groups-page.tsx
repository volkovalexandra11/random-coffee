import { FC, useEffect, useState } from 'react';
import { GroupTable } from '../components/group-table/group-table';
import { useAppSelector } from '../hooks';
import { TGroup } from '../types/group';

export const GroupsPage: FC = () => {
	// TODO убрать в редакс
	//const { groups } = useAppSelector((state) => state);
	const [loaded, setLoaded] = useState(false);
	const [groups, setGroups] = useState<TGroup[]>([]);

	useEffect(() => {
		async function getGroups() {
			const resp = await fetch('/api/groups?userId=43ef1000-0000-0000-0000-000000000000');
			const respJson = await resp.json();
			console.log(respJson);
			//setGroups(respJson.groupIds);
			setLoaded(true);
		}

		console.log('useEffect');
		getGroups();
	}, [])


	if (loaded) {
		return (
			<GroupTable groups={groups}/>
		);
	}

	return (
		<p>Loading</p>
	)
};
