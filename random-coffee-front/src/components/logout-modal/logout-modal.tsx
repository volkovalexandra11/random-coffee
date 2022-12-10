import {FC} from "react";
import {Button, Modal} from "@skbkontur/react-ui";
import style from './logout-modal.module.scss'


type Props = {
    opened: boolean;
    close: () => void;
}


export const ExitModal: FC<Props> = ({opened, close}) => {
    return (<>
            {opened && <Modal width={"120%"} onClose={close}>
                <Modal.Header>Хотите выйти?</Modal.Header>
                <Modal.Body>
                </Modal.Body>
                <Modal.Footer>
                    <div className={style.modalButtons}>
                        <Button className={style.button} use="danger" onClick={() => {
                            alert('Logout');
                            close()
                        }}>Да</Button>
                        <Button className={style.button} onClick={close}>Отменить</Button>
                    </div>
                </Modal.Footer>
            </Modal>}
        </>
    )
}