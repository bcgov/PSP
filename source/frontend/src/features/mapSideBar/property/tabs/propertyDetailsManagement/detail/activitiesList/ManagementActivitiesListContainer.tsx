import { useCallback, useEffect, useState } from 'react';

import { usePropertyManagementRepository } from '@/hooks/repositories/usePropertyManagementRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import useIsMounted from '@/hooks/util/useIsMounted';

import { IManagementActivitiesListViewProps } from './ManagementActivitiesListView';
import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IPropertyManagementActivitiesListContainerProps {
  propertyId: number;
  View: React.FC<IManagementActivitiesListViewProps>;
}

const PropertyManagementActivitiesListContainer: React.FunctionComponent<
  IPropertyManagementActivitiesListContainerProps
> = ({ propertyId, View }) => {
  const isMounted = useIsMounted();
  const { setModalContent, setDisplayModal } = useModalContext();
  const [propertyActivities, setPropertyActivities] = useState<PropertyActivityRow[]>([]);

  const {
    getPropertyManagementActivities: { execute: getActivities, loading },
    deletePropertyManagementActivity: { execute: deleteActivity, loading: deletingActivity },
  } = usePropertyManagementRepository();

  const fetchPropertyActivities = useCallback(async () => {
    const response = await getActivities(propertyId);
    if (response && isMounted()) {
      setPropertyActivities([...response.map(x => PropertyActivityRow.fromApi(x))]);
    }
  }, [getActivities, isMounted, propertyId]);

  const onDelete = useCallback(
    async (managementActivityId: number) => {
      const result = await deleteActivity(propertyId, managementActivityId);
      if (result === true) {
        fetchPropertyActivities();
      }
    },
    [deleteActivity, fetchPropertyActivities, propertyId],
  );

  useEffect(() => {
    fetchPropertyActivities();
  }, [fetchPropertyActivities]);

  return (
    <View
      isLoading={loading || deletingActivity}
      propertyActivities={propertyActivities}
      onDelete={async (managementActivityId: number) => {
        setModalContent({
          ...getDeleteModalProps(),
          handleOk: async () => {
            await onDelete(managementActivityId);
            setDisplayModal(false);
          },
          handleCancel: () => {
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      }}
    />
  );
};

export default PropertyManagementActivitiesListContainer;
