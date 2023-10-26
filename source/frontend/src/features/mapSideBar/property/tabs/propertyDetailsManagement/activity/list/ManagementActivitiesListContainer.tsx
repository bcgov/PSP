import { useCallback, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { usePropertyActivityRepository } from '@/hooks/repositories/usePropertyActivityRepository';
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
  const history = useHistory();
  const isMounted = useIsMounted();
  const { setModalContent, setDisplayModal } = useModalContext();
  const [propertyActivities, setPropertyActivities] = useState<PropertyActivityRow[]>([]);

  const {
    getActivities: { execute: getActivities, loading },
    deleteActivity: { execute: deleteActivity, loading: deletingActivity },
  } = usePropertyActivityRepository();

  const fetchPropertyActivities = useCallback(async () => {
    const response = await getActivities(propertyId);
    if (response && isMounted()) {
      setPropertyActivities([...response.map(x => PropertyActivityRow.fromApi(x))]);
    }
  }, [getActivities, isMounted, propertyId]);

  const onDelete = useCallback(
    async (activityId: number) => {
      const result = await deleteActivity(propertyId, activityId);
      if (result === true) {
        fetchPropertyActivities();
      }
    },
    [deleteActivity, fetchPropertyActivities, propertyId],
  );

  useEffect(() => {
    fetchPropertyActivities();
  }, [fetchPropertyActivities]);

  const onCreate = () => {
    history.push(`/mapview/sidebar/property/${propertyId}/activity/new`);
  };

  const onView = (activityId: number) => {
    history.push(`/mapview/sidebar/property/${propertyId}/activity/${activityId}`);
  };

  return (
    <View
      isLoading={loading || deletingActivity}
      propertyActivities={propertyActivities}
      onCreate={onCreate}
      onView={onView}
      onDelete={async (activityId: number) => {
        setModalContent({
          ...getDeleteModalProps(),
          handleOk: async () => {
            await onDelete(activityId);
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
