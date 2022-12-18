import { FC, useEffect, useState } from 'react';
import { GroupTable } from '../components/group-table/group-table';
import { useAppSelector } from '../hooks';
import { TGroup } from "../types/group";

export const GroupPage = () => {
	// const { groups } = useAppSelector((state) => state);
	const [groups, setGroups] = useState<null|TGroup[]>(null);
	const [loaded, setLoaded] = useState(false);
	useEffect(() => {
		async function getGroups() {
			const response =  await fetch('/api/groups');
			const responseJson = await response.json();
			setGroups(responseJson.groups);
			setLoaded(true);
		}
		console.log("useEffect");
		getGroups();
	}, [])

	if (loaded) {
		return (
			<GroupTable groups={groups}/>
		);
	}
};
