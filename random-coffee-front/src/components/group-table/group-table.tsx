import { FC, useState } from 'react';
import { TGroupShort } from '../../types/group';
import { GroupTableRow } from '../group-table-row/group-table-row';
import style from './group-table.module.scss'
import { Button } from '@skbkontur/react-ui';
import {Link, useNavigate} from "react-router-dom";

type Props = {
	groups: TGroupShort[];
}

export const GroupTable: FC<Props> = ({groups}) => {
	const navigate = useNavigate();

	return (
		<div className={style.background}>
			<div className={style.header}>
				<span className={style.name}>Группы</span>
				<Button className={style.button} use='primary' onClick={() => navigate('/create')}>+ Создать группу</Button>
			</div>
			<div className={style.wrapper}>
				{groups.map((group) =>
					<Link key={group.groupId} to={`/group/${group.groupId}`}>
						<GroupTableRow group={group}/>
					</Link>)}
			</div>
		</div>
	);
}
