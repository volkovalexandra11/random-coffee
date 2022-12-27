import { FC, useState } from 'react';
import style from './group-create-form.module.scss';
import { Button, Dropdown, Gapped, Input, MenuItem, Radio, RadioGroup, Textarea, DatePicker } from '@skbkontur/react-ui';

export const GroupCreateForm: FC = () => {
    const [repeatMeetings, setRepeatMeetings] = useState("Выберите частоту встреч");
    const [meetingDate, setMeetingDate] = useState<string>();
    const [hasError, setHasHasError] = useState(false);
    const [hasTooltip, setHasHasTooltip] = useState(false);

    const today = new Date()
    const minDate = String(today.getDate()).padStart(2, '0')
        + '.' + String(today.getMonth() + 1).padStart(2, '0')
        + '.' + today.getFullYear();
    const maxDate = '02.05.2023';

    const unvalidate = () => {
        setHasHasError(false);
        setHasHasTooltip(false);
    };

    const validate = () => {
        const errorNew = !!meetingDate && !DatePicker.validate(meetingDate, { minDate: minDate, maxDate: maxDate });
        setHasHasError(errorNew);
        setHasHasTooltip(errorNew);
    };
    const removeTooltip = () => setHasHasTooltip(false);
    return(
        <div className={style.form}>
            <Gapped gap={10} vertical className={style.gapped}>
                <label className={style.label}>
                    <p className={style.name}>Название</p>
                    <Input className={style.input} type={'text'} placeholder={"Введите название группы"}/>
                </label>
                <label className={style.label} id={style.description}>
                    <p className={style.name}>Описание</p>
                    <Textarea className={style.input} width={'75%'} placeholder={"Введите описание для группы "}/>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Добавить</p>
                    <Input className={style.input} type={'text'} placeholder={"Начните вводить логин пользователя"}/>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Повтор</p>
                    <Dropdown className={style.input} caption={repeatMeetings}>
                        <MenuItem onClick={() => setRepeatMeetings("Раз в неделю")}>
                            Раз в неделю
                        </MenuItem>
                        <MenuItem onClick={() => setRepeatMeetings("Раз в месяц")}>
                            Раз в месяц
                        </MenuItem>
                        <MenuItem onClick={() => setRepeatMeetings("Настраивать вручную")}>
                            Настраивать вручную
                        </MenuItem>
                    </Dropdown>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Первая<br/> встреча</p>
                    <div className={style.input}>
                        <DatePicker error={hasError}
                                    width={'auto'}
                                    value={meetingDate}
                                    onValueChange={(date) => setMeetingDate(date)}
                                    onFocus={unvalidate}
                                    onBlur={validate}
                                    minDate={minDate}
                                    maxDate={maxDate}
                                    enableTodayLink/>
                    </div>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Тип</p>
                    <div className={style.input}>
                    <RadioGroup  width={'50%'}>
                        <Gapped gap={15}>
                        <Radio value={1}>Приватная</Radio>
                        <Radio value={2}>Публичная</Radio>
                        </Gapped>
                    </RadioGroup>
                    </div>
                </label>
                <div className={style.button}>
                    <Button use={"primary"} className={style.input} style={{borderRadius: '10px', overflow: 'hidden'}}
                            size="medium">Создать группу</Button>
                </div>
            </Gapped>
        </div>
    )
}
