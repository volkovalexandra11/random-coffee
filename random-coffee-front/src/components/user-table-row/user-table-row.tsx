import { FC, useState } from 'react';
import { ImageWithPlaceholder } from '../image-with-placeholder/image-with-placeholder';
import { TUser } from '../../types/user';
import { KickModal } from '../kick-user-modal/kick-user-modal';
import style from './user-table-row.module.scss';

type Props = {
    user: TUser;
    isAdmin: boolean;
    adminView: boolean;
}

export const UserTableRow: FC<Props> = ({ user, isAdmin, adminView }) => {
    const [opened, setOpened] = useState(false);

    const openModal = () => {
        setOpened(true);
    }

    const closeModal = () => {
        setOpened(false);
    }
    return (
        <section className={style.wrapper}>
            <KickModal user={user} opened={opened} close={closeModal}/>
            <div className={style.NameAvatar}>
                <ImageWithPlaceholder showPlaceholder={user.profilePictureUrl === undefined}
                                      picturePath={user.profilePictureUrl}/>
                <div className={style.GroupName}>{user.firstName + ' ' + user.lastName}</div>
            </div>
            <div className={style.information}>
                <div className={style.params}>{isAdmin ? 'Администратор' : 'Участник'}</div>
                <div className={style.params}>{(adminView && !isAdmin) &&
                    <div className={style.kick} onClick={openModal}>&#10006;</div>}</div>
            </div>
        </section>
    );
}
