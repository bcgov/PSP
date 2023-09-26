import { FormikProps } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';

import { usePropertyManagementRepository } from '@/hooks/repositories/usePropertyManagementRepository';
import { Api_Property, Api_PropertyManagement } from '@/models/api/Property';

import { EditForms } from '../../../../PropertyViewSelector';
import { PropertyManagementFormModel } from './models';
import { IUpdateManagementSummaryViewProps } from './UpdateManagementSummaryView';

interface IUpdateManagementSummaryContainerProps {
  property: Api_Property;
  setEditFormId: (formId: EditForms | null) => void;
  onSuccess: () => void;
  View: React.FC<IUpdateManagementSummaryViewProps>;
}

export const UpdateManagementSummaryContainer = React.forwardRef<
  FormikProps<PropertyManagementFormModel>,
  IUpdateManagementSummaryContainerProps
>(({ property, setEditFormId, View, onSuccess }, ref) => {
  const [propertyManagement, setPropertyManagement] = useState<Api_PropertyManagement>({
    id: property.id ?? 0,
    rowVersion: null,
    managementPurposes: [],
    additionalDetails: null,
    isUtilitiesPayable: null,
    isTaxesPayable: null,
    isLeaseActive: false,
    isLeaseExpired: false,
    leaseExpiryDate: null,
  });

  const {
    getPropertyManagement: { execute: getPropertyManagement, loading },
  } = usePropertyManagementRepository();

  const fetchPropertyManagement = useCallback(async () => {
    if (!property.id) {
      return;
    }
    const response = await getPropertyManagement(property.id);
    if (response) {
      setPropertyManagement(response);
    }
  }, [getPropertyManagement, property.id]);

  useEffect(() => {
    fetchPropertyManagement();
  }, [fetchPropertyManagement]);

  const onSave = async (apiModel: Api_PropertyManagement) => {
    // TODO: Implement save
  };

  return <View isLoading={loading} propertyManagement={propertyManagement} onSave={onSave} />;
});
