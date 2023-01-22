import { FC } from 'react';
import style from '../styles/user-page.module.scss';
import { UserForm } from '../components/user-form/user-form';


export const UserPage: FC = () => {
    return (
        <div className={style.background}>
            <div className={style.header}>
                <span className={style.name}>Профиль </span>
            </div>
            <div className={style.wrapper}>
                <UserForm/>
            </div>
        </div>
    );
};
