import { FC } from 'react';
import style from './empty-group-list.module.scss'
import { Button } from '@skbkontur/react-ui';
import { useNavigate } from 'react-router-dom';

export const EmptyGroupList: FC = () => {
    const navigate = useNavigate();

    return (
        <div className={style.background}>
            <div className={style.header}>
                <span className={style.name}>Группы</span>
                <Button className={style.button} use='primary' onClick={() => navigate('/create')}>+ Создать
                    группу</Button>
            </div>
            <div className={style.wrapper}>
                <div className={style.message}> Похоже Вы ещё не состоите ни в одной группе, создайте новую или
                    дождитесь приглашения от Ваших друзей.
                </div>
            </div>
        </div>);
};
