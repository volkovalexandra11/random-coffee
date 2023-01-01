import { FC } from 'react';
import style from '../group-info/group-info.module.scss';
import { TUser } from '../../types/user';

type Props = {
	user: TUser;
}

export const AdminInfo: FC<Props> = ({ user }) => {
	return (
		<div className={style.admin}>{`Администратор: ${user.firstName} ${user.lastName}`}</div>
	);
}
