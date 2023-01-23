import { Modal } from '@skbkontur/react-ui';
import style from './login-with-google.module.scss';
import { GoogleSvg } from './google-svg';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { AuthStatus } from '../../types/authStatus';
import { changeAuthStatus } from '../../store/action';
import store from '../../store';

export const LoginWithGoogleForm = () => {
	// const dispatch = useAppDispatch();
	const { authStatus } = useAppSelector((state) => state);
	console.log('google');

	const handleClick = () => {
		console.log(authStatus);
		store.dispatch(changeAuthStatus({ authStatus: AuthStatus.Unknown }));
		console.log(authStatus);
		window.location.href = '/login/google-login';
	}

	return (
		<Modal>
			<Modal.Header>Войдите или зарегистрируйтесь</Modal.Header>
			<Modal.Footer>
				<div className={style.center}>
					<button className={style.googleButton} onClick={handleClick}>
						<GoogleSvg/>
					</button>
				</div>
			</Modal.Footer>
		</Modal>
	)
};
