import { FC, useState } from 'react';
import style from './user-form.module.scss';
import { Button, Gapped, Input } from '@skbkontur/react-ui';
import {useAppDispatch, useAppSelector} from '../../hooks';
import {TUpdateUserDto} from "../../types/user";
import {updateUserInfo} from "../../store/api-action";

export const UserForm: FC = () => {
	const { user } = useAppSelector((state) => state);
	const dispatch = useAppDispatch();

	const [data, setData] = useState({
		firstName: user?.firstName,
		lastName: user?.lastName
	});

	const [hasError, setHasError] = useState({
		firstName: false,
		lastName: false
	});

	const isHasError = () => {
		const errorName = data.firstName === '';
		const errorLastName = data.lastName === '';
		setHasError({ firstName: errorName, lastName: errorLastName });
		return errorName || errorLastName;
	};

	const SendData = async () => {
		if (!isHasError()) {
			const dto: TUpdateUserDto = {
				// @ts-ignore
				email: user.email,
				// @ts-ignore
				firstName: data.firstName,
				// @ts-ignore
				lastName: data.lastName,
				// @ts-ignore
				profilePictureUrl: user.profilePictureUrl,
			}
			// @ts-ignore
			await dispatch(updateUserInfo({userDto: dto, userId: user.userId}));
		}
	}


	return (
		<div className={style.form}>
			<Gapped gap={10} vertical className={style.gapped}>
				<label className={style.label}>
					<p className={style.name}>Имя</p>
					<Input className={style.input} type={'text'} placeholder={'Введите имя'} value={data.firstName}
								 onValueChange={value => setData({ ...data, firstName: value })}
								 error={hasError.firstName}
					/>
				</label>
				<label className={style.label}>
					<p className={style.name}>Фамилия</p>
					<Input className={style.input} type={'text'} placeholder={'Введите Фамилию'} value={data.lastName}
								 onValueChange={value => setData({ ...data, lastName: value })}
								 error={hasError.lastName}
					/>
				</label>
				<div className={style.button}>
					<Button use={'primary'} className={style.input} style={{ borderRadius: '10px', overflow: 'hidden' }}
									size='medium' onClick={SendData}
					>Сохранить изменения</Button>
				</div>
			</Gapped>
		</div>
	)
}
