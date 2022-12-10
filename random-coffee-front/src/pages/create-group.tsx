import { FC } from 'react';
import { TGroup } from '../types/group';
import style from "../components/group-table/group-table.module.scss";
import {Button, Gapped, Input} from "@skbkontur/react-ui";
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
