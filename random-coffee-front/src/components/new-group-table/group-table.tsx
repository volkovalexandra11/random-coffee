import { FC, useCallback } from 'react';
import { TGroupShort } from '../../types/group';
import { GroupTableRow } from '../group-table-row/group-table-row';
import style from './group-table.module.scss';
import { Button, ThemeContext, ThemeFactory } from '@skbkontur/react-ui';
import { Link, useNavigate } from 'react-router-dom';

type Props = {
  groups: TGroupShort[];
};

export const GroupTable: FC<Props> = ({ groups }) => {
  const navigate = useNavigate();
  const handlerCreateGroup = useCallback(() => navigate('/create'), []);
  const themeAllGroups = ThemeFactory.create({
    btnPayHoverBg: `rgb(242,242,242)`,
    btnPayTextColor: 'rgba(248, 170, 99, 1)',
    btnBorderRadiusSmall: `10px`,
    btnPayBg: `transparent`,
    btnPayActiveBg: 'rgb(242,242,242)',
    btnDefaultTextColor: `rgba(163, 163, 163, 1)`,
  });
  const themeCreateGroup = ThemeFactory.create({
    btnPayTextColor: 'white',
    btnBorderRadiusSmall: `10px`,
  });

  return (
    <div className={style.background}>
      <div className={style.wrapper}>
        <section className={style.panel}>
          <div className={style.controls}>
            <div className={style.left_controls}>
              <ThemeContext.Provider value={themeAllGroups}>
                <Button use="pay" className={style.all_groups}>
                  Все группы
                </Button>
                <Button>Управляемые</Button>
              </ThemeContext.Provider>
            </div>
            <div className={style.right_controls}>
              <ThemeContext.Provider value={themeCreateGroup}>
                <Button use="pay" onClick={handlerCreateGroup}>Создать группу</Button>
              </ThemeContext.Provider>
            </div>
          </div>
          <input placeholder={`Поиск группы`} className={style.input} />
          {groups.map((group) => (
            <Link key={group.groupId} to={`/group/${group.groupId}`}>
              <GroupTableRow group={group} />
            </Link>
          ))}
        </section>
        <section className={style.info}>
          <span className={style.header}>Рекомендуемые группы</span>
        </section>
      </div>
    </div>
  );
};
