import { FileTypes } from 'constants/fileTypes';
import { useFormDocumentRepository } from 'hooks/repositories/useFormDocumentRepository';
import { IActivityFilter } from 'interfaces/IActivityResults';
import { Api_FormDocumentFile } from 'models/api/FormDocument';
import React from 'react';

import { IFormListViewProps } from './FormListView';

export interface IFormListViewContainerProps {
  fileId: number;
  defaultFilters?: IActivityFilter;
  fileType: FileTypes;
  View: React.FunctionComponent<React.PropsWithChildren<IFormListViewProps>>;
}

/**
 * Page that displays template forms.
 */
export const FormListViewContainer: React.FunctionComponent<
  React.PropsWithChildren<IFormListViewContainerProps>
> = ({ fileId, defaultFilters, fileType, View }: IFormListViewContainerProps) => {
  const {
    addFilesForm: { execute: addForm },
  } = useFormDocumentRepository();

  const saveForm = (formTypeCode: string) => {
    const fileForm: Api_FormDocumentFile = {
      fileId: fileId,
      formDocumentType: {
        formTypeCode: formTypeCode,
        description: '',
        displayOrder: null,
        documentId: null,
      },
    };
    addForm(FileTypes.Acquisition, fileForm);
  };

  return <View saveForm={saveForm} />;
};

export default FormListViewContainer;
