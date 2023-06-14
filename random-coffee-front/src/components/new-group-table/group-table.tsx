import { ChangeEvent, FC, useCallback, useState } from 'react';
import { TGroup, TGroupShort } from '../../types/group';
import { GroupTableRow } from '../group-table-row/group-table-row';
import style from './group-table.module.scss';
import { Button, ThemeContext, ThemeFactory } from '@skbkontur/react-ui';
import { Link, useNavigate } from 'react-router-dom';
import {useAppSelector} from "../../hooks";

type Props = {
  groups: TGroupShort[];
  setFilter: any;
  isAllGroup: any;
};

export const GroupTable: FC<Props> = ({ groups, setFilter, isAllGroup }) => {
  const { user } = useAppSelector((state) => state);
  const navigate = useNavigate();
  const [copyData] = useState(groups);
  const [group, setGroups] = useState(groups);
  const [isBtnAllGroupUse, setIsBtnAllGroupUse] = useState(isAllGroup);
  const handlerCreateGroup = useCallback(() => navigate('/create'), []);
  const themeAllGroups = ThemeFactory.create({
    btnBorderRadiusSmall: `10px`,
    btnDefaultTextColor: `rgba(163, 163, 163, 1)`,
    btnDefaultBg: 'transparent',
    btnDefaultActiveBorderColor: 'rgba(248, 170, 99, 1)',
    btnPayTextColor: 'rgba(248, 170, 99, 1)',
    btnPayBg: 'transparent',
    btnPayHoverBg: `rgba(248, 170, 99, 0.2)`,
    btnDefaultHoverBg: 'rgba(163, 163, 163, 0.2)',
    btnPayActiveBg: `rgba(248, 170, 99, 0.4)`,
  });

  const themeCreateGroup = ThemeFactory.create({
    btnPayTextColor: 'white',
    btnBorderRadiusSmall: `10px`,
  });

  const onSearchGroup = (event: ChangeEvent<HTMLInputElement>) => {
    const target = event.target;
    const input = target.value;
    if (input === '') {
      setGroups(copyData);
      return;
    }
    let resultData: TGroupShort[] = [];
    for (let i = 0; i < copyData.length; i++) {
      if (copyData[i].name.includes(input)) resultData.push(copyData[i]);
    }
    setGroups(resultData);
  };

  return (
    <div className={style.background}>
      <div className={style.wrapper}>
        <section className={style.panel}>
          <div className={style.controls}>
            <div className={style.left_controls}>
              <ThemeContext.Provider value={themeAllGroups}>
                <Button
                  use={isBtnAllGroupUse ? 'pay' : 'default'}
                  onClick={() => {setIsBtnAllGroupUse(true); setFilter(true); }}
                  className={style.all_groups}>
                  Все группы
                </Button>
                <Button
                  use={!isBtnAllGroupUse ? 'pay' : 'default'}
                  onClick={() => {setIsBtnAllGroupUse(false); setFilter(false); }}>
                  Управляемые
                </Button>
              </ThemeContext.Provider>
            </div>
            <div className={style.right_controls}>
              <ThemeContext.Provider value={themeCreateGroup}>
                <Button use="pay" onClick={handlerCreateGroup}>
                  Создать группу
                </Button>
              </ThemeContext.Provider>
            </div>
          </div>
          <input
            placeholder={`Поиск группы`}
            className={style.input}
            onChange={onSearchGroup}
          />
          {group.map((group) => (
            <Link key={group.groupId} to={`/group/${group.groupId}`}>
              <GroupTableRow group={group} />
            </Link>
          ))}
        </section>
        <section className={style.info}>
          <h2 className={style.header}>Рекомендуемый <span className={style.font}>кофе</span></h2>
          {/*// @ts-ignore*/}
          {user && user.tag && user.tag.length === 0 ?
              <div>
                Выбери увлечения в профиле, и здесь появится что-то интересное
              </div>
              :
              <></>
          }
        </section>
      </div>
    </div>
  );
};
