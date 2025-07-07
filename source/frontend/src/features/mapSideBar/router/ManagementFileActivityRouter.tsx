import React, { useEffect } from 'react';
import { matchPath, Switch, useLocation } from 'react-router';

import { Claims } from '@/constants';
import { exists } from '@/utils';
import AppRoute from '@/utils/AppRoute';

import { FileActivityDetailContainer } from '../management/tabs/activities/detail/FileActivityDetailContainer';
import { FileActivityDetailView } from '../management/tabs/activities/detail/FileActivityDetailView';
import { ManagementActivityEditContainer } from '../management/tabs/activities/edit/ManagementActivityEditContainer';
import { ManagementActivityEditForm } from '../management/tabs/activities/edit/ManagementActivityEditForm';
import usePathGenerator from '../shared/sidebarPathGenerator';

interface IManagementFileActivityRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const ManagementFileActivityRouter: React.FunctionComponent<
  React.PropsWithChildren<IManagementFileActivityRouterProps>
> = React.memo(({ setShowActionBar }) => {
  const location = useLocation();
  const pathGenerator = usePathGenerator();

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

  return (
    <Switch>
      <AppRoute
        path={`/mapview/sidebar/management/:managementFileId/activities/new`}
        customRender={({ match }) => (
          <ManagementActivityEditContainer
            managementFileId={Number(match.params.managementFileId)}
            onClose={() => {
              pathGenerator.showDetails(
                'management',
                match.params.managementFileId,
                'activities',
                true,
              );
            }}
            View={ManagementActivityEditForm}
          />
        )}
        claim={[Claims.PROPERTY_VIEW, Claims.MANAGEMENT_EDIT]}
        exact
        key={'activity_add'}
        title={'Add Activity'}
      />
      <AppRoute
        path={`/mapview/sidebar/management/:managementFileId/activities/:activityId/edit`}
        customRender={({ match }) => (
          <ManagementActivityEditContainer
            managementFileId={Number(match.params.managementFileId)}
            activityId={Number(match.params.activityId)}
            onClose={() => {
              pathGenerator.showDetails(
                'management',
                match.params.managementFileId,
                'activities',
                true,
              );
            }}
            View={ManagementActivityEditForm}
          />
        )}
        claim={Claims.PROPERTY_VIEW}
        exact
        key={'activity_edit'}
        title={'Edit Activity'}
      />
      <AppRoute
        path={`/mapview/sidebar/management/:managementFileId/activities/:activityId`}
        customRender={({ match }) => (
          <FileActivityDetailContainer
            managementFileId={match.params.managementFileId}
            managementActivityId={match.params.activityId}
            onClose={() => {
              pathGenerator.showDetails(
                'management',
                match.params.managementFileId,
                'activities',
                true,
              );
            }}
            viewEnabled
            View={FileActivityDetailView}
          />
        )}
        claim={[Claims.PROPERTY_VIEW, Claims.MANAGEMENT_VIEW]}
        exact
        key={'activity_view'}
        title={'Activity View'}
      />
    </Switch>
  );
});

export default ManagementFileActivityRouter;
