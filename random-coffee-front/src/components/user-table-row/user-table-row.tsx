import { FC } from 'react';
import style from './user-table-row.module.scss';
import { ImageWithPlaceholder } from '../image-with-placeholder/image-with-placeholder';
import {TUser} from "../../types/user";

type Props = {
    user: TUser;
    isAdmin: boolean;
}

export const UserTableRow: FC<Props> = ({ user, isAdmin}) => {
  console.log(user);
    return (
        <section className={style.wrapper}>
            <div className={style.NameAvatar}>
                <ImageWithPlaceholder showPlaceholder={user.profilePictureUrl === undefined} picturePath={user.profilePictureUrl}/>
                <div className={style.GroupName}>{user.firstName+" "+user.lastName}</div>
            </div>
            <div className={style.information}>
                <div className={style.params}>{isAdmin ? "Администратор" :"Участник"}</div>
                <div className={style.params}></div>
            </div>
        </section>
    );
}
