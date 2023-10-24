import { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { usePropertyActivityRepository } from '@/hooks/repositories/usePropertyActivityRepository';
import { Api_PropertyActivity, Api_PropertyActivitySubtype } from '@/models/api/PropertyActivity';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

import useActivityContactRetriever from '../hooks';
import { IPropertyActivityEditFormProps } from './PropertyActivityEditForm';

export interface IPropertyActivityEditContainerProps {
  propertyId: number;
  propertyActivityId?: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IPropertyActivityEditFormProps>>;
}

export const PropertyActivityEditContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityEditContainerProps>
> = ({ propertyId, propertyActivityId, onClose, View }) => {
  const { getSystemConstant } = useSystemConstants();

  const history = useHistory();

  const { setStaleLastUpdatedBy } = useContext(SideBarContext);

  const [show, setShow] = useState(true);

  const [loadedActivity, setLoadedActivity] = useState<Api_PropertyActivity | undefined>();

  const [subtypes, setSubtypes] = useState<Api_PropertyActivitySubtype[]>([]);

  const {
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: isContactLoading,
  } = useActivityContactRetriever();

  const {
    getActivitySubtypes: { execute: getSubtypes, loading: getSubtypesLoading },
    getActivity: { execute: getActivity, loading: getActivityLoading },
    createActivity: { execute: createActivity, loading: createActivityLoading },
    updateActivity: { execute: updateActivity, loading: updateActivityLoading },
  } = usePropertyActivityRepository();

  // Load the subtypes
  const fetchSubtypes = useCallback(async () => {
    const retrieved = await getSubtypes();
    if (retrieved !== undefined) {
      setSubtypes(retrieved);
    } else {
      setSubtypes([]);
    }
  }, [getSubtypes]);

  useEffect(() => {
    fetchSubtypes();
  }, [fetchSubtypes]);

  // Load the activity
  const fetchActivity = useCallback(
    async (propertyId: number, activityId: number) => {
      const retrieved = await getActivity(propertyId, activityId);
      if (retrieved !== undefined) {
        for (let i = 0; i < retrieved.ministryContacts.length; i++) {
          await fetchMinistryContacts(retrieved.ministryContacts[i]);
        }
        for (let i = 0; i < retrieved.involvedParties.length; i++) {
          await fetchPartiesContact(retrieved.involvedParties[i]);
        }
        await fetchProviderContact(retrieved);

        setLoadedActivity(retrieved);
      } else {
        setLoadedActivity(undefined);
      }
    },
    [fetchMinistryContacts, fetchPartiesContact, fetchProviderContact, getActivity],
  );

  useEffect(() => {
    if (propertyId !== undefined && propertyActivityId !== undefined) {
      fetchActivity(propertyId, propertyActivityId);
    }
  }, [propertyId, propertyActivityId, fetchActivity]);

  const gstConstant = getSystemConstant(SystemConstants.GST);
  const pstConstant = getSystemConstant(SystemConstants.PST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) * 0.01 : 0;
  const pstDecimal = pstConstant !== undefined ? parseFloat(pstConstant.value) * 0.01 : 0;

  const onSave = async (model: Api_PropertyActivity) => {
    let result = undefined;
    if (model.id !== 0) {
      result = await updateActivity(propertyId, model);
    } else {
      result = await createActivity(propertyId, model);
    }

    if (result !== undefined) {
      setStaleLastUpdatedBy(true);
      history.push(`/mapview/sidebar/property/${propertyId}/activity/${result.id}`);
    }
  };

  const onCancelClick = () => {
    if (propertyActivityId !== undefined) {
      history.push(`/mapview/sidebar/property/${propertyId}/activity/${propertyActivityId}`);
    } else {
      onClose();
    }
  };

  return (
    <View
      propertyId={propertyId}
      activity={loadedActivity}
      subtypes={subtypes}
      gstConstant={gstDecimal}
      pstConstant={pstDecimal}
      onCancel={onCancelClick}
      loading={
        getSubtypesLoading ||
        getActivityLoading ||
        createActivityLoading ||
        updateActivityLoading ||
        isContactLoading
      }
      show={show}
      setShow={setShow}
      onSave={onSave}
    />
  );
};
