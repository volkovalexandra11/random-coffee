import { FC, useState } from 'react';
import style from './pick-tags.module.scss';
import { Button } from '@skbkontur/react-ui';
import { tags } from '../../mocks/tags';
import { TagForPick } from '../tag-for-pick/tag-for-pick';
import {useDispatch} from "react-redux";
import {setTags} from "../../store/action";
import {useNavigate} from "react-router-dom";
import {useAppSelector} from "../../hooks";

export const PickTags: FC = () => {
  const { user } = useAppSelector((state) => state);
  // @ts-ignore
  const [pickedTags, setPickedTags] = useState<string[]>(user.tag);
  const dispatch = useDispatch();
  const navigator = useNavigate();
  const handleTagPick = (tagName: string) => {
    if (pickedTags.includes(tagName)) {
      setPickedTags(pickedTags.filter((tag) => tag !== tagName));
    } else {
      setPickedTags([...pickedTags, tagName]);
    }
  };

  const handleSaveButtonClick = () => {
    dispatch(setTags({ tags: pickedTags} ))
    navigator('/user');
  };

  return (
    <div className={style.wrapper}>
      <div className={style.form}>
        <section className={style.info}>
          <h1>
            Выберите что вам <br />
            интересно
          </h1>
          <Button use="pay" onClick={handleSaveButtonClick}>
            Сохранить
          </Button>
        </section>
        <section className={style.tags}>
          {tags.map((t) => (
            <TagForPick
              name={t}
              key={t}
              onTagPick={handleTagPick}
              isSelected={pickedTags.includes(t)}
            />
          ))}
        </section>
      </div>
    </div>
  );
};
