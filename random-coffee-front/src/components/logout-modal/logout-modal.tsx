import { FC } from 'react';
import { Button, Gapped, Modal } from '@skbkontur/react-ui';
import style from './logout-modal.module.scss'
import { useAppDispatch } from '../../hooks';
import { changeAuthStatus } from '../../store/action';
import { AuthStatus } from '../../types/authStatus';

type Props = {
    opened: boolean;
    close: () => void;
}

export const ExitModal: FC<Props> = ({ opened, close }) => {
    const dispatch = useAppDispatch();
    const handleClick = () => {
        dispatch(changeAuthStatus({ authStatus: AuthStatus.NotLogged }));
        close();
        window.location.href = '/logout';
    };

    return (<>
            {opened && <Modal width={'120%'} onClose={close}>
                <Modal.Header>Хотите выйти?</Modal.Header>
                <Modal.Body>
                </Modal.Body>
                <Modal.Footer>
                    <Gapped>
                        <Button className={style.button} use='danger' onClick={handleClick}>Да</Button>
                        <Button className={style.button} onClick={close}>Отменить</Button>
                    </Gapped>
                </Modal.Footer>
            </Modal>}
        </>
    )
}
