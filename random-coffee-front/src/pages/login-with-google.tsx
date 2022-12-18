import { GoogleOAuthProvider } from '@react-oauth/google';
import { LoginWithGoogleForm } from '../components/login-with-google/login-with-google';

const clientId = "806545697068-19qrbu9h73cgnnkfqr7pug16rdjgtnk1.apps.googleusercontent.com";
export function LoginWithGoogle() {
	return (
		<GoogleOAuthProvider clientId={clientId}>
			<LoginWithGoogleForm/>
		</GoogleOAuthProvider>
	)
}
