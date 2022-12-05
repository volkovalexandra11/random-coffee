import {FC, useState} from 'react';
import { TGroup } from '../../types/group';
import { GroupTableRow } from '../group-table-row/group-table-row';
import {NavBar} from "../nav-bar/nav-bar";
import style from './group-table.module.scss'
import {Button} from '@skbkontur/react-ui';
// import SearchIcon from '@skbkontur/react-icons/search';
import {Search} from "../search/search";

type Props = {
	groups: TGroup[];
}

export const GroupTable: FC<Props> = (props) => {
	const [groups, SetGroups] = useState(props.groups)

	return (
		<div className={style.background}>
			<NavBar id={1} avatarPath={''} firstName={"Самсонов"} lastName={"Иван"}/>
			<div className={style.header}>
				<span className={style.name}>Группы</span>
				<Button className={style.button} use='primary'>+ Создать группу</Button>
			</div>
			<div className={style.wrapper}>
				<Search groups={groups} setGroups={SetGroups}/>
				{groups.map((group) =>
					<div key={group.id}>
						<GroupTableRow group={group}/>
					</div>)}
			</div>
		</div>
	);
}