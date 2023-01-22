import { FC } from "react";
import { Button, Gapped, Modal } from "@skbkontur/react-ui";
import style from './kick-user-modal.module.scss'
import {TUser} from "../../types/user";
import store from "../../store";
import {KickUserFromGroup} from "../../store/api-action";
import {useAppSelector} from "../../hooks";

type Props = {
    user: TUser;
    opened: boolean;
    close: () => void;
}

export const KickModal: FC<Props> = ({ user, opened, close }) => {
    const { currentGroup } = useAppSelector((state) => state);
    const handleClick = () => {
        // @ts-ignore
        store.dispatch(KickUserFromGroup({groupId: currentGroup.groupId, userId: user.userId}))
        close();
    };

    return (<>
            {opened && <Modal width={"120%"} onClose={close}>
                <Modal.Header/>
                <Modal.Body> Вы действительно хотите исключить из группы участника {user.firstName} {user.lastName}
                </Modal.Body>
                <Modal.Footer>
                    <Gapped>
                        <Button className={style.button} use="danger" onClick={handleClick}>Да</Button>
                        <Button className={style.button} onClick={close}>Отменить</Button>
                    </Gapped>
                </Modal.Footer>
            </Modal>}
        </>
    )
}
