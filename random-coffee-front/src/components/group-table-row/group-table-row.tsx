import {FC, useCallback} from 'react';
import { TGroupShort } from '../../types/group';
import style from './group-table-row.module.scss';
import { ImageWithPlaceholder } from '../image-with-placeholder/image-with-placeholder';
import { formatDate } from '../../helpers/dateHelper';
import {forGroups} from "../../mocks/tags";

type Props = {
	group: TGroupShort;
}
export const GroupTableRow: FC<Props> = ({ group }) => {
	const tagsToString = useCallback((tag: string[] | null) => {
		if (tag === null)
			return '';
		return tag.join(', ');
	}, [group.tag]);
	return (
		<section className={style.wrapper}>
			<div className={style.NameAvatar}>
				<ImageWithPlaceholder showPlaceholder={group.groupPictureUrl === undefined} picturePath={group.groupPictureUrl}/>
				<div className={style.name}>
					<span className={style.GroupName}>{group.name}</span>
					<span className={style.tags}>{tagsToString(group.tag)}</span>
				</div>
			</div>
			<div className={style.information}>
				<div className={style.params}></div>
			</div>
		</section>
	);
}
