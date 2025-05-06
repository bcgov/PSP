import { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
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

  const {
    getActivitySubtypes: { execute: getSubtypes, loading: getSubtypesLoading },
    addManagementActivity: { execute: addManagementActivity, loading: addActivityLoading },
  } = useManagementActivityRepository();

  const { file, fileLoading, setStaleLastUpdatedBy } = useContext(SideBarContext);

  if (!isValidId(file?.id) && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  const castedFile = file as unknown as ApiGen_Concepts_ManagementFile;

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

  const gstConstant = getSystemConstant(SystemConstants.GST);
  const pstConstant = getSystemConstant(SystemConstants.PST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) * 0.01 : 0;
  const pstDecimal = pstConstant !== undefined ? parseFloat(pstConstant.value) * 0.01 : 0;

  const onSave = async (model: ApiGen_Concepts_PropertyActivity) => {
    let result: ApiGen_Concepts_PropertyActivity | undefined = undefined;
    if (isValidId(model.id)) {
      // TODO: implement update activity flow
    } else {
      result = await addManagementActivity(managementFileId, model);
    }

    if (exists(result)) {
      setStaleLastUpdatedBy(true);
      history.push(`/mapview/sidebar/management/${managementFileId}/activities/${result.id}`);
    }
  };

  const onCancelClick = () => {
    if (isValidId(activityId)) {
      history.push(`/mapview/sidebar/management/${managementFileId}/activities/${activityId}`);
    } else {
      onClose();
    }
  };

  return exists(castedFile) && isValidId(castedFile?.id) ? (
    <View
      managementFile={castedFile}
      subtypes={subtypes}
      gstConstant={gstDecimal}
      pstConstant={pstDecimal}
      onCancel={onCancelClick}
      loading={getSubtypesLoading || addActivityLoading}
      show={show}
      setShow={setShow}
      onSave={onSave}
      onClose={onClose}
    />
  ) : null;
};
