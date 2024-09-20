import { FormikProps } from 'formik/dist/types';
import React, { useCallback, useContext } from 'react';

import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { UpdateLeaseContainer } from '@/features/leases/detail/LeasePages/details/UpdateLeaseContainer';
import { LeaseFormModel } from '@/features/leases/models';
import { useGenerateLicenceOfOccupation } from '@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateLicenceOfOccupation';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists } from '@/utils';

import LeaseDetailsForm from './LeaseDetailsForm';
import UpdateLeaseForm from './UpdateLeaseForm';

const DetailContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps<void>>> = ({
  isEditing,
  onEdit,
  formikRef,
}) => {
  const { lease } = useContext(LeaseStateContext);
  const generateLicenceOfOccupation = useGenerateLicenceOfOccupation();

  const onGenerate = useCallback(
    (lease?: ApiGen_Concepts_Lease) => {
      if (exists(lease)) {
        generateLicenceOfOccupation(lease);
      }
    },
    [generateLicenceOfOccupation],
  );

  return !!isEditing && !!onEdit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <UpdateLeaseContainer
        View={UpdateLeaseForm}
        onEdit={onEdit}
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
      />
    </ProtectedComponent>
  ) : (
    <LeaseDetailsForm lease={lease} onGenerate={onGenerate} />
  );
};

export default DetailContainer;
