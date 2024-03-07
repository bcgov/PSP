import React from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router';

import { Claims } from '@/constants';
import AppRoute from '@/utils/AppRoute';

import { CompensationRequisitionTrayContainer } from '../acquisition/tabs/compensation/CompensationRequisitionTrayContainer';
import { CompensationRequisitionTrayView } from '../acquisition/tabs/compensation/CompensationRequisitionTrayView';

interface ICompensationRequisitionRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const CompensationRequisitionRouter: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionRouterProps>
> = React.memo(props => {
  const location = useLocation();
  const history = useHistory();

  const matched = matchPath(location.pathname, {
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
    const backUrl = location.pathname.split('/compensation-requisition')[0];
    history.push(backUrl);
  };

  return (
    <Switch>
      <AppRoute
        path={`/mapview/sidebar/acquisition/*/compensation-requisition/:id`}
        customRender={({ match }) => (
          <CompensationRequisitionTrayContainer
            compensationRequisitionId={Number(match.params.id)}
            onClose={onClose}
            View={CompensationRequisitionTrayView}
          />
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
