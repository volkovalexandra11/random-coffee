import { FC, MouseEvent, useState } from 'react';
import { Button } from '@skbkontur/react-ui';
import style from './group-info.module.scss';
import { TGroup } from '../../types/group';
import { TUser } from '../../types/user';
import { UserTableRow } from '../user-table-row/user-table-row';
import { AdminInfo } from '../admin-info/admin-user';
import { useAppDispatch } from '../../hooks';
import { deleteGroupFromUser } from '../../store/action';
import { leaveGroupAction, makeRoundAction } from '../../store/api-action';
import { useNavigate } from 'react-router-dom';
import { RoundMadeModal } from '../round-made-modal/round-made-modal';

type Props = {
	group: TGroup;
	adminView: boolean;
}

export const GroupInfo: FC<Props> = ({ group, adminView }) => {
	const dispatch = useAppDispatch();
	const navigate = useNavigate();

	const [openModal, setOpenModal] = useState(false);

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

	return (
		<div className={style.background}>
			{openModal && <RoundMadeModal onClose={() => setOpenModal(false)}/>}
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
						{adminView && <Button use='primary' width={'200px'} className={style.button}>Редактировать</Button>}
						{adminView && <Button use='primary' width={'200px'} className={style.button}
                                  onClick={handleMakeRoundClick}
            >Начать случайный кофе</Button>}
						{!adminView && <Button use='primary' width={'200px'} className={style.button}
                                   onClick={handleLeaveButtonClick}
            >Покинуть</Button>}
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
