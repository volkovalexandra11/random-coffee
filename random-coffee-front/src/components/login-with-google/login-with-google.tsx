import { CredentialResponse, GoogleLogin } from '@react-oauth/google';
import jwt_decode from 'jwt-decode';
import style from './login-with-google.module.scss';

export const LoginWithGoogleForm = () => {
	const handleSuccess = (res: CredentialResponse) => {
		console.log('Success', res);
		if (res.credential) {
			const decoded = jwt_decode(res.credential);
			console.log(decoded);
		}
		else
			handleError();
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
