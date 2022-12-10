import {FC, useState} from 'react';
import { NotificationBellIcon } from '@skbkontur/icons'
import {User} from '@skbkontur/react-icons';
import style from './nav-bar.module.scss'
import { TUser } from "../../types/user";
import {Button, Modal} from '@skbkontur/react-ui';
import {useNavigate} from "react-router-dom";

export const NavBar: FC<TUser> = (props) => {
	const [opened, setOpened] = useState(false);

	function renderModal() {
		return (
			<Modal width={"120%"} onClose={close}>
				<Modal.Header>Хочешь выйти?</Modal.Header>
				<Modal.Body>
				</Modal.Body>
				<Modal.Footer>
					<div className={style.modalButtons}>
						<Button className={style.button} use="danger" onClick={() => {alert('Logout'); close()}}>Да</Button>
						<Button className={style.button} onClick={close}>Отменить</Button>
					</div>
				</Modal.Footer>
			</Modal>
		);
	}

	function open() {
		setOpened(true);
	}

	function close() {
		setOpened(false);
	}

	const navigate = useNavigate();

	return (
		<>
			<div className={style.navBar}>
				<div className={style.logo} onClick={() => navigate('/')}>
					<div><h2>Cлучайный кофе</h2></div>
				</div>
				<div className={style.userPanel}>
					<Button className={style.buttons} borderless>
						<br/><NotificationBellIcon/>
					</Button>
					<a className={style.buttons} href={''}>
						<span className={style.text}><User/>{props.firstName + " " + props.lastName}</span>
					</a>
					{opened && renderModal()}
					<span className={style.line}/>
					<Button className={style.buttons} onClick={open} borderless>
						<span className={style.text}>Выйти</span>
					</Button>
				</div>
			</div>
			<div style={{ paddingTop: 50 }}/>
		</>);
};
