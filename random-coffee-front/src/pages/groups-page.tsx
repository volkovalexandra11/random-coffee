import { FC } from 'react';
import { Loader } from '@skbkontur/react-ui';
import {  useAppSelector } from '../hooks';
import { StubGroupTable } from '../components/stub/stub-group-table/stub-group-table';
import { GroupTable } from '../components/group-table/group-table';
import { AuthStatus } from '../types/authStatus';
import { EmptyGroupList } from '../components/empty-group-list/empty-group-list';

export const GroupsPage: FC = () => {
    const { groups } = useAppSelector((state) => state);
    const { isGroupsLoaded } = useAppSelector((state) => state);
    const { authStatus } = useAppSelector((state) => state);

    return (
        <Loader active={!isGroupsLoaded}>
            {isGroupsLoaded && (authStatus === AuthStatus.Logged) ? groups.length !== 0 ?
                <GroupTable groups={groups}/> : <EmptyGroupList/> : <StubGroupTable/>}
        </Loader>
    );
};
