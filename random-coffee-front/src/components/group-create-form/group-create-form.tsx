import {FC, useState} from 'react';
import style from './group-create-form.module.scss';
import {Button, Dropdown, Gapped, Input, MenuItem, Radio, RadioGroup, Textarea, DatePicker} from '@skbkontur/react-ui';

export const GroupCreateForm: FC = () => {
    const [data, setData] = useState({
        groupName: "",
        description: "",
        users: "",
        groupType: "",
        repeatMeetings: "Выберите частоту встреч",
        meetingDate: ""
    })

    const [hasError, setHasError] = useState({
        name: false,
        repeat: false,
        date: false,
        type: false
    })

    const today = new Date()
    const minDate = String(today.getDate()).padStart(2, '0')
        + '.' + String(today.getMonth() + 1).padStart(2, '0')
        + '.' + today.getFullYear();
    const maxDate = '02.05.2023';

    const validate = () => {
        const errorData = !!data.meetingDate && !DatePicker.validate(data.meetingDate, {
            minDate: minDate,
            maxDate: maxDate
        }) || data.meetingDate === "";
        const errorName = data.groupName === "";
        const errorRepeat = data.repeatMeetings === "Выберите частоту встреч";
        const errorType = data.groupType === "";
        setHasError({name: errorName, repeat: errorRepeat, type: errorType, date: errorData});
        return errorData || errorRepeat || errorType || errorName;
    };

    const choseRepeatValue = (value: string) => {
        setData({...data, repeatMeetings: value});
        setHasError({...hasError, repeat: false});
    }

    const SendData = () => {
        if (!validate())
            console.log("отправил")
        else
            console.log("нет")
    }
    return (
        <div className={style.form}>
            <Gapped gap={10} vertical className={style.gapped}>
                <label className={style.label}>
                    <p className={style.name}>Название</p>
                    <Input className={style.input} type={'text'} placeholder={"Введите название группы"}
                           onValueChange={value => setData({...data, groupName: value})} error={hasError.name}
                           onFocus={() => setHasError({...hasError, name: false})}/>
                </label>
                <label className={style.label} id={style.description}>
                    <p className={style.name}>Описание</p>
                    <Textarea className={style.input} width={'75%'} placeholder={"Введите описание для группы "}
                              onValueChange={value => setData({...data, description: value})}/>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Добавить</p>
                    <Input className={style.input} type={'text'} placeholder={"Начните вводить логин пользователя"}
                           onValueChange={value => setData({...data, users: value})}/>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Повтор</p>
                    <Dropdown className={style.input} caption={data.repeatMeetings} error={hasError.repeat}>
                        <MenuItem onClick={() => choseRepeatValue("Раз в неделю")}>
                            Раз в неделю
                        </MenuItem>
                        <MenuItem onClick={() => choseRepeatValue("Раз в месяц")}>
                            Раз в месяц
                        </MenuItem>
                        <MenuItem onClick={() => choseRepeatValue("Настраивать вручную")}>
                            Настраивать вручную
                        </MenuItem>
                    </Dropdown>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Первая<br/> встреча</p>
                    <div className={style.input}>
                        <DatePicker error={hasError.date}
                                    width={'auto'}
                                    value={data.meetingDate}
                                    onValueChange={(date) => setData({...data, meetingDate: date})}
                                    onFocus={() => setHasError({...hasError, date: false})}
                                    minDate={minDate}
                                    maxDate={maxDate}
                                    enableTodayLink/>
                    </div>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Тип</p>
                    <div className={style.input}>
                        <RadioGroup width={'50%'} onValueChange={value => {
                            setData({...data, groupType: String(value)});
                            setHasError({...hasError, type: false})
                        }} error={hasError.type}>
                            <Gapped gap={15}>
                                <Radio value={"private"}>Приватная</Radio>
                                <Radio value={"public"}>Публичная</Radio>
                            </Gapped>
                        </RadioGroup>
                    </div>
                </label>
                <div className={style.button}>
                    <Button use={"primary"} className={style.input} style={{borderRadius: '10px', overflow: 'hidden'}}
                            size="medium" onClick={SendData}>Создать группу</Button>
                </div>
            </Gapped>
        </div>
    )
}
