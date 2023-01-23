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
                <div className={style.errorCode}>504</div>
                <div className={style.message}>Ой, кажется на нашей стороне что-то пошло не так. Перезагрузите страницу и попробуйт еще раз</div>
            </div>
        </div>
    );
}
