import { FC } from 'react';
import { TGroup } from '../../types/group';
import style from './group-info.module.scss';
import { Button } from '@skbkontur/react-ui';
import { TUser } from '../../types/user';
import { UserTableRow } from '../user-table-row/user-table-row';
import { AdminInfo } from '../admin-info/admin-user';

type Props = {
	group: TGroup;
}

export const GroupInfo : FC<Props> = ({group}) => {
	return (
		<div className={style.background}>
			<div className={style.header}>
				<span className={style.name}>{group.name}</span>
			</div>
			<div className={style.wrapper}>
				<div className={style.information}>
					<div className={style.about}>
						<div className={style.avatar}/>
						<div>
							<div className={style.description}>{group.description}</div>
							<AdminInfo userId={group.adminUserId}/>
						</div>
					</div>
					<Button use='primary' className={style.button}>Присоединиться</Button>
				</div>
				<div className={style.users}>
					<h2>Участники</h2>
					{group.participants.map((user: TUser) =>
						<div key={user.id}>
							<UserTableRow user={user}/>
						</div>)}
				</div>
			</div>
		</div>
	);
}
