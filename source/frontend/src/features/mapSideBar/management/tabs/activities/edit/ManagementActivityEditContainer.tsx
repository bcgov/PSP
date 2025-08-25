import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import useActivityContactRetriever from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/hooks';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { getCurrentIsoDate } from '@/utils/dateUtils';
import { exists, isValidId } from '@/utils/utils';

import { IManagementActivityEditFormProps } from './ManagementActivityEditForm';
import { ManagementActivityFormModel } from './models';

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
  const pathGenerator = usePathGenerator();
  const [show, setShow] = useState(true);
  const [initialValues, setInitialValues] = useState<ManagementActivityFormModel | null>(null);
  const {
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: isContactLoading,
  } = useActivityContactRetriever();

  const {
    getManagementActivity: { execute: getManagementActivity, loading: getActivityLoading },
    addManagementActivity: { execute: addManagementActivity, loading: addActivityLoading },
    updateManagementActivity: { execute: updateManagementActivity, loading: updateActivityLoading },
  } = useManagementActivityRepository();

  const { file, fileLoading, setStaleLastUpdatedBy } = useContext(SideBarContext);

  const castedFile = file as unknown as ApiGen_Concepts_ManagementFile;

  // Load the activity
  const fetchActivity = useCallback(async () => {
    if (isValidId(activityId)) {
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

        setInitialValues(
          ManagementActivityFormModel.fromApi(retrieved, castedFile?.fileProperties),
        );
      }
    } else {
      // Create activity flow
      const defaultModel = new ManagementActivityFormModel(null, managementFileId);
      defaultModel.activityStatusCode = 'NOTSTARTED';
      defaultModel.requestedDate = getCurrentIsoDate();
      defaultModel.selectedProperties = (castedFile?.fileProperties ?? [])
        .filter(x => x.isActive !== false)
        .map(x => {
          return {
            id: x.id,
            fileId: castedFile.id,
            propertyName: x.propertyName,
            location: x.location,
            displayOrder: x.displayOrder,
            property: x.property,
            propertyId: x.propertyId,
          } as ApiGen_Concepts_FileProperty;
        });

      setInitialValues(defaultModel);
    }
  }, [
    activityId,
    castedFile?.fileProperties,
    castedFile?.id,
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    getManagementActivity,
    managementFileId,
  ]);

  const gstConstant = getSystemConstant(SystemConstants.GST);
  const pstConstant = getSystemConstant(SystemConstants.PST);
  const gstDecimal = exists(gstConstant) ? parseFloat(gstConstant.value) * 0.01 : 0;
  const pstDecimal = exists(pstConstant) ? parseFloat(pstConstant.value) * 0.01 : 0;

  const onSave = async (model: ApiGen_Concepts_ManagementActivity) => {
    let result: ApiGen_Concepts_ManagementActivity | undefined = undefined;
    if (isValidId(model.id)) {
      result = await updateManagementActivity(managementFileId, model);
    } else {
      result = await addManagementActivity(managementFileId, model);
    }

    if (exists(result) && isValidId(managementFileId)) {
      setStaleLastUpdatedBy(true);
      pathGenerator.showDetail('management', managementFileId, 'activities', result.id, false);
    }
  };

  const onCancelClick = () => {
    if (isValidId(managementFileId) && isValidId(activityId)) {
      pathGenerator.showDetail('management', managementFileId, 'activities', activityId, false);
    } else {
      onClose();
    }
  };

  useEffect(() => {
    if (isValidId(managementFileId) && initialValues === null) {
      fetchActivity();
    }
  }, [managementFileId, fetchActivity, initialValues]);

  return exists(castedFile) && isValidId(castedFile?.id) && exists(initialValues) ? (
    <View
      managementFile={castedFile}
      initialValues={initialValues}
      gstConstant={gstDecimal}
      pstConstant={pstDecimal}
      onCancel={onCancelClick}
      loading={
        fileLoading ||
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
