import { FC, useState } from 'react';
import {
    Button,
    DatePicker,
    Dropdown,
    Gapped,
    Input,
    MenuItem,
    Radio,
    RadioGroup,
    Textarea
} from '@skbkontur/react-ui';
import { useNavigate } from 'react-router-dom';
import style from './edit-group.module.scss';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { getGroupDto } from '../../helpers/groupHelper';
import { postGroupAction } from '../../store/api-action';

export const EditGroupForm: FC = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const { currentGroup } = useAppSelector((state) => state);

    const [data, setData] = useState({
        // @ts-ignore
        groupName: currentGroup.name,
        description: '',
        repeatMeetings: '',
        meetingDate: ''
    });

    const [hasError, setHasError] = useState({
        name: false,
        repeat: false,
        date: false
    });

    const today = new Date();
    const minDate = String(today.getDate()).padStart(2, '0')
        + '.' + String(today.getMonth() + 1).padStart(2, '0')
        + '.' + today.getFullYear();
    const maxDate = '02.05.2023';

    const isHasError = () => {
        const errorData = !!data.meetingDate && !DatePicker.validate(data.meetingDate, {
            minDate: minDate,
            maxDate: maxDate
        }) || data.meetingDate === '';
        const errorName = data.groupName === '';
        const errorRepeat = data.repeatMeetings === 'Выберите частоту встреч';
        setHasError({ name: errorName, repeat: errorRepeat, date: errorData });
        return errorData || errorRepeat || errorName;
    };

    const choseRepeatValue = (value: string) => {
        setData({ ...data, repeatMeetings: value });
        setHasError({ ...hasError, repeat: false });
    }

    const sendData = () => {
        if (!isHasError()) {
            // const groupDto = getGroupDto(data);
            // // @ts-ignore
            // dispatch(postGroupAction(groupDto));
            // // @ts-ignore
            // navigate(`/group/${currentGroup?.groupId}`);
        }
    };

    return (
        <div className={style.form}>
            <Gapped gap={10} vertical className={style.gapped}>
                <label className={style.label}>
                    <p className={style.name}>Название</p>
                    <Input className={style.input} type={'text'} placeholder={'Введите название группы'}
                           onValueChange={value => setData({ ...data, groupName: value })} error={hasError.name}
                           onFocus={() => setHasError({ ...hasError, name: false })}
                    />
                </label>
                <label className={style.label} id={style.description}>
                    <p className={style.name}>Описание</p>
                    <Textarea className={style.input} width={'75%'} placeholder={'Введите описание для группы '}
                              onValueChange={value => setData({ ...data, description: value })}
                    />
                </label>
                <label className={style.label}>
                    <p className={style.name}>Повтор</p>
                    <Dropdown className={style.input} caption={data.repeatMeetings} error={hasError.repeat}>
                        <MenuItem onClick={() => choseRepeatValue('Раз в неделю')}>
                            Раз в неделю
                        </MenuItem>
                        <MenuItem onClick={() => choseRepeatValue('Раз в месяц')}>
                            Раз в месяц
                        </MenuItem>
                        <MenuItem onClick={() => choseRepeatValue('Настраивать вручную')}>
                            Настраивать вручную
                        </MenuItem>
                    </Dropdown>
                </label>
                <label className={style.label}>
                    <p className={style.name}>Следующая<br/> встреча</p>
                    <div className={style.input}>
                        <DatePicker error={hasError.date}
                                    width={'auto'}
                                    value={data.meetingDate}
                                    onValueChange={(date) => setData({ ...data, meetingDate: date })}
                                    onFocus={() => setHasError({ ...hasError, date: false })}
                                    minDate={minDate}
                                    maxDate={maxDate}
                                    enableTodayLink
                        />
                    </div>
                </label>
                <div className={style.button}>
                    <Button use={'primary'} className={style.input} style={{ borderRadius: '10px', overflow: 'hidden' }}
                            size='medium' onClick={sendData}
                    >Сохранить изменения</Button>
                </div>
            </Gapped>
        </div>
    )
}
