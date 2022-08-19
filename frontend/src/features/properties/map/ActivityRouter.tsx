import Claims from 'constants/claims';
import { ActivityContainer } from 'features/research/activities/activity/ActivityContainer';
import * as React from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router-dom';
import AppRoute from 'utils/AppRoute';

interface IActivityRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const ActivityRouter: React.FunctionComponent<IActivityRouterProps> = React.memo(props => {
  const location = useLocation();
  const history = useHistory();

  let matched = matchPath(location.pathname, {
    path: '/mapview/sidebar/*/*/activity/*',
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
    const backUrl = history.location.pathname.split('activity')[0];
    props.setShowActionBar(false);
    history.push(backUrl);
  };

  return (
    <Switch>
      <AppRoute
        path={`/mapview/sidebar/*/*/activity/:activityId`}
        customRender={({ match }) => (
          <ActivityContainer
            activityId={Number(match.params.activityId)}
            onClose={onClose}
          ></ActivityContainer>
        )}
        claim={Claims.ACTIVITY_VIEW}
        exact
        key={'activity'}
        title={'Activity'}
      />
    </Switch>
  );
});

export default ActivityRouter;
