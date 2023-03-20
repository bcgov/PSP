import { FileTypes } from 'constants/fileTypes';
import { IActivityFilter } from 'interfaces/IActivityResults';
import { Api_FileForm } from 'models/api/Form';
import React from 'react';

import { useFormRepository } from '../hooks/useFormRepository';
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
  } = useFormRepository();

  const saveForm = (formTypeId: string) => {
    const fileForm: Api_FileForm = {
      fileId: fileId,
      formTypeCode: {
        id: formTypeId,
      },
    };
    addForm(FileTypes.Acquisition, fileForm);
  };

  return <View saveForm={saveForm} />;
};

export default FormListViewContainer;
