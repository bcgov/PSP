import { CancelConfirmationModal } from 'components/common/CancelConfirmationModal';
import { INSURANCE_TYPES } from 'constants/API';
import { Claims } from 'constants/claims';
import { useLeaseDetail } from 'features/leases';
import { LeasePageProps } from 'features/properties/map/lease/LeaseContainer';
import { getIn } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IInsurance } from 'interfaces';
import React, { useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';

import InsuranceDetailsView from './details/Insurance';
import InsuranceEditContainer from './edit/EditInsuranceContainer';

const InsuranceContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps>> = ({
  isEditing,
  onEdit,
  formikRef,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const [showCancelModal, setShowCancelModal] = useState(false);

  const { refresh, lease } = useLeaseDetail();
  const leaseId: number = getIn(lease, 'id') || -1;
  const insuranceList: IInsurance[] = getIn(lease, 'insurances') ?? [];

  const lookupCodes = useLookupCodeHelpers();
  const insuranceTypes = lookupCodes.getByType(INSURANCE_TYPES).sort((a, b) => {
    return (a.displayOrder || 0) - (b.displayOrder || 0);
  });
  const location = useLocation();
  const history = useHistory();

  return (
    <>
      {!isEditing && (
        <InsuranceDetailsView insuranceList={insuranceList} insuranceTypes={insuranceTypes} />
      )}
      {isEditing && hasClaim(Claims.LEASE_EDIT) && (
        <InsuranceEditContainer
          formikRef={formikRef}
          leaseId={leaseId}
          insuranceList={insuranceList}
          insuranceTypes={insuranceTypes}
          onSuccess={async () => {
            await refresh();
            onEdit && onEdit(false);
          }}
          onCancel={(dirty?: boolean) => {
            if (dirty) {
              setShowCancelModal(true);
            }
          }}
        />
      )}
      <CancelConfirmationModal
        display={showCancelModal}
        setDisplay={setShowCancelModal}
        handleOk={() => history.push(location.pathname)}
      />
    </>
  );
};

export default InsuranceContainer;
