import {FC, useEffect, useState} from 'react';
import { NotificationBellIcon } from '@skbkontur/icons'
import {User} from '@skbkontur/react-icons';
import style from './nav-bar.module.scss'
import {Button} from '@skbkontur/react-ui';
import {useNavigate, useLocation} from "react-router-dom";
import {useSelector} from "react-redux";
import {ExitModal} from "../logout-modal/logout-modal";

export const NavBar: FC = () => {
	// @ts-ignore
	const user = useSelector(state => state.user);
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
				<div className={style.logo} onClick={()=>{navigate('')}}>
					<div><h2>Cлучайный кофе</h2></div>
				</div>
				<div className={style.userPanel}>
					<Button className={style.buttons} borderless>
						<br/><NotificationBellIcon/>
					</Button>
					<a className={style.buttons} href={''}>
						<span className={style.text}><User/>{user.firstName + " " + user.lastName}</span>
					</a>
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
