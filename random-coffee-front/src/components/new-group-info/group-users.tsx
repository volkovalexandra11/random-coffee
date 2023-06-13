import React, { FC } from 'react';
import style from './section.module.scss';
import { TUser } from '../../types/user';
import { UserAvatar } from './user';

type Props = {
  users: TUser[];
};

export const GroupUsers: FC<Props> = React.memo(
  ({ users }) => {
    return (
      <section className={style.wrapper}>
        <span className={style.text}>Участники</span>
        <div className={style.users}>
          {users.map((user) => (
            <UserAvatar
              key={user.userId}
              name={user.firstName}
              avatar={user.profilePictureUrl}
            />
          ))}
        </div>
      </section>
    );
  },
  (prev, next) => prev.users === next.users
);
