import { FC, useState } from 'react';
import { Button, Input, ThemeContext, ThemeFactory } from '@skbkontur/react-ui';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import { Copy } from '@skbkontur/react-icons';
import styles from './invite-link.module.scss';

type Props = {
  link: string;
};
export const InviteLink: FC<Props> = ({ link }) => {
  const theme = ThemeFactory.create({ btnDefaultBorderColor: 'none', btnBorderRadiusSmall: '10px', btnDefaultHoverBorderColor: 'none',
      btnDefaultTextColor: 'rgba(31, 135, 239, 1)'});
  const [isCopied, setIsCopied] = useState(false);
  const handleClick = () => {
    setIsCopied(true);
    setTimeout(() => setIsCopied(false), 2000);
  };

  return (
    <CopyToClipboard text={link} onCopy={handleClick}>
      <ThemeContext.Provider value={theme}>
        <Button
          className={styles.button}
          use={isCopied ? 'success' : 'default'}>
          Скопировать ссылку-приглашение
        </Button>
      </ThemeContext.Provider>
    </CopyToClipboard>
  );
};
