import { FC, useState, useCallback } from 'react';
import { Button } from '@skbkontur/react-ui';
import style from './nav-bar.module.scss';
import { useNavigate } from 'react-router-dom';
import { ExitModal } from '../logout-modal/logout-modal';
import { useAppSelector } from '../../hooks';

export const NavBar: FC = () => {
  const { user } = useAppSelector((state) => state);
  const [opened, setOpened] = useState(false);
  const [isOpened, setIsOpened] = useState(false);
  const handlerOpenMenu = () => setIsOpened(!isOpened);

  const openModal = useCallback(() => {
    setOpened(true);
  }, []);

  const closeModal = useCallback(() => {
    setOpened(false);
  }, []);

  const navigate = useNavigate();

  return (
    <>
      <nav className={`${style.navBar} ${isOpened && style.responsive}`}>
        <div
          className={`${style.text} ${style.logo}`}
          onClick={() => {
            navigate('');
          }}>
          Cлучайный кофе
        </div>
        <div className={style.controls}>
          <div className={style.userPanel}>
            <Button
              className={style.buttons}
              onClick={() => navigate('/user')}
              borderless>
              <span className={style.text}>
                {user?.firstName + ' ' + user?.lastName}
              </span>
            </Button>
            <ExitModal opened={opened} close={closeModal} />
            <span className={style.line} />
            <Button className={style.buttons} onClick={openModal} borderless>
              <span className={style.text}>Выйти</span>
            </Button>
          </div>
        </div>
        <button
          className={`${style.icon} ${style.button}`}
          onClick={handlerOpenMenu}>
          {isOpened ? '✖' : '☰'}
        </button>
      </nav>
      <div style={{ paddingTop: 50 }} />
    </>
  );
};
