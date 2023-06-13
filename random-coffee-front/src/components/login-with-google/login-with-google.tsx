import {
  Button,
  Gapped,
  Input,
  ThemeFactory,
  ThemeContext,
} from '@skbkontur/react-ui';
import style from './login-with-google.module.scss';
import { useAppSelector } from '../../hooks';
import { AuthStatus } from '../../types/authStatus';
import { changeAuthStatus } from '../../store/action';
import store from '../../store';
import { FC } from 'react';

export const LoginWithGoogleForm: FC = () => {
  // const dispatch = useAppDispatch();
  const { authStatus } = useAppSelector((state) => state);
  console.log('google');
  const theme = ThemeFactory.create({
    btnPayTextColor: 'white',
  });

  const handleClick = () => {
    console.log(authStatus);
    store.dispatch(changeAuthStatus({ authStatus: AuthStatus.Unknown }));
    console.log(authStatus);
    window.location.href = '/login/google-login';
  };

  return (
    <div className={style.background}>
      <div className={style.wrapper}>
        <section className={style.info}>
          <h1 className={style.headerH1}>Случайный <br/> кофе</h1>
          <span>
            Крокодилы ходят лежа, а здесь ты сможешь найти новые знакомства,
            идеи, или просто хорошо провести время с людьми с похожими
            интересами
          </span>
        </section>
        <form className={style.form}>
          <div className={style.login}>Уже есть аккаунт?  Войти</div>
          <Gapped gap={14} vertical style={{width: "100%"}}>
            <h2 className={style.headerH2}>Регистрация</h2>
            <Button
              use={'primary'}
              className={style.button}
              onClick={handleClick}>
              Зарегистрироваться через Google
            </Button>
            <div className={style.parser}>или</div>
            <Input placeholder={"Имя"} className={style.input}/>
            <Input placeholder={"Фамилия"} className={style.input}/>
            <Input placeholder={"Придумайте логин"} className={style.input}/>
            <Input placeholder={"Придумайте пароль"} className={style.input}/>
            <Input placeholder={"Повторите пароль"} className={style.input}/>
            <Input placeholder={"Почта для связи"} className={style.input}/>
            <ThemeContext.Provider value={theme}>
              <Button use={'pay'} className={style.button}>Зарегистрироваться</Button>
            </ThemeContext.Provider>
          </Gapped>
        </form>
      </div>
    </div>
  );
};
