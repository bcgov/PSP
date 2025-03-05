import { FormikProps } from 'formik/dist/types';
import React, { useContext } from 'react';

import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { UpdateLeaseContainer } from '@/features/leases/detail/LeasePages/details/UpdateLeaseContainer';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';

import LeaseDetailsWrapperView from './LeaseDetailsWrapperView';
import UpdateLeaseForm from './UpdateLeaseForm';

const DetailContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps<void>>> = ({
  isEditing,
  onEdit,
  formikRef,
}) => {
  const { lease } = useContext(LeaseStateContext);

  return !!isEditing && !!onEdit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <UpdateLeaseContainer
        View={UpdateLeaseForm}
        onEdit={onEdit}
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
      />
    </ProtectedComponent>
  ) : (
    <LeaseDetailsWrapperView lease={lease} />
  );
};

export default DetailContainer;
