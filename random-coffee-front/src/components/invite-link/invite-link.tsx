import {FC, useState} from 'react';
import {Button, Input} from "@skbkontur/react-ui";
import {CopyToClipboard} from 'react-copy-to-clipboard';
import { Copy } from "@skbkontur/react-icons";
import styles from "./invite-link.module.scss"
import {makeRoundAction} from "../../store/api-action";

type Props = {
    linkToCopy: string;
}
export const InviteLink: FC<Props> = ({linkToCopy}) => {
    const [isCopied, setIsCopied] = useState(false);

    const handleClick = () => {
        setIsCopied(true)
        setTimeout(() => setIsCopied(false), 2000);
    }

    return (
        <div className={styles.wrapper}>
            <Input value={"Инвайт ссылка"}/>
            <CopyToClipboard text={linkToCopy} onCopy={handleClick}>
                <Button use={isCopied? 'success' : 'primary'}>
                    <Copy/>
                </Button>
            </CopyToClipboard>
        </div>
    );
}