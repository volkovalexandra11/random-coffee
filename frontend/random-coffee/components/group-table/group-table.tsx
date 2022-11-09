import { FC } from 'react';
import { TGroup } from '../../types/group';
import { GroupTableRow } from '../group-table-row/group-table-row';

type Props = {
	groups: TGroup[];
}

export const GroupTable: FC<Props> = ({ groups }) => {
	return (
		<>
			{groups.map((group) =>
				<div key={group.id}>
					<GroupTableRow group={group}/>
				</div>)}
		</>
	);
}
