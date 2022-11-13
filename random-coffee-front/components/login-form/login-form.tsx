import { ChangeEvent, MouseEvent, FC, useState } from 'react';
import { Button, Gapped, Input, Link } from '@skbkontur/react-ui';
import style from './login-form.module.scss'

export const LoginForm: FC = () => {
	const [loginEmpty, setLoginEmpty] = useState(true);
	const [passwordEmpty, setPasswordEmpty] = useState(true);

	const handleLoginChange = (event: ChangeEvent<HTMLInputElement>) => {
		setLoginEmpty(event.target.value.length === 0);
	}

	const handlePasswordChange = (event: ChangeEvent<HTMLInputElement>) => {
	  setPasswordEmpty(event.target.value.length === 0);
	}

	const handleButtonClick = (event: MouseEvent<HTMLButtonElement>) => {
		if (loginEmpty || passwordEmpty) {
			event.preventDefault();
			return;
		}
		console.log('login-form goes');
	}

	return (
		<div className={style.wrapper}>
			<div className={style.form}>
				<h2 className={style.header}>Войти</h2>
				<Gapped gap={15} vertical>
					<label className={style.label}>
						{'Логин или почта'}
						<Input type={'text'} onChange={handleLoginChange}/>
					</label>
					<label className={style.label}>
						{'Пароль'}
						<Input type={'password'} onChange={handlePasswordChange}/>
					</label>
				</Gapped>
				<Gapped gap={15} className={style.buttons}>
					<Button onClick={handleButtonClick} use='primary'>Войти</Button>
					<Button>Забыли пароль</Button>
				</Gapped>
				<p className={style.toSignUp}>Нет аккаунта? <Link href='/signup/signup'>Зарегистрируйтесь!</Link></p>
			</div>
		</div>
	)
}
