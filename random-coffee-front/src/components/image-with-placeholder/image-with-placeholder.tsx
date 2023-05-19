import { FC } from 'react';
import style from './image-with-placeholder.module.scss';

type Props = {
  showPlaceholder: boolean;
  picturePath?: string;
};

export const ImageWithPlaceholder: FC<Props> = ({
  showPlaceholder,
  picturePath,
}) => {
  const getPlaceholder = () => {
    return <div className={style.placeholder} />;
  };

  if (showPlaceholder) {
    return getPlaceholder();
  } else {
    return (
      // eslint-disable-next-line jsx-a11y/img-redundant-alt
      <img className={style.placeholder} src={picturePath} referrerPolicy="no-referrer" alt="picture" />
    );
  }
};
