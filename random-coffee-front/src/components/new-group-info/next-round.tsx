import { FC } from 'react';
import style from './section.module.scss';

type Props = {
  nextRound: string;
};

export const NextRound: FC<Props> = ({ nextRound }) => {
  return (
    <section className={style.wrapper}>
      <span className={style.text}>Следующий раунд</span>
      <div className={style.info}>
        <span className={style.text}>{nextRound}</span>
        <span className={style.text}>{nextRound}</span>
      </div>
    </section>
  );
};
