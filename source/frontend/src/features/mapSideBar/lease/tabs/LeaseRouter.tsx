import { FormikProps } from 'formik/dist/types';
import React, { useContext } from 'react';
import { Switch, useRouteMatch } from 'react-router-dom';

import Claims from '@/constants/claims';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { exists, stripTrailingSlash } from '@/utils';
import AppRoute from '@/utils/AppRoute';

import { LeasePageNames, LeasePageProps } from '../LeaseContainer';
import ConsultationListContainer from './consultations/detail/ConsultationListContainer';
import ConsultationListView from './consultations/detail/ConsultationListView';
import ConsultationAddContainer from './consultations/edit/ConsultationAddContainer';
import ConsultationEditForm from './consultations/edit/ConsultationEditForm';
import ConsultationUpdateContainer from './consultations/edit/ConsultationUpdateContainer';

export interface ILeaseRouterProps extends LeasePageProps<void> {
  isEditing: boolean;
  onEdit?: (isEditing: boolean) => void;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onSuccess: () => void;
  componentView: React.FunctionComponent<React.PropsWithChildren<void>>;
}

export const LeaseRouter: React.FunctionComponent<React.PropsWithChildren<ILeaseRouterProps>> = ({
  onSuccess,
}) => {
  const { lease } = useContext(LeaseStateContext);
  const { path } = useRouteMatch();

  if (!exists(lease)) {
    return null;
  }

  const consultationsPath = LeasePageNames.CONSULTATIONS;
  return (
    <Switch>
      <AppRoute
        exact
        path={`${stripTrailingSlash(path)}`}
        customRender={() => (
          <ConsultationListContainer leaseId={lease.id} View={ConsultationListView} />
        )}
        claim={Claims.LEASE_VIEW}
        key={'consultation'}
        title="Lease Approval/Consultation"
      />
      <AppRoute
        exact
        path={`${stripTrailingSlash(path)}/${consultationsPath}/add`}
        customRender={() => (
          <ConsultationAddContainer
            leaseId={lease.id}
            View={ConsultationEditForm}
            onSuccess={onSuccess}
          />
        )}
        claim={Claims.LEASE_EDIT}
        key={'consultation'}
        title="Add Lease Approval/Consultation"
      />
      <AppRoute
        exact
        path={`${stripTrailingSlash(path)}/${consultationsPath}/:consultationId/edit`}
        customRender={({ match }) => (
          <ConsultationUpdateContainer
            leaseId={lease.id}
            consultationId={match.params.consultationId}
            View={ConsultationEditForm}
            onSuccess={onSuccess}
          />
        )}
        claim={Claims.LEASE_EDIT}
        key={'consultation'}
        title="Edit Lease Approval/Consultation"
      />
    </Switch>
  );
};

export default LeaseRouter;
