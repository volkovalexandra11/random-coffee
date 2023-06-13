import {FC, useState} from 'react';
import style from './tag-for-pick.module.scss';

type Props = {
  name: string;
  onTagPick: (tagName: string) => void;
  isSelected: boolean;
};

const colors = ['#1F87EF', '#F8AA63'];
export const TagForPick: FC<Props> = (props) => {
  const [color] = useState(Math.floor(Math.random() * colors.length));
  const handleCheckboxChange = () => {
    props.onTagPick(props.name);
  };
  return (
    <div className={style.tag}>
      <label className={style.checkboxContainer}>
        <input
          type="checkbox"
          className={style.input}
          name="tags"
          checked={props.isSelected}
          onChange={handleCheckboxChange}
        />
        <span
          className={style.checkmark}
          style={{ background: colors[color] }}
        />
        <span className={style.name}>{props.name}</span>
      </label>
    </div>
  );
};
