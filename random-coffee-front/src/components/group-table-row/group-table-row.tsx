import { FC } from 'react';
import { TGroupShort } from '../../types/group';
import style from './group-table-row.module.scss';
import { ImageWithPlaceholder } from '../image-with-placeholder/image-with-placeholder';
import { formatDate } from '../../helpers/dateHelper';

type Props = {
	group: TGroupShort;
}

export const GroupTableRow: FC<Props> = ({ group }) => {
	return (
		<section className={style.wrapper}>
			<div className={style.NameAvatar}>
				<ImageWithPlaceholder showPlaceholder={group.groupPictureUrl === undefined} picturePath={group.groupPictureUrl}/>
				<div className={style.GroupName}>{group.name}</div>
			</div>
			<div className={style.information}>
				<div className={style.params}>{group.participantsCount}</div>
				<div className={style.params}>{formatDate(group.nextRoundDate)}</div>
			</div>
		</section>
	);
}
