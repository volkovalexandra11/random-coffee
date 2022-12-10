import { ChangeEvent, MouseEvent, FC, useState } from 'react';
import { Button, Gapped, Input, Link } from '@skbkontur/react-ui';
import style from './signup-form.module.scss'

export const SignupForm: FC = () => {
	const [loginEmpty, setLoginEmpty] = useState(true);
	const [passwordEmpty, setPasswordEmpty] = useState(true);
	const [passwordRepeatEmpty, setPasswordRepeatEmpty] = useState(true);
	const [nameEmpty, setNameEmpty] = useState(true);
	const [surnameEmpty, setSurnameEmpty] = useState(true);

	const handleLoginChange = (event: ChangeEvent<HTMLInputElement>) => {
		setLoginEmpty(event.target.value.length === 0);
	}

	const handlePasswordChange = (event: ChangeEvent<HTMLInputElement>) => {
		setPasswordEmpty(event.target.value.length === 0);
	}

	const handlePasswordRepeatChange = (event: ChangeEvent<HTMLInputElement>) => {
		setPasswordRepeatEmpty(event.target.value.length === 0);
	}

	const handleNameChange = (event: ChangeEvent<HTMLInputElement>) => {
	  setNameEmpty(event.target.value.length === 0);
	}

	const handleSurnameChange = (event: ChangeEvent<HTMLInputElement>) => {
		setSurnameEmpty(event.target.value.length === 0);
	}

	const handleButtonClick = (event: MouseEvent<HTMLButtonElement>) => {
		if (loginEmpty || passwordEmpty || passwordRepeatEmpty || nameEmpty || surnameEmpty) {
			event.preventDefault();
			return;
		}
		console.log('login-form goes');
	}

	return (
		<div className={style.wrapper}>
			<div className={style.form}>
				<h2 className={style.header}>Регистрация</h2>
				<Gapped gap={15} vertical>
					<label className={style.label}>
						{'Имя'}
						<Input type={'text'} onChange={handleNameChange}/>
					</label>
					<label className={style.label}>
						{'Фамилия'}
						<Input type={'text'} onChange={handleSurnameChange}/>
					</label>
					<label className={style.label}>
						{'Почта'}
						<Input type={'text'} onChange={handleLoginChange}/>
					</label>
					<label className={style.label}>
						{'Пароль'}
						<Input type={'password'} onChange={handlePasswordChange}/>
					</label>
					<label className={style.label}>
						{'Повторите пароль'}
						<Input type={'password'} onChange={handlePasswordRepeatChange}/>
					</label>
				</Gapped>
				<Gapped gap={15} className={style.buttons}>
					<Button onClick={handleButtonClick} use='primary'>Зарегистрироваться</Button>
				</Gapped>
			</div>
		</div>
	)
}
