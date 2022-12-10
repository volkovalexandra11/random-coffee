import { CredentialResponse, GoogleLogin } from '@react-oauth/google';
import style from './login-with-google.module.scss';

export const LoginWithGoogleForm = () => {
	const handleSuccess = (res: CredentialResponse) => {
		console.log('Success', res);
	}

	const handleError = () => {
		console.log('Error')
	}

	return (
		<div className={style.page}>
			<div className={style.wrapper}>
				<GoogleLogin
					onSuccess={handleSuccess}
					onError={handleError}
				/>
			</div>
		</div>
	)
}
