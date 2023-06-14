import { FC, useCallback, useState } from 'react';
import style from './user-info.module.scss';
import {
  Button,
  Gapped,
  Input,
  ThemeFactory,
  ThemeContext,
} from '@skbkontur/react-ui';
import {useAppDispatch, useAppSelector} from '../../hooks';
import { useNavigate } from 'react-router-dom';
import { UserTags } from '../user-tags/user-tags';
import { TGModal } from '../tg-modal/tg-modal';
import {updateUserInfo} from "../../store/api-action";
import {TUpdateUserDto} from "../../types/user";

export const UserInfo: FC = () => {
  const { user } = useAppSelector((state) => state);
  const [text, setText] = useState('');
  const [opened, setOpened] = useState(false);
  const dispatch = useAppDispatch();
  const navigator = useNavigate();
  const [isBtnForCoffeeUse, setIsBntForCoffeeUse] = useState(true);
  const [isChanging, setIsChanging] = useState(false);
  const [email, setEmail] = useState(
    //@ts-ignore
    user.email === undefined ? '' : user.email
  );

  const [tg, setTg] = useState(
    //@ts-ignore
    user.telegram === undefined ? '' : user.telegram
  );

  const handlerClickChange = async () => {
    if (!isChanging) setIsChanging(true);
    else {
      setIsChanging(false);
      const dto: TUpdateUserDto = {
        email: email,
        // @ts-ignore
        firstName: user.firstName,
        // @ts-ignore
        lastName: user.lastName,
        // @ts-ignore
        profilePictureUrl: user.profilePictureUrl,
        telegram: tg,
      }
      // @ts-ignore
      await dispatch(updateUserInfo({userDto: dto, userId: user.userId}));
      openModal('Не забудь подтвердить свой телеграмм');
    }
  };

  const openModal = (text: string) => {
    setText(text);
    setOpened(true);
  };

  const closeModal = useCallback(() => {
    setOpened(false);
  }, []);

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
      <TGModal text={text} opened={opened} close={closeModal} />
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
                          placeholder={'email'}
                        />
                        <input
                          className={style.input}
                          value={tg}
                          onChange={(e) => setTg(e.target.value)}
                          placeholder={'telegram'}
                        />
                      </>
                    ) : (
                      <>
                        <div className={style.title}>{email}</div>
                        <div className={style.title}>{tg}</div>
                        <button
                          className={style.hint}
                          onClick={() => openModal('Подтверди свой телеграм')}>
                          ?
                        </button>
                      </>
                    )}
                  </div>
                </div>
                <span className={style.name}>
                  Увлечение
                  <button
                    className={style.change}
                    onClick={() => navigator('/user/tags')}
                  />
                </span>
                <div className={style.forTags}>
                  {/*@ts-ignore*/}
                  {user.tag &&
                    //@ts-ignore
                    user.tag.map((t) => <UserTags name={t} key={t} />)}
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
