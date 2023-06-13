import { FC, useState } from 'react';
import style from './pick-tags.module.scss';
import { Button } from '@skbkontur/react-ui';
import { tags } from '../../mocks/tags';
import { TagForPick } from '../tag-for-pick/tag-for-pick';

export const PickTags: FC = () => {
  const [pickedTags, setPickedTags] = useState<string[]>([]);

  const handleTagPick = (tagName: string) => {
    if (pickedTags.includes(tagName)) {
      setPickedTags(pickedTags.filter((tag) => tag !== tagName));
    } else {
      setPickedTags([...pickedTags, tagName]);
    }
  };

  const handleSaveButtonClick = () => {
    console.log(pickedTags);
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
