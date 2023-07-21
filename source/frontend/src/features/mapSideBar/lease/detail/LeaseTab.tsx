import { FormikProps } from 'formik';
import React from 'react';

import { LeaseViewPageForm } from '@/features/leases';
import { LeaseFormModel } from '@/features/leases/models';

import { ILeasePage } from '../LeaseContainer';

export interface ILeaseTabProps {
  leasePage?: ILeasePage;
  onEdit?: () => void;
  isEditing: boolean;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
}

export const LeaseTab: React.FC<ILeaseTabProps> = ({ leasePage, onEdit, isEditing, formikRef }) => {
  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }

  const Component = leasePage.component;

  return (
    <LeaseViewPageForm isEditing={isEditing} onEdit={onEdit}>
      <Component onEdit={onEdit} isEditing={isEditing} formikRef={formikRef} />
    </LeaseViewPageForm>
  );
};
