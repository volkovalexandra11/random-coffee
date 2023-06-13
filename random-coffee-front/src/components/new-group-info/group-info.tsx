import { FC, MouseEvent, useCallback, useRef, useState } from 'react';
import { Button, Gapped } from '@skbkontur/react-ui';
import style from './group-info.module.scss';
import { TGroup } from '../../types/group';
import { TUser } from '../../types/user';
import { UserTableRow } from '../user-table-row/user-table-row';
import { AdminInfo } from '../admin-info/admin-user';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { deleteGroupFromUser } from '../../store/action';
import {
  fetchGroupByIdAction,
  joinAGroup,
  leaveGroupAction,
  makeRoundAction,
} from '../../store/api-action';
import { useNavigate } from 'react-router-dom';
import { RoundMadeModal } from '../round-made-modal/round-made-modal';
import { InviteLink } from '../invite-link/invite-link';
import { NextRound } from './next-round';
import { Administrator } from './admin';
import { GroupUsers } from './group-users';

type Props = {
  group: TGroup;
  adminView: boolean;
};

export const GroupInfo: FC<Props> = ({ group, adminView }) => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [openModal, setOpenModal] = useState(false);

  const clearTimerRef = useRef();

  let { user } = useAppSelector((state) => state);
  user = user as TUser;
  let userInGroup = false;
  for (let i = 0; i < group.participants.length; i++) {
    if (user.userId === group.participants[i].userId) {
      userInGroup = true;
      break;
    }
  }
  const handlerOpenModal = useCallback(() => {
    setOpenModal(true);
    // @ts-ignore
    clearTimerRef.current = setTimeout(() => setOpenModal(false), 5000);
  }, []);

  const handlerCloseModal = useCallback(() => {
    setOpenModal(false);
    clearTimeout(clearTimerRef.current);
  }, []);

  const handleLeaveButtonClick = (_: MouseEvent<HTMLButtonElement>) => {
    // @ts-ignore
    dispatch(deleteGroupFromUser());
    // @ts-ignore
    dispatch(leaveGroupAction(group.groupId));
    navigate('/');
  };

  const handleMakeRoundClick = () => {
    dispatch(makeRoundAction(group.groupId)).catch((e) => console.log(e));
    handlerOpenModal();
  };

  const handleJoinButtonClick = () => {
    dispatch(joinAGroup(group.groupId));
    dispatch(fetchGroupByIdAction(group.groupId));
  };

  return (
    <div className={style.background}>
      {openModal && <RoundMadeModal onClose={handlerCloseModal} />}
      <div className={style.wrapper}>
        <section className={style.panel}>
          <div className={style.image_holder}>
            <div className={style.avatar}>
              {group.groupPictureUrl && (
                <img
                  className={style.group_avatar}
                  alt=""
                  src={group.groupPictureUrl}
                  referrerPolicy="no-referrer"
                />
              )}
            </div>
          </div>
          <Gapped vertical gap={10}>
            <h1>{group.name}</h1>
            <span className={style.description}>{group.description}</span>
            <div className={style.buttons}>
              {!userInGroup && (
                <Button
                  use="primary"
                  className={style.button}
                  onClick={handleJoinButtonClick}>
                  Присоединится
                </Button>
              )}
              {/*{adminView && userInGroup &&*/}
              {/*	<Button use='primary' className={style.button} onClick={()=>navigate(`/group/${group.groupId}/edit`)}>Редактировать</Button>}*/}
              {adminView && userInGroup && (
                <Button
                  use="primary"
                  className={style.button}
                  onClick={handleMakeRoundClick}>
                  Начать случайный кофе
                </Button>
              )}
              {!adminView && userInGroup && (
                <Button
                  use="pay"
                  className={style.button}
                  onClick={handleLeaveButtonClick}>
                  Покинуть
                </Button>
              )}
            </div>
            <InviteLink link={window.location.href} />
          </Gapped>
        </section>
        <section className={style.info}>
          <NextRound nextRound={'26 апреля'} />
          <Administrator admin={group.admin} />
          <GroupUsers users={group.participants} />
        </section>
      </div>
    </div>
  );
};
