import { FC } from 'react';
import { TGroup } from '../types/group';
import { GroupTable } from '../components/group-table/group-table';
import { GoogleOAuthProvider } from '@react-oauth/google';

type Props = {
	groupList: TGroup[];
}

export const GroupPage: FC<Props> = ({ groupList }) => {
	return (
		<GroupTable groups={groupList}/>
	);
};
