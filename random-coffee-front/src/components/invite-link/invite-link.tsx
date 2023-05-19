import { FC, useState } from 'react';
import { Button, Input, ThemeContext, ThemeFactory } from '@skbkontur/react-ui';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import { Copy } from '@skbkontur/react-icons';
import styles from './invite-link.module.scss';

type Props = {
  link: string;
};
export const InviteLink: FC<Props> = ({ link }) => {
  const theme = ThemeFactory.create({ inputBorderRadiusSmall: '10px' });
  const [isCopied, setIsCopied] = useState(false);
  const handleClick = () => {
    setIsCopied(true);
    setTimeout(() => setIsCopied(false), 2000);
  };

  return (
    <div className={styles.wrapper}>
      <ThemeContext.Provider value={theme}>
        <Input className={styles.input} value={link} />
      </ThemeContext.Provider>
      <CopyToClipboard text={link} onCopy={handleClick}>
        <Button
          className={styles.button}
          width={'47px'}
          use={isCopied ? 'success' : 'primary'}>
          <Copy />
        </Button>
      </CopyToClipboard>
    </div>
  );
};
