import { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import useActivityContactRetriever from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/hooks';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivitySubtype } from '@/models/api/generated/ApiGen_Concepts_PropertyActivitySubtype';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists, isValidId } from '@/utils/utils';

import { IManagementActivityEditFormProps } from './ManagementActivityEditForm';

export interface IManagementActivityEditContainerProps {
  managementFileId: number;
  activityId?: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IManagementActivityEditFormProps>>;
}

export const ManagementActivityEditContainer: React.FunctionComponent<
  IManagementActivityEditContainerProps
> = ({ managementFileId, activityId, onClose, View }) => {
  const { getSystemConstant } = useSystemConstants();
  const history = useHistory();
  const [show, setShow] = useState(true);
  const [subtypes, setSubtypes] = useState<ApiGen_Concepts_PropertyActivitySubtype[]>([]);
  const [loadedActivity, setLoadedActivity] = useState<
    ApiGen_Concepts_PropertyActivity | undefined
  >();

  const {
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: isContactLoading,
  } = useActivityContactRetriever();

  const {
    getActivitySubtypes: { execute: getSubtypes, loading: getSubtypesLoading },
    getManagementActivity: { execute: getManagementActivity, loading: getActivityLoading },
    addManagementActivity: { execute: addManagementActivity, loading: addActivityLoading },
    updateManagementActivity: { execute: updateManagementActivity, loading: updateActivityLoading },
  } = useManagementActivityRepository();

  const { file, fileLoading, setStaleLastUpdatedBy } = useContext(SideBarContext);

  const castedFile = file as unknown as ApiGen_Concepts_ManagementFile;

  // Load the subtypes
  const fetchSubtypes = useCallback(async () => {
    const retrieved = await getSubtypes();
    if (exists(retrieved)) {
      setSubtypes(retrieved);
    } else {
      setSubtypes([]);
    }
  }, [getSubtypes]);

  useEffect(() => {
    fetchSubtypes();
  }, [fetchSubtypes]);

  if (!isValidId(file?.id) && fileLoading === false) {
    return null;
  }

  // Load the activity
  const fetchActivity = useCallback(
    async (managementFileId: number, activityId: number) => {
      const retrieved = await getManagementActivity(managementFileId, activityId);
      if (exists(retrieved)) {
        if (exists(retrieved.ministryContacts)) {
          for (let i = 0; i < retrieved.ministryContacts.length; i++) {
            await fetchMinistryContacts(retrieved.ministryContacts[i]);
          }
        }
        if (exists(retrieved.involvedParties)) {
          for (let i = 0; i < retrieved.involvedParties.length; i++) {
            await fetchPartiesContact(retrieved.involvedParties[i]);
          }
        }
        await fetchProviderContact(retrieved);

        setLoadedActivity(retrieved);
      } else {
        setLoadedActivity(undefined);
      }
    },
    [fetchMinistryContacts, fetchPartiesContact, fetchProviderContact, getManagementActivity],
  );

  useEffect(() => {
    if (isValidId(managementFileId) && isValidId(activityId)) {
      fetchActivity(managementFileId, activityId);
    }
  }, [managementFileId, activityId, fetchActivity]);

  const gstConstant = getSystemConstant(SystemConstants.GST);
  const pstConstant = getSystemConstant(SystemConstants.PST);
  const gstDecimal = exists(gstConstant) ? parseFloat(gstConstant.value) * 0.01 : 0;
  const pstDecimal = exists(pstConstant) ? parseFloat(pstConstant.value) * 0.01 : 0;

  const onSave = async (model: ApiGen_Concepts_PropertyActivity) => {
    let result: ApiGen_Concepts_PropertyActivity | undefined = undefined;
    if (isValidId(model.id)) {
      result = await updateManagementActivity(managementFileId, model);
    } else {
      result = await addManagementActivity(managementFileId, model);
    }

    if (exists(result) && isValidId(managementFileId)) {
      setStaleLastUpdatedBy(true);
      history.push(`/mapview/sidebar/management/${managementFileId}/activities/${result.id}`);
    }
  };

  const onCancelClick = () => {
    if (isValidId(managementFileId) && isValidId(activityId)) {
      history.push(`/mapview/sidebar/management/${managementFileId}/activities/${activityId}`);
    } else {
      onClose();
    }
  };

  return exists(castedFile) && isValidId(castedFile?.id) ? (
    <View
      managementFile={castedFile}
      activity={loadedActivity}
      subtypes={subtypes}
      gstConstant={gstDecimal}
      pstConstant={pstDecimal}
      onCancel={onCancelClick}
      loading={
        getSubtypesLoading ||
        addActivityLoading ||
        getActivityLoading ||
        updateActivityLoading ||
        isContactLoading
      }
      show={show}
      setShow={setShow}
      onSave={onSave}
      onClose={onClose}
    />
  ) : null;
};
