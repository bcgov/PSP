import { ProtectedComponent } from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import { UpdateLeaseContainer } from 'features/leases/detail/LeasePages/details/UpdateLeaseContainer';
import { LeaseFormModel } from 'features/leases/models';
import { LeasePageProps } from 'features/properties/map/lease/LeaseContainer';
import { FormikProps } from 'formik';
import * as React from 'react';

import LeaseDetailsForm from './LeaseDetailsForm';

const DetailContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps>> = ({
  isEditing,
  onEdit,
  formikRef,
}) => {
  return !!isEditing && !!onEdit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <UpdateLeaseContainer
        onEdit={onEdit}
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
      />
    </ProtectedComponent>
  ) : (
    <LeaseDetailsForm />
  );
};

export default DetailContainer;
