import React from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router';

import { Claims } from '@/constants';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import AppRoute from '@/utils/AppRoute';

import { CompensationRequisitionTrayContainer } from '../compensation/CompensationRequisitionTrayContainer';
import { CompensationRequisitionTrayView } from '../compensation/CompensationRequisitionTrayView';

interface ICompensationRequisitionRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const CompensationRequisitionRouter: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionRouterProps>
> = React.memo(props => {
  const location = useLocation();
  const history = useHistory();

  const matched = matchPath(location.pathname, {
    path: [
      '/mapview/sidebar/acquisition/*/compensation-requisition/*',
      `/mapview/sidebar/lease/*/compensation-requisition/:id`,
    ],
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
        path={[`/mapview/sidebar/acquisition/*/compensation-requisition/:id`]}
        customRender={({ match }) => (
          <CompensationRequisitionTrayContainer
            fileType={ApiGen_CodeTypes_FileTypes.Acquisition}
            compensationRequisitionId={Number(match.params.id)}
            onClose={onClose}
            View={CompensationRequisitionTrayView}
          />
        )}
        claim={Claims.COMPENSATION_REQUISITION_VIEW}
        exact
        key={'compensation'}
        title={'Compensation'}
      />{' '}
      <AppRoute
        path={[`/mapview/sidebar/lease/*/compensation-requisition/:id`]}
        customRender={({ match }) => (
          <CompensationRequisitionTrayContainer
            fileType={ApiGen_CodeTypes_FileTypes.Lease}
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
