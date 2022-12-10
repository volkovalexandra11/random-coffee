import {FC} from 'react';
import style from './group-create-form.module.scss';
import {Button, Dropdown, Gapped, Input, MenuItem, Radio, RadioGroup, Textarea} from '@skbkontur/react-ui';
import {Add} from '@skbkontur/react-icons';
import {useNavigate} from "react-router-dom";

export const GroupCreateForm: FC = () => {

    const navigate = useNavigate();
    return(
        <>
            <div className={style.test}>
                <div className={style.close} onClick={() => navigate('/')}><Add/></div>
            </div>
            <Gapped gap={10} vertical className={style.gapped}>
                <label className={style.label}>
                    <p className={style.name}>Название</p>
                    <Input className={style.input} type={'text'} placeholder={"Введите название группы"}/>
                </label>
                <label className={style.label} id={style.description}>
                    <p className={style.name}>Описание</p>
                    <Textarea className={style.input} width={'80%'} placeholder={"Введите описание для группы "}/>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Добавить</p>
                    <Input className={style.input} type={'text'} placeholder={"Начните вводить логин пользователя"}/>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Повтор</p>
                    <Dropdown className={style.input} caption={"Выберите частоту встреч"}>
                        <MenuItem>
                            Раз в неделю
                        </MenuItem>
                        <MenuItem>
                            Раз в месяц
                        </MenuItem>
                        <MenuItem>
                            Настраивать вручную
                        </MenuItem>
                    </Dropdown>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Первая<br/> встреча</p>
                    <Input className={style.input} type={'text'}/>
                </label>
                <div className={style.label}>
                    <p className={style.name}>Тип</p>
                    <RadioGroup className={style.radioGroup} width={'80%'}>
                        <Radio value={1}>Приватная</Radio>
                        <Radio value={2}>Публичная</Radio>
                    </RadioGroup>
                </div>
                <Button use={"primary"} className={style.buttons}>Создать группу</Button>
            </Gapped>
        </>
    )
}
