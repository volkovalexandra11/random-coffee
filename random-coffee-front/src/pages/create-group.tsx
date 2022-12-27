import { FC } from 'react';
import style from "../styles/create-group.module.scss";
import {GroupCreateForm} from "../components/group-create-form/group-create-form";


export const CreateGroup: FC = () => {
    return (
        <div className={style.background}>
            <div className={style.header}>
                <span className={style.name}>Создание группы</span>
            </div>
            <div className={style.wrapper}>
                <GroupCreateForm/>
            </div>
        </div>
    );
};
