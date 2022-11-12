import { FC } from 'react';
import { TGroup } from '../../types/group';
import style from './group-table-row.module.scss';

type Props = {
	group: TGroup;
}

export const GroupTableRow: FC<Props> = ({ group }) => {
	return (
		<section className={style.wrapper}>
			<div>
				<img src={group.picturePath} alt='picture'/>
			</div>
			<div className={style.GroupName}>{group.name}</div>
			<div>{group.users.length}</div>
		</section>
	);
}
