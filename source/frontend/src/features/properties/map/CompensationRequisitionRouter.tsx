import Claims from 'constants/claims';
import React from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router';
import AppRoute from 'utils/AppRoute';

import { CompensationRequisitionDetailContainer } from './compensation/detail/CompensationRequisitionDetailContainer';
import { CompensationRequisitionDetailView } from './compensation/detail/CompensationRequisitionDetailView';

interface ICompensationRequisitionRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const CompensationRequisitionRouter: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionRouterProps>
> = React.memo(props => {
  const location = useLocation();
  const history = useHistory();

  let matched = matchPath(location.pathname, {
    path: '/mapview/sidebar/acquisition/*/compensation-requisition/*',
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
    const backUrl = history.location.pathname.split('compensation-requisition')[0];
    props.setShowActionBar(false);
    history.push(backUrl);
  };

  return (
    <Switch>
      <AppRoute
        path={`/mapview/sidebar/acquisition/*/compensation-requisition/:id`}
        customRender={({ match }) => (
          <CompensationRequisitionDetailContainer
            compensationRequisitionId={Number(match.params.id)}
            onClose={onClose}
            View={CompensationRequisitionDetailView}
          ></CompensationRequisitionDetailContainer>
        )}
        claim={Claims.COMPENSATION_REQUISITION_VIEW}
        exact
        key={'compensation'}
        title={'Compensation'}
      />
    </Switch>
  );
});

export default CompensationRequisitionRouter;
