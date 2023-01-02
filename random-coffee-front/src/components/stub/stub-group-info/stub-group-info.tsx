import style from '../../group-info/group-info.module.scss';
import { Button } from '@skbkontur/react-ui';

export const StubGroupInfo = () => {
	return (
		<div className={style.background}>
			<div className={style.header}>
				<span className={style.name}>Группа</span>
			</div>
			<div className={style.wrapper}>
				<div className={style.information}>
					<div className={style.about}>
						<div className={style.avatar}/>
						<div>
							<div className={style.description}/>
						</div>
					</div>
					<Button use='primary' className={style.button}>Выйти из группы</Button>
				</div>
				<div className={style.users}>
					<h2>Участники</h2>
				</div>
			</div>
		</div>);
};
