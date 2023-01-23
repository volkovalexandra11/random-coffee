import { FC } from 'react';
import style from './error-page.module.scss';

type Props = {
    errorCode: number;
    message: string;
}

export const ErrorPage: FC<Props> = ({ errorCode, message }) => {
    return (
        <div className={style.wrapper}>
            <div className={style.form}>
                <div className={style.errorCode}>{errorCode}</div>
                <div className={style.message}>{message}</div>
            </div>
        </div>
    );
}
