import orderBy from 'lodash/orderBy';
import { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router';

import { CancelConfirmationModal } from '@/components/common/CancelConfirmationModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Claims } from '@/constants';
import { INSURANCE_TYPES } from '@/constants/API';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useInsurancesRepository } from '@/hooks/repositories/useInsuranceRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { isValidId } from '@/utils';

import InsuranceDetailsView from './details/Insurance';
import InsuranceEditContainer from './edit/EditInsuranceContainer';

const InsuranceContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps>> = ({
  isEditing,
  onEdit,
  formikRef,
  onSuccess,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const [showCancelModal, setShowCancelModal] = useState(false);
  const {
    getInsurances: { execute: getInsurances, loading, response: insurances },
    updateInsurances: { execute: updateInsurances },
  } = useInsurancesRepository();

  const { lease } = useContext(LeaseStateContext);
  const insuranceList = orderBy(insurances, i => i.insuranceType?.displayOrder) ?? [];
  const leaseId = lease?.id;
  useEffect(() => {
    leaseId && getInsurances(leaseId);
  }, [getInsurances, leaseId]);

  const lookupCodes = useLookupCodeHelpers();
  const insuranceTypes = lookupCodes.getByType(INSURANCE_TYPES).sort((a, b) => {
    return (a.displayOrder || 0) - (b.displayOrder || 0);
  });
  const location = useLocation();
  const history = useHistory();

  const onSave = useCallback(
    async (insurances: ApiGen_Concepts_Insurance[]) => {
      if (isValidId(leaseId)) {
        const updatedInsurance = await updateInsurances(leaseId, insurances);
        if (updatedInsurance) {
          leaseId && (await getInsurances(leaseId));
          onEdit && onEdit(false);
          onSuccess();
        }
      }
    },
    [getInsurances, leaseId, onEdit, updateInsurances, onSuccess],
  );

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen />
      {!isEditing && (
        <InsuranceDetailsView insuranceList={insuranceList} insuranceTypes={insuranceTypes} />
      )}
      {isEditing && isValidId(leaseId) && !loading && hasClaim(Claims.LEASE_EDIT) && (
        <InsuranceEditContainer
          formikRef={formikRef}
          leaseId={leaseId}
          insuranceList={insuranceList}
          insuranceTypes={insuranceTypes}
          onSave={onSave}
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
