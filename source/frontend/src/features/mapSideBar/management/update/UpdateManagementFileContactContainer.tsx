import { FormikProps } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';

import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { isValidId } from '@/utils/utils';

import { ManagementFileContactFormModel } from '../models/ManagementFileContactFormModel';
import { IManagementFileContactEditFormProps } from './forms/ManagementFileContactEditForm';

interface IUpdateManagementFileContactContainerProps {
  managementFileId: number;
  contactId: number | null | undefined;
  onSuccess: () => void;
  View: React.ForwardRefExoticComponent<
    IManagementFileContactEditFormProps & React.RefAttributes<FormikProps<any>>
  >;
}

export const UpdateManagementFileContactContainer = React.forwardRef<
  FormikProps<any>,
  IUpdateManagementFileContactContainerProps
>((props, formikRef) => {
  const View = props.View;
  const [managementFileContact, setManagementFileContact] =
    useState<ManagementFileContactFormModel>(null);

  const {
    getManagementFileContact: { execute: getContact, loading: loadingGet },
    postManagementFileContact: { execute: createContact, loading: loadingCreate },
    putManagementContact: { execute: updateContact, loading: loadingUpdate },
  } = useManagementFileRepository();

  const handleSave = async (model: ApiGen_Concepts_ManagementFileContact) => {
    let result = undefined;
    if (isValidId(model.id)) {
      result = await updateContact(props.managementFileId, model.id, model);
    } else {
      result = await createContact(props.managementFileId, model);
    }
    if (isValidId(result?.id)) {
      props.onSuccess();
    }
  };

  const fetchContact = useCallback(async () => {
    let contactModel: ManagementFileContactFormModel;
    if (isValidId(props.contactId)) {
      const response = await getContact(props.managementFileId, props.contactId);
      if (response) {
        contactModel = ManagementFileContactFormModel.fromApi(response);
      }
    } else {
      contactModel = new ManagementFileContactFormModel(0, props.managementFileId, 0);
    }
    setManagementFileContact(contactModel);
  }, [getContact, props.contactId, props.managementFileId]);

  useEffect(() => {
    fetchContact();
  }, [fetchContact]);

  return (
    <View
      isLoading={loadingGet || loadingUpdate || loadingCreate}
      managementFileContact={managementFileContact}
      ref={formikRef}
      onSave={handleSave}
    />
  );
});

export default UpdateManagementFileContactContainer;
