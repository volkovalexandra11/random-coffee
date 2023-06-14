import { Button } from '@skbkontur/react-ui';
import style from '../../new-group-table/group-table.module.scss';
import { StubGroupTableRow } from '../stub-group-table-row/stub-group-table-row';

export const StubGroupTable = () => {
  return (
    <div className={style.background}>
      <div className={style.wrapper}>
        <section className={style.panel}>
          <div className={style.controls}>
            <div className={style.left_controls}>
              <Button use="pay" className={style.all_groups} disabled>
                Все группы
              </Button>
              <Button use="default" disabled>
                Управляемые
              </Button>
            </div>
            <div className={style.right_controls}>
              <Button use="pay" disabled>
                Создать группу
              </Button>
            </div>
          </div>
          <input placeholder={`Поиск группы`} className={style.input} disabled />
          {[1, 2, 3].map((i) => (
            <StubGroupTableRow key={i} />
          ))}
        </section>
        <section className={style.info}>
          <h2 className={style.header}>Рекомендуемые группы</h2>
          <></>
        </section>
      </div>
    </div>
  );
};
