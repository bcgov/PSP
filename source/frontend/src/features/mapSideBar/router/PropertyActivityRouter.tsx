import React from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router';

import { Claims } from '@/constants';
import AppRoute from '@/utils/AppRoute';

import { PropertyActivityDetailContainer } from '../property/tabs/propertyDetailsManagement/activity/detail/PropertyActivityDetailContainer';
import { PropertyActivityDetailView } from '../property/tabs/propertyDetailsManagement/activity/detail/PropertyActivityDetailView';
import { PropertyActivityEditContainer } from '../property/tabs/propertyDetailsManagement/activity/edit/PropertyActivityEditContainer';
import { PropertyActivityEditForm } from '../property/tabs/propertyDetailsManagement/activity/edit/PropertyActivityEditForm';

interface IPropertyActivityRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const PropertyActivityRouter: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityRouterProps>
> = React.memo(props => {
  const location = useLocation();
  const history = useHistory();

  const matched = matchPath(location.pathname, {
    path: '/mapview/sidebar/property/*/activity/*',
    exact: true,
    strict: true,
  });

  React.useEffect(() => {
    if (matched !== null) {
      props.setShowActionBar(true);
    } else {
      props.setShowActionBar(false);
    }
  }, [matched, props]);

  const onClose = () => {
    const backUrl = location.pathname.split('/activity')[0];
    history.push(backUrl);
  };

  return (
    <Switch>
      <AppRoute
        path={`/mapview/sidebar/property/:propertyId/management/activity/new`}
        customRender={({ match }) => (
          <PropertyActivityEditContainer
            propertyId={Number(match.params.propertyId)}
            onClose={onClose}
            View={PropertyActivityEditForm}
          />
        )}
        claim={Claims.PROPERTY_VIEW}
        exact
        key={'activity_new'}
        title={'Activity New'}
      />
      <AppRoute
        path={`/mapview/sidebar/property/:propertyId/management/activity/:activityId/edit`}
        customRender={({ match }) => (
          <PropertyActivityEditContainer
            propertyId={Number(match.params.propertyId)}
            propertyActivityId={Number(match.params.activityId)}
            onClose={onClose}
            View={PropertyActivityEditForm}
          />
        )}
        claim={Claims.PROPERTY_VIEW}
        exact
        key={'activity_edit'}
        title={'Activity Edit'}
      />
      <AppRoute
        path={`/mapview/sidebar/property/:propertyId/management/activity/:activityId`}
        customRender={({ match }) => (
          <PropertyActivityDetailContainer
            propertyId={Number(match.params.propertyId)}
            propertyActivityId={Number(match.params.activityId)}
            onClose={onClose}
            View={PropertyActivityDetailView}
          />
        )}
        claim={Claims.PROPERTY_VIEW}
        exact
        key={'activity'}
        title={'Activity View'}
      />
    </Switch>
  );
});

export default PropertyActivityRouter;
