import style from '../../new-group-info/group-info.module.scss';
import { Button, Gapped } from '@skbkontur/react-ui';

export const StubGroupInfo = () => {
  return (
    <div className={style.background}>
      <div className={style.wrapper}>
        <section className={style.panel}>
          <div className={style.image_holder}>
            <div className={style.avatar} />
          </div>
          <Gapped vertical gap={10}>
            <h1 />
            <span className={style.description} />
            <div className={style.buttons}>
              <Button use="pay" className={style.button} disabled />
            </div>
          </Gapped>
        </section>
        <section className={style.info}>
          <section className={style.stub} />
          <section className={style.stub} />
          <section className={style.stub} />
        </section>
      </div>
    </div>
  );
};
