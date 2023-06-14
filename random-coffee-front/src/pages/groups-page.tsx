import { FC, useEffect, useState } from 'react';
import { Loader } from '@skbkontur/react-ui';
import { useAppDispatch, useAppSelector } from '../hooks';
import { StubGroupTable } from '../components/stub/stub-group-table/stub-group-table';
import { GroupTable } from '../components/new-group-table/group-table';
import { AuthStatus } from '../types/authStatus';
import { EmptyGroupList } from '../components/empty-group-list/empty-group-list';
import { fetchGroupsAction, getManagedGroup } from '../store/api-action';

export const GroupsPage: FC = () => {
  const { groups, user } = useAppSelector((state) => state);
  const { isGroupsLoaded } = useAppSelector((state) => state);
  const { authStatus } = useAppSelector((state) => state);
  const [isAllGroup, setIsAllGroup] = useState(true);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (authStatus === AuthStatus.Logged) {
      if (isAllGroup) dispatch(fetchGroupsAction(user?.userId));
      else dispatch(getManagedGroup(user?.userId));
    }
  }, [authStatus, dispatch, user, isAllGroup]);

  return (
    <Loader active={!isGroupsLoaded}>
      {isGroupsLoaded && authStatus === AuthStatus.Logged ? (
        groups.length !== 0 ? (
          <GroupTable
            groups={groups}
            setFilter={setIsAllGroup}
            isAllGroup={isAllGroup}
          />
        ) : (
          <EmptyGroupList />
        )
      ) : (
        <StubGroupTable />
      )}
    </Loader>
  );
};
