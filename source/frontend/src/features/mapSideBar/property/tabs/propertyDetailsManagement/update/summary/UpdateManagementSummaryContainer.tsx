import { FormikProps } from 'formik';
import { useCallback, useEffect, useState } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { Api_Property } from '@/models/api/Property';
import { Api_PropertyLease } from '@/models/api/PropertyLease';

import { EditForms } from '../../../../PropertyViewSelector';
import { IUpdateManagementSummaryViewProps } from './UpdateManagementSummaryView';

interface IUpdateManagementSummaryContainerProps {
  property: Api_Property;
  setEditFormId: (formId: EditForms | null) => void;
  View: React.FC<IUpdateManagementSummaryViewProps>;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
}

export const UpdateManagementSummaryContainer: React.FC<IUpdateManagementSummaryContainerProps> = ({
  property,
  setEditFormId,
  View,
}) => {
  const [propertyLeases, setPropertyLeases] = useState<Api_PropertyLease[]>([]);

  const {
    getPropertyLeases: { execute: getPropertyLeases, loading: propertyLeasesLoading },
  } = usePimsPropertyRepository();

  const fetchPropertyLeases = useCallback(async () => {
    if (!property.id) {
      return;
    }
    const propertyLeasesResponse = await getPropertyLeases(property.id);
    if (propertyLeasesResponse) {
      setPropertyLeases(propertyLeasesResponse);
    }
  }, [getPropertyLeases, property]);

  useEffect(() => {
    fetchPropertyLeases();
  }, [fetchPropertyLeases]);

  return (
    <View
      isLoading={propertyLeasesLoading}
      property={property}
      propertyLeases={propertyLeases}
      setEditFormId={setEditFormId}
    />
  );
};
