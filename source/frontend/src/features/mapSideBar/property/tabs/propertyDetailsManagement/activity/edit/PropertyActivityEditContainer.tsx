import { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { usePropertyActivityRepository } from '@/hooks/repositories/usePropertyActivityRepository';
import {
  Api_PropertyActivity,
  Api_PropertyActivityInvolvedParty,
  Api_PropertyActivitySubtype,
  Api_PropertyMinistryContact,
} from '@/models/api/PropertyActivity';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

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
    getActivitySubtypes: { execute: getSubtypes, loading: getSubtypesLoading },
    getActivity: { execute: getActivity, loading: getActivityLoading },
    createActivity: { execute: createActivity, loading: createActivityLoading },
    updateActivity: { execute: updateActivity, loading: updateActivityLoading },
  } = usePropertyActivityRepository();

  const {
    getPersonDetail: { execute: getPerson, loading: loadingPerson },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: loadingOrganization },
  } = useOrganizationRepository();

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

  const fetchMinistryContacs = useCallback(
    async (c: Api_PropertyMinistryContact) => {
      if (c.personId !== undefined && c.personId !== null) {
        const person = await getPerson(c.personId);
        if (person !== undefined) {
          c.person = person;
        }
      }
    },
    [getPerson],
  );

  const fetchPartiesContact = useCallback(
    async (c: Api_PropertyActivityInvolvedParty) => {
      if (c.personId !== undefined && c.personId !== null) {
        const person = await getPerson(c.personId);
        if (person !== undefined) {
          c.person = person;
        }
      }
      if (c.organizationId !== null) {
        const organization = await getOrganization(c.organizationId);
        if (organization !== undefined) {
          c.organization = organization;
        }
      }
    },
    [getPerson, getOrganization],
  );

  const fetchProviderContact = useCallback(
    async (c: Api_PropertyActivity) => {
      if (c.serviceProviderPersonId !== undefined && c.serviceProviderPersonId !== null) {
        const person = await getPerson(c.serviceProviderPersonId);
        if (person !== undefined) {
          c.serviceProviderPerson = person;
        }
      }
      if (c.serviceProviderOrgId !== null) {
        const organization = await getOrganization(c.serviceProviderOrgId);
        if (organization !== undefined) {
          c.serviceProviderOrg = organization;
        }
      }
    },
    [getPerson, getOrganization],
  );

  // Load the activity
  const fetchActivity = useCallback(
    async (propertyId: number, activityId: number) => {
      const retrieved = await getActivity(propertyId, activityId);
      if (retrieved !== undefined) {
        for (let i = 0; i < retrieved.ministryContacts.length; i++) {
          await fetchMinistryContacs(retrieved.ministryContacts[i]);
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
    [fetchMinistryContacs, fetchPartiesContact, fetchProviderContact, getActivity],
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
        loadingPerson ||
        loadingOrganization
      }
      show={show}
      setShow={setShow}
      onSave={onSave}
    />
  );
};
