import { FC } from 'react';
import { TGroup } from '../../types/group';
import { GroupTableRow } from '../group-table-row/group-table-row';
import {NavBar} from "../nav-bar/nav-bar";

type Props = {
	groups: TGroup[];
}

export const GroupTable: FC<Props> = ({ groups }) => {
	return (
		<>
			<NavBar id={1} avatarPath={''} firstName={"Самсонов"} lastName={"Иван"}/>
			{groups.map((group) =>
				<div key={group.id}>
					<GroupTableRow group={group}/>
				</div>)}
		</>
	);
}
