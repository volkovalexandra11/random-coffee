import { FC } from 'react';
import style from './stub-group-table-row.module.scss'
import { StubImage } from '../stub-image/stub-image';

export const StubGroupTableRow : FC = () => {
	return (
		<section className={style.wrapper}>
			<div className={style.NameAvatar}>
				<StubImage />
				<div className={style.GroupName}/>
			</div>
		</section>
	);
}
