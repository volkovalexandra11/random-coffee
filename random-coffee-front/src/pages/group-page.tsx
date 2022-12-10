import {FC, useEffect, useState} from 'react';
import style from "../styles/group-page.module.scss";
import {TGroup} from "../types/group";
import {Button} from "@skbkontur/react-ui";
import {UserTableRow} from "../components/user-table-row/user-table-row";
import {useNavigate, useParams} from "react-router-dom";
import {useSelector} from "react-redux";
import {TUser} from "../types/user";


export const Group: FC = () => {
    const { groupId } = useParams();
    // @ts-ignore
    const groupList: TGroup[] = useSelector(state => state.groups);
    // @ts-ignore
    const group = groupList.find(group => group.id == groupId);
    if (group == undefined) return (<></>)
    return (
        <div className={style.background}>
            <div className={style.header}>
                <span className={style.name}>Группа {group.name}</span>
            </div>
            <div className={style.wrapper}>
                <div className={style.information}>
                    <div className={style.about}>
                        <div className={style.avatar}/>
                        <div>
                            <div className={style.description}>{group.description}</div>
                            <div className={style.admin}>Администратор: ???</div>
                        </div>
                    </div>
                    <Button use='primary' className={style.button}>Присоединиться</Button>
                </div>
                <div className={style.users}>
                    <h2>Участники</h2>
                    {group.users.map((user: TUser)=>
                        <div key={user.id}>
                            <UserTableRow user={user}/>
                        </div>)}
                </div>
            </div>
        </div>
    );
};
