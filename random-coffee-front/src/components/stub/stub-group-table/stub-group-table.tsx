import { Button } from '@skbkontur/react-ui';
import style from '../../group-table/group-table.module.scss';
import { StubGroupTableRow } from '../stub-group-table-row/stub-group-table-row';

export const StubGroupTable = () => {
	return (
		<div className={style.background}>
			<div className={style.header}>
				<span className={style.name}>Группы</span>
				<Button className={style.button} use='primary' disabled>+ Создать группу</Button>
			</div>
			<div className={style.wrapper}>
				{[1, 2, 3].map((_) => <StubGroupTableRow/>)}
			</div>
		</div>
	);
}
