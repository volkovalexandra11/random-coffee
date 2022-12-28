import { FC, useEffect, useState } from 'react';
import style from '../group-info/group-info.module.scss';
import { TUser } from '../../types/user';

type Props = {
	userId: string;
}

export const AdminInfo : FC<Props> = ({userId}) => {
	const [userInfo, setUserInfo] = useState<TUser>();
	const [loaded, setLoaded] = useState(false);

	//TODO унести в редакс
	useEffect(() => {
		async function getUserInfo() {
			const resp = await fetch(`/api/users/${userId}`);
			const respJson = await resp.json();
			setUserInfo(respJson);
			console.log(respJson);
			setLoaded(true);
		}

		getUserInfo();
	}, [userId])

	if (!loaded) {
		return (<p>Loading</p>);
	}

	return (
		<div className={style.admin}>{`Администратор: ${userInfo?.firstName} ${userInfo?.lastName}`}</div>
	);
}
