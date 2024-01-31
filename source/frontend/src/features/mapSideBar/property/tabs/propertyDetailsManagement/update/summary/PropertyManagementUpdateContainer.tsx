import { FormikProps } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';

import { usePropertyManagementRepository } from '@/hooks/repositories/usePropertyManagementRepository';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

import { PropertyManagementFormModel } from './models';
import { IPropertyManagementUpdateFormProps } from './PropertyManagementUpdateForm';

export interface IPropertyManagementUpdateContainerProps {
  propertyId: number;
  onSuccess: () => void;
  View: React.ForwardRefExoticComponent<
    IPropertyManagementUpdateFormProps &
      React.RefAttributes<FormikProps<PropertyManagementFormModel>>
  >;
}

export const PropertyManagementUpdateContainer = React.forwardRef<
  FormikProps<PropertyManagementFormModel>,
  IPropertyManagementUpdateContainerProps
>(({ propertyId, View, onSuccess }, formikRef) => {
  const [propertyManagement, setPropertyManagement] = useState<ApiGen_Concepts_PropertyManagement>({
    id: propertyId,
    managementPurposes: [],
    additionalDetails: null,
    isUtilitiesPayable: null,
    isTaxesPayable: null,
    relatedLeases: 0,
    leaseExpiryDate: null,
    ...getEmptyBaseAudit(),
  });

  const {
    getPropertyManagement: { execute: getPropertyManagement, loading: loadingGet },
    updatePropertyManagement: { execute: updatePropertyManagement, loading: loadingUpdate },
  } = usePropertyManagementRepository();

  const fetchPropertyManagement = useCallback(async () => {
    if (!propertyId) {
      return;
    }
    const response = await getPropertyManagement(propertyId);
    if (response) {
      setPropertyManagement(response);
    }
  }, [getPropertyManagement, propertyId]);

  useEffect(() => {
    fetchPropertyManagement();
  }, [fetchPropertyManagement]);

  const onSave = async (apiModel: ApiGen_Concepts_PropertyManagement) => {
    const result = await updatePropertyManagement(propertyId, apiModel);
    if (result?.id) {
      onSuccess();
    }
  };

  return (
    <View
      isLoading={loadingGet || loadingUpdate}
      propertyManagement={propertyManagement}
      onSave={onSave}
      ref={formikRef}
    />
  );
});
