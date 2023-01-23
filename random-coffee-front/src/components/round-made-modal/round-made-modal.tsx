import { Button, Modal } from '@skbkontur/react-ui';
import { FC } from 'react';

type Props = {
	onClose: () => void;
}

export const RoundMadeModal : FC<Props> = ({onClose} : Props) => {
	return (
		<Modal onClose={onClose}>
			<Modal.Header>
				Рассылка сделана
			</Modal.Header>
			<Modal.Footer>
				<Button use={'primary'} onClick={onClose}>OK</Button>
			</Modal.Footer>
		</Modal>
	);
}
