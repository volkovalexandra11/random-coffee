import { FC } from 'react';
import { TGroup } from '../../types/group';
import style from './group-info.module.scss';
import { Button } from '@skbkontur/react-ui';
import { TUser } from '../../types/user';
import { UserTableRow } from '../user-table-row/user-table-row';
import { AdminInfo } from '../admin-info/admin-user';
import {useAppSelector} from "../../hooks";
import store from "../../store";
import {JoinAGroup, LeaveFromGroup} from "../../store/api-action";

type Props = {
	user: TUser;
	group: TGroup;
	adminView: boolean;
}

export const GroupInfo: FC<Props> = ({user, group, adminView}) => {
	let userInGroup = false;
	for (let i = 0; i < group.participants.length; i++) {
		if (user.userId === group.participants[i].userId) {
			userInGroup = true
			break;
		}
	}

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
							<AdminInfo user={group.admin}/>
						</div>
					</div>
					<div className={style.buttons}>
						{!userInGroup && <Button use='primary' width={"200px"} className={style.button}
												 onClick={() => store.dispatch(JoinAGroup(group.groupId))}>Присоединится</Button>}
						{adminView && userInGroup &&
							<Button use='primary' width={"200px"} className={style.button}>Редактировать</Button>}
						{adminView && userInGroup && <Button use='primary' width={"200px"} className={style.button}
															 onClick={() => alert("раунд проведён")}>Начать случайный
							кофе</Button>}
						{!adminView && userInGroup && <Button use='primary' width={"200px"} className={style.button}
															  onClick={() => store.dispatch(LeaveFromGroup(group.groupId))}>Покинуть</Button>}
					</div>
				</div>
				<div className={style.users}>
					<h2>Участники</h2>
					{group.participants.map((user: TUser) =>
						<div key={user.userId}>
							<UserTableRow user={user} isAdmin={group.admin.userId === user.userId}
										  adminView={adminView}/>
						</div>)}
				</div>
			</div>
		</div>
	);
}
