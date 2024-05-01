import { FormikProps } from 'formik/dist/types';
import { sortBy } from 'lodash';
import { useContext } from 'react';

import * as API from '@/constants/API';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { ILookupCode } from '@/store/slices/lookupCodes';

import AddImprovementsForm from './AddImprovementsForm';
import { ILeaseImprovementForm, ILeaseImprovementsForm } from './models';

interface IAddImprovementsContainerProps {
  formikRef: React.RefObject<FormikProps<any>>;
  onEdit?: (isEditing: boolean) => void;
  improvements: ApiGen_Concepts_PropertyImprovement[];
  loading: boolean;
  onSuccess: () => void;
}

export const AddImprovementsContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddImprovementsContainerProps>
> = ({ formikRef, onEdit, children, loading, improvements, onSuccess }) => {
  const { lease } = useContext(LeaseStateContext);
  const { updatePropertyImprovements } = usePropertyImprovementRepository();
  const { getByType } = useLookupCodeHelpers();
  const improvementTypeCodes = getByType(API.PROPERTY_IMPROVEMENT_TYPES);

  const onSubmit = async (form: ILeaseImprovementsForm) => {
    try {
      const apiImprovements = removeEmptyImprovements(form).improvements.map(i =>
        ILeaseImprovementForm.toApi(i),
      );
      if (lease?.id && lease.rowVersion) {
        const updatedPropertyImprovements = await updatePropertyImprovements.execute(
          lease.id,
          apiImprovements,
        );
        if (updatedPropertyImprovements) {
          const form = new ILeaseImprovementsForm();
          form.improvements = updatedPropertyImprovements.map(i =>
            ILeaseImprovementForm.fromApi(i),
          );
          formikRef?.current?.resetForm({ values: form });
          onEdit && onEdit(false);
          onSuccess();
        }
      }
    } finally {
      formikRef?.current?.setSubmitting(false);
    }
  };
  const form = new ILeaseImprovementsForm();
  form.improvements = improvements?.map(i => ILeaseImprovementForm.fromApi(i)) ?? [];

  return (
    <AddImprovementsForm
      loading={loading}
      formikRef={formikRef}
      onSubmit={onSubmit}
      initialValues={addEmptyImprovements(form, improvementTypeCodes, lease?.id ?? undefined)}
    >
      {children}
    </AddImprovementsForm>
  );
};

/**
 * Populate the lease with all required improvement types, use existing values if present, otherwise inject empty, default values.
 * @param lease
 * @param improvementTypes
 */
const addEmptyImprovements = (
  lease: ILeaseImprovementsForm,
  improvementTypes: ILookupCode[],
  leaseId?: number,
): ILeaseImprovementsForm | undefined => {
  const allImprovements: ILeaseImprovementForm[] = [];

  improvementTypes.forEach(improvementType => {
    let improvementForType: ILeaseImprovementForm | undefined = lease?.improvements.find(
      existingImprovement => existingImprovement.propertyImprovementTypeId === improvementType.id,
    );
    if (improvementForType === undefined) {
      improvementForType = {
        propertyImprovementTypeId: improvementType.id as string,
        propertyImprovementType: improvementType.description ?? '',
        description: '',
        structureSize: '',
        address: '',
        leaseId: leaseId ?? 0,
      };
    }
    allImprovements.push(improvementForType);
  });
  return {
    improvements: sortBy(allImprovements, 'displayOrder'),
  } as ILeaseImprovementsForm;
};

/**
 * Remove any improvements that are empty before saving to the API.
 * @param lease
 */
const removeEmptyImprovements = (form: ILeaseImprovementsForm) => {
  return {
    ...form,
    improvements: form.improvements.filter(
      improvement =>
        !!improvement.address || !!improvement.description || !!improvement.structureSize,
    ),
  };
};
