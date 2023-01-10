import { Modal } from '@skbkontur/react-ui';
import style from './login-with-google.module.scss';
import { GoogleSvg } from './google-svg';

export const LoginWithGoogleForm = () => {
	return (
		<Modal>
			<Modal.Header>Войдите или зарегистрируйтесь</Modal.Header>
			<Modal.Footer>
				<div className={style.center}>
					<button className={style.googleButton} onClick={() => window.location.href = '/login/google-login'}>
						<GoogleSvg/>
					</button>
				</div>
			</Modal.Footer>
		</Modal>
	)
};
