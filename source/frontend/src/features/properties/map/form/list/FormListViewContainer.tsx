import orderBy from 'lodash/orderBy';
import React, { useCallback, useContext } from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { FileTypes } from '@/constants/fileTypes';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useFormDocumentRepository } from '@/hooks/repositories/useFormDocumentRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { defaultFormFilter, IFormFilter } from '@/interfaces/IFormResults';
import { Api_FormDocumentFile } from '@/models/api/FormDocument';

import { IFormListViewProps } from './FormListView';

export interface IFormListViewContainerProps {
  fileId: number;
  defaultFilters?: IFormFilter;
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
    getFileForms: { execute: getFileForms, response: forms },
    deleteFileForm: { execute: deleteForm },
  } = useFormDocumentRepository();
  const [formFilter, setFormFilter] = React.useState<IFormFilter>(defaultFormFilter);
  const [sort, setSort] = React.useState<TableSort<Api_FormDocumentFile>>({});
  const { staleFile, setStaleFile } = useContext(SideBarContext);
  const { setModalContent, setDisplayModal } = useModalContext();

  const fetchData = useCallback(async () => {
    await getFileForms(fileType, fileId);
  }, [getFileForms, fileId, fileType]);

  React.useEffect(() => {
    if (forms === undefined || staleFile) {
      fetchData();
    }
  }, [fetchData, staleFile, setStaleFile, forms]);

  const sortedFilteredForms = React.useMemo(() => {
    if (!!forms && forms?.length > 0) {
      let formItems = [...forms];

      if (formFilter) {
        formItems = formItems.filter(form => {
          return (
            !formFilter.formTypeId || form.formDocumentType?.formTypeCode === formFilter.formTypeId
          );
        });
      }
      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = sortFields[0] as keyof Api_FormDocumentFile;
          const sortDirection = sort[keyName];

          let sortBy: string;
          switch (keyName) {
            case 'formDocumentType':
              sortBy = 'formDocumentType.formTypeCode';
              break;

            default:
              sortBy = keyName;
              break;
          }

          return orderBy(formItems, sortBy, sortDirection);
        }
      }
      return formItems;
    }

    return [];
  }, [forms, sort, formFilter]);

  const saveForm = async (formTypeId: string) => {
    const fileForm: Api_FormDocumentFile = {
      id: null,
      fileId: fileId,
      formDocumentType: {
        formTypeCode: formTypeId,
        description: '',
        displayOrder: null,
        documentId: null,
      },
    };
    await addForm(FileTypes.Acquisition, fileForm);
    setStaleFile(true);
  };
  return (
    <View
      saveForm={saveForm}
      sort={sort}
      setSort={setSort}
      formFilter={formFilter}
      setFormFilter={setFormFilter}
      forms={sortedFilteredForms ?? []}
      onDelete={async (fileFormId: number) => {
        setModalContent({
          ...getDeleteModalProps(),
          handleOk: async () => {
            await deleteForm(FileTypes.Acquisition, fileFormId);
            setStaleFile(true);
            setDisplayModal(false);
          },
          handleCancel: () => {
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      }}
    />
  );
};

export default FormListViewContainer;
