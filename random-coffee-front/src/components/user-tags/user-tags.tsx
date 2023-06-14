import {FC, useState} from "react";
import style from './user-tags.module.scss';

type Props = {
    name: string;
}

const colors = ['#1F87EF', '#F8AA63'];

export const UserTags: FC<Props> = (props) => {
    const [color] = useState(Math.floor(Math.random() * colors.length));
    return (
        <div className={style.holder}>
            <div className={style.circle} style={{ background: colors[color] }}/>
            <span className={style.name}>{props.name}</span>
        </div>
    )
}
