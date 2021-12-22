import * as React from 'react';
import { Redirect, Switch, useRouteMatch } from 'react-router-dom';
import AppRoute from 'utils/AppRoute';

import { leasePages } from '..';

interface ILeaseRouterProps {}

export const LeaseRouter: React.FunctionComponent<ILeaseRouterProps> = React.memo(props => {
  let { path, url } = useRouteMatch();
  return (
    <Switch>
      <Redirect exact from={path} to={`${url}/details`} />
      {Array.from(leasePages.entries()).map(([pageName, page]) => (
        <AppRoute
          path={`${path}/${pageName}`}
          component={page.component ?? React.Fragment}
          title={`${page.title}`}
          exact
          key={pageName}
        ></AppRoute>
      ))}
    </Switch>
  );
});

export default LeaseRouter;
