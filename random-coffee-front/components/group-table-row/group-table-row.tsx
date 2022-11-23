import { FC } from 'react';
import { TGroup } from '../../types/group';
import style from './group-table-row.module.scss';
import { ImageWithPlaceholder } from '../image-with-placeholder/image-with-placeholder';

type Props = {
	group: TGroup;
}

export const GroupTableRow: FC<Props> = ({ group }) => {
	return (
		<section className={style.wrapper}>
			<div>
				<ImageWithPlaceholder showPlaceholder={group.picturePath === undefined} picturePath={group.picturePath}/>
			</div>
			<div className={style.GroupName}>{group.name}</div>
			<div>{group.users.length}</div>
		</section>
	);
}
