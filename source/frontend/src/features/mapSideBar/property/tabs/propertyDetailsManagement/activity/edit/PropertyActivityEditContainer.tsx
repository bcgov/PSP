import { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useManagementActivityPropertyRepository } from '@/hooks/repositories/useManagementActivityPropertyRepository';
import { ApiGen_CodeTypes_ManagementActivityStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ManagementActivityStatusTypes';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists, isValidId } from '@/utils/utils';

import useActivityContactRetriever from '../hooks';
import { PropertyActivityFormModel } from './models';
import { IPropertyActivityEditFormProps } from './PropertyActivityEditForm';

export interface IPropertyActivityEditContainerProps {
  propertyId: number;
  managementActivityId?: number;
  onClose: () => void;
  viewEnabled: boolean;
  View: React.FunctionComponent<React.PropsWithChildren<IPropertyActivityEditFormProps>>;
}

/**
 * Modal displaying form allowing add/update lease deposits. Save button triggers internal formik validation and submit.
 * @param viewEnabled defines the condition for the PopupTray to show based on the route exact match
 */
export const PropertyActivityEditContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityEditContainerProps>
> = ({ propertyId, managementActivityId, onClose, viewEnabled, View }) => {
  const { getSystemConstant } = useSystemConstants();

  const history = useHistory();
  const [initialValues, setInitialValues] = useState<PropertyActivityFormModel | null>(null);

  const { setStaleLastUpdatedBy } = useContext(SideBarContext);

  const [show, setShow] = useState(true);

  const {
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: isContactLoading,
  } = useActivityContactRetriever();

  const {
    getActivity: { execute: getActivity, loading: getActivityLoading },
    createActivity: { execute: createActivity, loading: createActivityLoading },
    updateActivity: { execute: updateActivity, loading: updateActivityLoading },
  } = useManagementActivityPropertyRepository();

  // Load the activity
  const fetchActivity = useCallback(
    async (propertyId: number, activityId: number) => {
      let formInitialValues: PropertyActivityFormModel;
      const retrieved = await getActivity(propertyId, activityId);
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

        formInitialValues = PropertyActivityFormModel.fromApi(retrieved);
      } else {
        formInitialValues = new PropertyActivityFormModel();
        formInitialValues.activityStatusCode =
          ApiGen_CodeTypes_ManagementActivityStatusTypes.NOTSTARTED;
      }
      setInitialValues(formInitialValues);
    },
    [fetchMinistryContacts, fetchPartiesContact, fetchProviderContact, getActivity],
  );

  useEffect(() => {
    if (isValidId(propertyId) && initialValues === null) {
      fetchActivity(propertyId, managementActivityId);
    }
  }, [propertyId, managementActivityId, fetchActivity, initialValues]);

  const gstConstant = getSystemConstant(SystemConstants.GST);
  const pstConstant = getSystemConstant(SystemConstants.PST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) * 0.01 : 0;
  const pstDecimal = pstConstant !== undefined ? parseFloat(pstConstant.value) * 0.01 : 0;

  const handleSave = async (model: ApiGen_Concepts_ManagementActivity) => {
    let result = undefined;
    if (isValidId(model.id)) {
      result = await updateActivity(propertyId, model);
    } else {
      result = await createActivity(propertyId, model);
    }

    if (exists(result)) {
      setStaleLastUpdatedBy(true);
      history.push(`/mapview/sidebar/property/${propertyId}/management/activity/${result.id}`);
    }
  };

  const onCancelClick = () => {
    if (isValidId(managementActivityId)) {
      history.push(
        `/mapview/sidebar/property/${propertyId}/management/activity/${managementActivityId}`,
      );
    } else {
      onClose();
    }
  };

  return isValidId(propertyId) && exists(initialValues) ? (
    <View
      propertyId={propertyId}
      initialValues={initialValues}
      gstConstant={gstDecimal}
      pstConstant={pstDecimal}
      onCancel={onCancelClick}
      loading={
        getActivityLoading || createActivityLoading || updateActivityLoading || isContactLoading
      }
      show={show && viewEnabled}
      setShow={setShow}
      onSave={handleSave}
      onClose={onClose}
    />
  ) : null;
};
