import { FormikProps } from 'formik';
import { useCallback, useEffect, useState } from 'react';
import React from 'react';

import { usePropertyContactRepository } from '@/hooks/repositories/usePropertyContactRepository';
import { Api_PropertyContact } from '@/models/api/Property';

import { IPropertyContactEditFormProps } from './PropertyContactEditForm';

interface IPropertyContactEditContainerProps {
  propertyId: number;
  contactId: number;
  onSuccess: () => void;
  View: React.ForwardRefExoticComponent<
    IPropertyContactEditFormProps & React.RefAttributes<FormikProps<any>>
  >;
}

export const PropertyContactEditContainer = React.forwardRef<
  FormikProps<any>,
  IPropertyContactEditContainerProps
>((props, formikRef) => {
  const View = props.View;
  const [propertyContact, setPropertyContact] = useState<Api_PropertyContact>({
    id: 0,
    propertyId: props.propertyId,
    organizationId: null,
    organization: null,
    personId: null,
    person: null,
    primaryContactId: null,
    primaryContact: null,
    purpose: null,
    rowVersion: null,
  });

  const {
    getPropertyContact: { execute: getContact, loading: loadingGet },
    updatePropertyContact: { execute: updateContact, loading: loadingUpdate },
    createPropertyContact: { execute: createContact, loading: loadingCreate },
  } = usePropertyContactRepository();

  const fetchPropertyContacts = useCallback(async () => {
    if (props.contactId !== 0) {
      const propertyContactResponse = await getContact(props.propertyId, props.contactId);
      if (propertyContactResponse) {
        setPropertyContact(propertyContactResponse);
      }
    }
  }, [getContact, props.propertyId, props.contactId]);

  useEffect(() => {
    fetchPropertyContacts();
  }, [fetchPropertyContacts]);

  const onSave = async (model: Api_PropertyContact) => {
    let result = undefined;
    if (model.id !== 0) {
      result = await updateContact(props.propertyId, model.id, model);
    } else {
      result = await createContact(props.propertyId, model);
    }
    if (result?.id) {
      props.onSuccess();
    }
  };

  return (
    <View
      isLoading={loadingGet || loadingUpdate || loadingCreate}
      propertyContact={propertyContact}
      ref={formikRef}
      onSave={onSave}
    />
  );
});
