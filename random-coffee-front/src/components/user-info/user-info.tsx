import { FC, useState } from 'react';
import style from './user-info.module.scss';
import {
  Button,
  Gapped,
  Input,
  ThemeFactory,
  ThemeContext,
} from '@skbkontur/react-ui';
import { useAppSelector } from '../../hooks';
import { useNavigate } from 'react-router-dom';
import {forGroups} from "../../mocks/tags";
import {UserTags} from "../user-tags/user-tags";

export const UserInfo: FC = () => {
  const { user } = useAppSelector((state) => state);
  const navigator = useNavigate();
  const [isBtnForCoffeeUse, setIsBntForCoffeeUse] = useState(true);
  const [isChanging, setIsChanging] = useState(false);
  const [email, setEmail] = useState('email');
  const [tg, setTg] = useState('tg');
  const handlerClickChange = () => {
    if (!isChanging) setIsChanging(true);
    else setIsChanging(false);
  };
  const buttonTheme = ThemeFactory.create({
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

  return (
    <div className={style.wrapper}>
      <section className={style.form}>
        <section className={style.user}>
          <img
            alt="Аватар пользователя"
            //@ts-ignore
            src={user.profilePictureUrl}
            className={style.avatar}
          />
          <span className={style.name}>
            {/*@ts-ignore*/}
            {`${user.firstName} ${user.lastName}`}
            <button
              className={style.change}
              onClick={() => navigator('/user/change')}
            />
          </span>
        </section>
        <section className={style.info}>
          <div className={style.controls}>
            <ThemeContext.Provider value={buttonTheme}>
              <Button
                use={isBtnForCoffeeUse ? `pay` : `default`}
                onClick={() => setIsBntForCoffeeUse(true)}>
                Данные для кофе
              </Button>
              {/*<Button*/}
              {/*  use={!isBtnForCoffeeUse ? `pay` : `default`}*/}
              {/*  onClick={() => setIsBntForCoffeeUse(false)}>*/}
              {/*  Данные для входа*/}
              {/*</Button>*/}
            </ThemeContext.Provider>
          </div>
          <section className={style.forCoffee}>
            {isBtnForCoffeeUse ? (
              <Gapped gap={23} vertical>
                <div>
                  <span className={style.name}>
                    Способы связи
                    <button
                      className={isChanging ? style.confirm : style.change}
                      onClick={handlerClickChange}
                    />
                  </span>
                  <div className={style.title_control}>
                    {isChanging ? (
                      <>
                        <input
                          className={style.input}
                          value={email}
                          onChange={(e) => setEmail(e.target.value)}
                        />
                        <input
                          className={style.input}
                          value={tg}
                          onChange={(e) => setTg(e.target.value)}
                        />
                      </>
                    ) : (
                      <>
                        <div className={style.title}>{email}</div>
                        <div className={style.title}>{tg}</div>
                      </>
                    )}
                  </div>
                </div>
                <span className={style.name}>
                  Увлечение
                  <button className={style.change} onClick={() => navigator('/user/tags')}/>
                </span>
                <div className={style.forTags}>
                {/*@ts-ignore*/}
                {user.tag && user.tag.map(t => <UserTags name={t} key={t}/>)}
                </div>
              </Gapped>
            ) : (
              <></>
            )}
          </section>
        </section>
      </section>
    </div>
  );
};
