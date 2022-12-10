import {FC, useState} from 'react';
import { TGroup } from '../../types/group';
import { GroupTableRow } from '../group-table-row/group-table-row';
import {NavBar} from "../nav-bar/nav-bar";
import style from './group-table.module.scss'
import {Button} from '@skbkontur/react-ui';
// import SearchIcon from '@skbkontur/react-icons/search';
import {SearchGroup} from "../search/search";
import { useNavigate } from "react-router-dom";

type Props = {
	groups: TGroup[];
}

export const GroupTable: FC<Props> = (props) => {
	const [groups, SetGroups] = useState(props.groups)
	const navigate = useNavigate();

	return (
		<div className={style.background}>
			<div className={style.header}>
				<span className={style.name}>Группы</span>
				<Button className={style.button} use='primary' onClick={() => navigate('/create')}>+ Создать группу</Button>
			</div>
			<div className={style.wrapper}>
				<SearchGroup groups={groups} setGroups={SetGroups}/>
				{groups.map((group) =>
					<div key={group.id}>
						<GroupTableRow group={group}/>
					</div>)}
			</div>
		</div>
	);
}