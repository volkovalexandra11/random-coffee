import { FC, useEffect } from 'react';
import { Loader } from '@skbkontur/react-ui';
import { useAppDispatch, useAppSelector } from '../hooks';
import { StubGroupTable } from '../components/stub/stub-group-table/stub-group-table';
import { GroupTable } from '../components/new-group-table/group-table';
import { AuthStatus } from '../types/authStatus';
import { EmptyGroupList } from '../components/empty-group-list/empty-group-list';
import { fetchGroupsAction } from '../store/api-action';

export const GroupsPage: FC = () => {
    const { groups, user } = useAppSelector((state) => state);
    const { isGroupsLoaded } = useAppSelector((state) => state);
    const { authStatus } = useAppSelector((state) => state);

    const dispatch = useAppDispatch();

    useEffect(() => {
        if (authStatus === AuthStatus.Logged) {
            dispatch(fetchGroupsAction(user?.userId));
        }
    }, [authStatus, dispatch, user])

    console.log(groups);

    return (
        <Loader active={!isGroupsLoaded}>
            {isGroupsLoaded && (authStatus === AuthStatus.Logged) ? groups.length !== 0 ?
                <GroupTable groups={groups}/> : <EmptyGroupList/> : <StubGroupTable/>}
        </Loader>
    );
};
