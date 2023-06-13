import { FC } from 'react';
import style from './error-page.module.scss';
import {useParams} from "react-router-dom";

export const ErrorPage: FC = () => {
    const { errorCode } = useParams();
    return (
        <div className={style.wrapper}>
            <div className={style.form}>
                <div className={style.errorCode}>{errorCode}</div>
                <div className={style.message}>Что-то пошло не так :(</div>
            </div>
        </div>
    );
}
