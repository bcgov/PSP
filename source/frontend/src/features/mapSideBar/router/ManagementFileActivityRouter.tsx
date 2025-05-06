import React, { useEffect } from 'react';
import { matchPath, Switch, useHistory, useLocation, useRouteMatch } from 'react-router';

import { Claims } from '@/constants';
import { exists } from '@/utils';
import AppRoute from '@/utils/AppRoute';

import { ManagementActivityEditContainer } from '../management/tabs/activities/edit/ManagementActivityEditContainer';
import { ManagementActivityEditForm } from '../management/tabs/activities/edit/ManagementActivityEditForm';

interface IManagementFileActivityRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const ManagementFileActivityRouter: React.FunctionComponent<
  React.PropsWithChildren<IManagementFileActivityRouterProps>
> = React.memo(({ setShowActionBar }) => {
  const location = useLocation();
  const history = useHistory();
  const match = useRouteMatch();

  const matched = matchPath(location.pathname, {
    path: '/mapview/sidebar/management/*/activities/*',
    exact: true,
    strict: true,
  });

  useEffect(() => {
    if (exists(matched)) {
      setShowActionBar(true);
    } else {
      setShowActionBar(false);
    }
  }, [matched, setShowActionBar]);

  const onClose = () => {
    const parentPath = match?.url.substring(0, match?.url.lastIndexOf('/')) || '/';
    history.push(parentPath);
  };

  return (
    <Switch>
      <AppRoute
        path={`/mapview/sidebar/management/:managementFileId/activities/new`}
        customRender={({ match }) => (
          <ManagementActivityEditContainer
            managementFileId={Number(match.params.managementFileId)}
            onClose={onClose}
            View={ManagementActivityEditForm}
          />
        )}
        claim={Claims.PROPERTY_VIEW}
        exact
        key={'activity_new'}
        title={'Activity New'}
      />
    </Switch>
  );
});

export default ManagementFileActivityRouter;
