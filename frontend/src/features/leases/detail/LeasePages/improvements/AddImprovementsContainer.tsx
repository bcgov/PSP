import * as API from 'constants/API';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { apiLeaseToFormLease, formLeaseToApiLease } from 'features/leases/leaseUtils';
import { FormikProps } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { IFormLease, ILeaseImprovement } from 'interfaces';
import { sortBy } from 'lodash';
import * as React from 'react';
import { useContext } from 'react';
import { useHistory } from 'react-router';
import { ILookupCode } from 'store/slices/lookupCodes';

import AddImprovementsForm from './AddImprovementsForm';

interface IAddImprovementsContainerProps {}

export const AddImprovementsContainer: React.FunctionComponent<IAddImprovementsContainerProps> = () => {
  const formikRef = React.useRef<FormikProps<IFormLease>>(null);
  const { lease, setLease } = useContext(LeaseStateContext);
  const { updateLease } = useUpdateLease();
  const history = useHistory();
  const { getByType } = useLookupCodeHelpers();
  const improvementTypeCodes = getByType(API.PROPERTY_IMPROVEMENT_TYPES);

  const onCancel = () => {
    history.push(`/lease/${lease?.id}/improvements`);
  };

  const onSubmit = async (lease: IFormLease) => {
    try {
      const leaseToUpdate = formLeaseToApiLease(removeEmptyImprovements(lease));
      const updatedLease = await updateLease(leaseToUpdate, undefined, undefined, 'improvements');
      if (!!updatedLease?.id) {
        formikRef?.current?.resetForm({ values: apiLeaseToFormLease(updatedLease) });
        setLease(updatedLease);
        history.push(`/lease/${updatedLease?.id}/improvements`);
      }
    } finally {
      formikRef?.current?.setSubmitting(false);
    }
  };

  return (
    <>
      <AddImprovementsForm
        onCancel={onCancel}
        formikRef={formikRef}
        onSubmit={onSubmit}
        initialValues={addEmptyImprovements(apiLeaseToFormLease(lease), improvementTypeCodes)}
      />
    </>
  );
};

/**
 * Populate the lease with all required improvement types, use existing values if present, otherwise inject empty, default values.
 * @param lease
 * @param improvementTypes
 */
const addEmptyImprovements = (
  lease: IFormLease | undefined,
  improvementTypes: ILookupCode[],
): IFormLease | undefined => {
  const allImprovements: ILeaseImprovement[] = [];
  improvementTypes.forEach(improvementType => {
    let improvementForType: ILeaseImprovement | undefined = lease?.improvements.find(
      existingImprovement => existingImprovement.propertyImprovementTypeId === improvementType.id,
    );
    if (improvementForType === undefined) {
      improvementForType = {
        propertyImprovementTypeId: improvementType.id as string,
        propertyImprovementType: improvementType.description ?? '',
        description: '',
        structureSize: '',
        address: '',
      };
    }
    allImprovements.push(improvementForType);
  });
  return {
    ...lease,
    improvements: sortBy(allImprovements, 'propertyImprovementTypeId'),
  } as IFormLease;
};

/**
 * Remove any improvements that are empty before saving to the API.
 * @param lease
 */
const removeEmptyImprovements = (lease: IFormLease) => {
  return {
    ...lease,
    improvements: lease.improvements.filter(
      improvement =>
        !!improvement.address || !!improvement.description || !!improvement.structureSize,
    ),
  };
};
