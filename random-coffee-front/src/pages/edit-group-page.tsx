import { FC } from 'react';
import style from '../styles/create-group.module.scss';
import {EditGroupForm} from "../components/edit-group/edit-group";


export const EditGroup: FC = () => {
    return (
        <div className={style.background}>
            <div className={style.header}>
                <span className={style.name}>Редактирование группы</span>
            </div>
            <div className={style.wrapper}>
                <EditGroupForm/>
            </div>
        </div>
    );
};