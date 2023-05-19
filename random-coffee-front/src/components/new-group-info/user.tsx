import React, {FC} from 'react';
import style from './section.module.scss';
import { ImageWithPlaceholder } from '../image-with-placeholder/image-with-placeholder';

type Props = {
  name: string;
  avatar?: string;
};

export const UserAvatar: FC<Props> = React.memo(({avatar, name}) => {
  return (
    <div className={style.userAvatar}>
      <ImageWithPlaceholder
        showPlaceholder={avatar === undefined}
        picturePath={avatar}
      />
      <span>{name}</span>
    </div>
  );
},(prev, next) => (
    prev.name === next.name && prev.avatar === next.avatar
));
