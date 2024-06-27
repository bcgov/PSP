import { FormikProps } from 'formik';
import React from 'react';

import { LeaseViewPageForm } from '@/features/leases';
import { LeaseFormModel } from '@/features/leases/models';

import { ILeasePage } from '../LeaseContainer';

export interface ILeaseTabProps {
  leasePage?: ILeasePage<any>;
  onEdit?: () => void;
  isEditing: boolean;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onSuccess: () => void;
}

export const LeaseTab: React.FC<ILeaseTabProps> = ({
  leasePage,
  onEdit,
  isEditing,
  formikRef,
  onSuccess,
}) => {
  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }

  const Component = leasePage.component;

  return (
    <LeaseViewPageForm isEditing={isEditing} onEdit={onEdit} leasePageName={leasePage.pageName}>
      <Component
        onEdit={onEdit}
        isEditing={isEditing}
        formikRef={formikRef}
        onSuccess={onSuccess}
        componentView={leasePage.componentView}
      />
    </LeaseViewPageForm>
  );
};
