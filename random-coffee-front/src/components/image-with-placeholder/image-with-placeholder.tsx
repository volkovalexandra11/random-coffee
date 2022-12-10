import { FC } from 'react';
import style from './image-with-placeholder.module.scss';

type Props = {
	showPlaceholder: boolean;
	picturePath? : string;
}

export const ImageWithPlaceholder : FC<Props> = ({showPlaceholder, picturePath}) => {
	const getPlaceholder = () => {
		return (
			<div className={style.placeholder}></div>
		)
	}

	if (showPlaceholder){
		return getPlaceholder();
	} else {
		return (
			<img src={String("." + picturePath)} alt='picture'/>
		);
	}
};
