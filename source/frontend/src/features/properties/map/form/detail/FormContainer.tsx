import * as React from 'react';
import { useCallback, useContext, useEffect } from 'react';

import { FileTypes } from '@/constants/fileTypes';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useFormDocumentRepository } from '@/hooks/repositories/useFormDocumentRepository';

import { formContent } from '../../shared/content/formContent';
import { FormTemplateTypes } from '../../shared/content/models';
import { IFormViewProps } from './FormView';

export interface IFormContainerProps {
  formFileId?: number;
  fileType: FileTypes;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IFormViewProps>>;
}

export const FormContainer: React.FunctionComponent<
  React.PropsWithChildren<IFormContainerProps>
> = ({ fileType, formFileId, onClose, View }) => {
  const {
    getFileForm: { execute: getForm, response, loading },
  } = useFormDocumentRepository();
  const { file, fileLoading } = useContext(SideBarContext);

  const fetchForm = useCallback(async () => {
    if (!!formFileId) {
      return await getForm(fileType, formFileId);
    }
  }, [formFileId, getForm, fileType]);
  useEffect(() => {
    fetchForm();
  }, [fetchForm]);

  if (!!file && file?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  const currentFormContent = response?.formDocumentType?.formTypeCode
    ? formContent.get(response?.formDocumentType?.formTypeCode as FormTemplateTypes)
    : undefined;

  return !!file?.id ? (
    <>
      <View
        loading={loading}
        formFile={response}
        onClose={onClose}
        formContent={currentFormContent}
      ></View>
    </>
  ) : null;
};

export default FormContainer;
