import { FC } from 'react';
import { NotificationBellIcon } from '@skbkontur/icons'
import style from './nav-bar.module.scss'
import { TUser } from "../../types/user";
import { Button } from '@skbkontur/react-ui';

export const NavBar: FC<TUser> = (props) => {
	return (
		<>
			<div className={style.navBar}>
				<div className={style.logo}>
					<a href={''}><h2>Cлучайный кофе</h2></a>
				</div>
				<div className={style.userPanel}>
					<Button className={style.buttons} borderless>
						<br/><NotificationBellIcon/>
					</Button>
					<a className={style.buttons} href={''}>
						<span className={style.text}>{props.firstName + " " + props.lastName}</span>
					</a>
					<span className={style.line}/>
					<Button className={style.buttons} onClick={() => alert('Logout!')} borderless>
						<span className={style.text}>Выйти</span>
					</Button>
				</div>
			</div>
			<div style={{ paddingTop: 50 }}/>
		</>);
};
