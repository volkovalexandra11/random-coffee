import { FC } from 'react';
import { Modal } from '@skbkontur/react-ui';
import style from './tg-modal.module.scss'

type Props = {
    text: string;
    opened: boolean;
    close: () => void;
}

export const TGModal: FC<Props> = ({ text, opened, close }) => {
    return (<>
            {opened && <Modal width={'120%'} onClose={close}>
                <Modal.Header/>
                <Modal.Body>
                </Modal.Body>
                <Modal.Footer>
                    <div className={style.wrapper}>
                    {text}
                        <br/>
                    <a href='https://t.me/coffee_bot' target="_blank" className={style.link}>t.me/coffee_bot</a>
                    </div>
                </Modal.Footer>
            </Modal>}
        </>
    )
}
