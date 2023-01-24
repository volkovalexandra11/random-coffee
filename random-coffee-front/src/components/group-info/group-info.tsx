import { FC, MouseEvent, useState } from 'react';
import {Button, DropdownMenu, Gapped, MenuSeparator} from '@skbkontur/react-ui';
import style from './group-info.module.scss';
import { TGroup } from '../../types/group';
import { TUser } from '../../types/user';
import { UserTableRow } from '../user-table-row/user-table-row';
import { AdminInfo } from '../admin-info/admin-user';
import {useAppDispatch, useAppSelector} from '../../hooks';
import { deleteGroupFromUser } from '../../store/action';
import {joinAGroup, leaveGroupAction, makeRoundAction} from '../../store/api-action';
import { useNavigate } from 'react-router-dom';
import { RoundMadeModal } from '../round-made-modal/round-made-modal';
import {InviteLink} from "../invite-link/invite-link";

type Props = {
	group: TGroup;
	adminView: boolean;
}

export const GroupInfo: FC<Props> = ({ group, adminView }) => {
	const dispatch = useAppDispatch();
	const navigate = useNavigate();


	const [openModal, setOpenModal] = useState(false);
	let { user } = useAppSelector((state) => state);
	user = user as TUser;
	let userInGroup = false;
	for (let i = 0; i < group.participants.length; i++) {
		if (user.userId === group.participants[i].userId) {
			userInGroup = true
			break;
		}
	}
	const handleLeaveButtonClick = (_: MouseEvent<HTMLButtonElement>) => {
		// @ts-ignore
		dispatch(deleteGroupFromUser());
		// @ts-ignore
		dispatch(leaveGroupAction(group.groupId));
		navigate('/');
	};

	const handleMakeRoundClick = () => {
		dispatch(makeRoundAction(group.groupId));
		setOpenModal(true);
	}

	const handleJoinButtonClick = () => {
		dispatch(joinAGroup(group.groupId))
	}

	return (
		<div className={style.background}>
			{openModal && <RoundMadeModal onClose={() => setOpenModal(false)}/>}
			<div className={style.header}>
				<span className={style.name}>{group.name}</span>
			</div>
			<div className={style.wrapper}>
				<div className={style.information}>
					<div className={style.about}>
						<div className={style.avatar}>
							{group.groupPictureUrl && <img className={style.avatar} src={group.groupPictureUrl} referrerPolicy='no-referrer'/>}
						</div>
						<div>
							<div className={style.description}>{group.description}</div>
							<Gapped vertical gap={10}>
							<AdminInfo user={group.admin}/>
							<InviteLink link={window.location.href}/>
							</Gapped>
						</div>
					</div>
					<div className={style.buttons}>
							{!userInGroup && <Button use='primary' width={"200px"} className={style.button}
													 onClick={handleJoinButtonClick}>Присоединится</Button>}
							{/*{adminView && userInGroup &&*/}
							{/*	<Button use='primary' width={"200px"} className={style.button} onClick={()=>navigate(`/group/${group.groupId}/edit`)}>Редактировать</Button>}*/}
							{adminView && userInGroup && <Button use='primary' width={"200px"} className={style.button}
																 onClick={handleMakeRoundClick}>Начать случайный
								кофе</Button>}
							{!adminView && userInGroup && <Button use='primary' width={"200px"} className={style.button}
																  onClick={handleLeaveButtonClick}>Покинуть</Button>}
					</div>
				</div>
				<div className={style.users}>
					<h2>Участники</h2>
					{group.participants.map((user: TUser) =>
						<div key={user.userId}>
							<UserTableRow user={user} isAdmin={group.admin.userId === user.userId}
														adminView={adminView}
							/>
						</div>)}
				</div>
			</div>
		</div>
	);
}
