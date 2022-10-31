import { FC } from 'react';
import { TGroup } from '../../types/group';
import styles from './group-table-row.less'

type Props = {
	group: TGroup;
}

export const GroupTableRow: FC<Props> = ({ group }) => {
	return (
		<section className={styles.wrapper}>
			<div>
				<img src={group.picturePath} alt='picture'/>
			</div>
			<div className={styles.GroupName}>{group.name}</div>
			<div>{group.users.length}</div>
		</section>
	);
}
