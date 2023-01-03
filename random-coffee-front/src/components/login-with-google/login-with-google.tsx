import style from './login-with-google.module.scss';
import { Button, Modal } from '@skbkontur/react-ui';
import { GoogleSvg } from './google-svg';

export const LoginWithGoogleForm = () => {
	const handleClick = () => {
		async function loginOnBack() {
			const resp = await fetch('/login', { method: 'POST' });
			const respJson = await resp.json();
			console.log(respJson);
		}

		loginOnBack();
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
