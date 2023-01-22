import { FC, useState } from 'react';
import style from './user-form.module.scss';
import { Button, Gapped, Input } from '@skbkontur/react-ui';
import {useAppSelector} from '../../hooks';

export const UserForm: FC = () => {
    const { user } = useAppSelector((state) => state);

    const [data, setData] = useState({
        firstName: user?.firstName,
        lastName: user?.lastName
    })

    const [hasError, setHasError] = useState({
        firstName: false,
        lastName: false
    })

    const validate = () => {
        const errorName = data.firstName === '';
        const errorLastName = data.lastName === '';
        setHasError({firstName: errorName, lastName: errorLastName});
        return errorName || errorLastName;
    };

    const SendData = () => {
        console.log(!validate())
        if (!validate())
            console.log('отправил')
        else
            console.log('нет')
    }

    return(
        <div className={style.form}>
            <Gapped gap={10} vertical className={style.gapped}>
                {/*<label className={style.label}>*/}
                {/*    <p className={style.name}>Логин</p>*/}
                {/*    <Input className={style.input} type={'text'} placeholder={'Введите название группы'}/>*/}
                {/*</label>*/}
                <label className={style.label}>
                    <p className={style.name}>Имя</p>
                    <Input className={style.input} type={'text'} placeholder={'Введите имя'} value={data.firstName}
                           onValueChange={value => setData({...data, firstName: value})}
                           error={hasError.firstName}/>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Фамилия</p>
                    <Input className={style.input} type={'text'} placeholder={'Введите Фамилию'} value={data.lastName}
                           onValueChange={value => setData({...data, lastName: value})}
                           error={hasError.lastName}/>
                </label>
                <div className={style.button}>
                    <Button use={'primary'} className={style.input} style={{borderRadius: '10px', overflow: 'hidden'}}
                            size='medium' onClick={SendData}>Сохранить изменения</Button>
                </div>
            </Gapped>
        </div>
    )
}
