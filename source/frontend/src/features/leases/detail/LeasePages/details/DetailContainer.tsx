import { FormikProps } from 'formik/dist/types';

import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';
import { UpdateLeaseContainer } from '@/features/leases/detail/LeasePages/details/UpdateLeaseContainer';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';

import LeaseDetailsForm from './LeaseDetailsForm';
import UpdateLeaseForm from './UpdateLeaseForm';

const DetailContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps>> = ({
  isEditing,
  onEdit,
  formikRef,
}) => {
  return !!isEditing && !!onEdit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <UpdateLeaseContainer
        View={UpdateLeaseForm}
        onEdit={onEdit}
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
      />
    </ProtectedComponent>
  ) : (
    <LeaseDetailsForm />
  );
};

export default DetailContainer;
