import { FC, useState } from 'react';
import { NotificationBellIcon } from '@skbkontur/icons'
import { Button } from '@skbkontur/react-ui';
import { User } from '@skbkontur/react-icons';
import style from './nav-bar.module.scss'
import { useNavigate } from 'react-router-dom';
import { ExitModal } from '../logout-modal/logout-modal';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { fetchGroupsAction } from '../../store/api-action';

export const NavBar: FC = () => {
    const dispatch = useAppDispatch();

    const { user } = useAppSelector((state) => state);
    const [opened, setOpened] = useState(false);

    const openModal = () => {
        setOpened(true);
    }

    const closeModal = () => {
        setOpened(false);
    }

    const navigate = useNavigate();

    return (
        <>
            <div className={style.navBar}>
                <div className={style.logo} onClick={() => {
                    // @ts-ignore
                    dispatch(fetchGroupsAction(user.userId));
                    navigate('')
                }}>
                    <div><h2>Cлучайный кофе</h2></div>
                </div>
                <div className={style.userPanel}>
                    <Button className={style.buttons} borderless>
                        <br/><NotificationBellIcon/>
                    </Button>
                    <Button className={style.buttons} onClick={() => navigate('/user')} borderless>
                        <span className={style.text}><User/>{user?.firstName + ' ' + user?.lastName}</span>
                    </Button>
                    <ExitModal opened={opened} close={closeModal}/>
                    <span className={style.line}/>
                    <Button className={style.buttons} onClick={openModal} borderless>
                        <span className={style.text}>Выйти</span>
                    </Button>
                </div>
            </div>
            <div style={{ paddingTop: 50 }}/>
        </>);
};
