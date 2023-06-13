import React, { FC } from 'react';
import style from './section.module.scss';
import { TUser } from '../../types/user';
import { ImageWithPlaceholder } from '../image-with-placeholder/image-with-placeholder';

type Props = {
  admin: TUser;
};

export const Administrator: FC<Props> = React.memo(({ admin }) => {
  return (
    <section className={style.wrapper}>
      <div className={style.adminHolder}>
        <div className={style.user}>
          <ImageWithPlaceholder
            showPlaceholder={admin.profilePictureUrl === undefined}
            picturePath={admin.profilePictureUrl}
          />
          <span
            className={
              style.text
            }>{`${admin.lastName} ${admin.firstName}`}</span>
        </div>
        <span className={style.text}>Администратор</span>
      </div>
    </section>
  );
}, (prev, next) => (
    prev.admin === next.admin
));
