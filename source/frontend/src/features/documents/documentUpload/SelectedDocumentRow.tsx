import { FormikErrors, FormikProps, getIn } from 'formik';
import { ChangeEvent, useCallback } from 'react';

import { SelectOption } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { exists } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import { StyledScrollable } from '../commonStyles';
import { BatchUploadFormModel, DocumentUploadFormData } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { SelectedDocumentHeader } from './SelectedDocumentHeader';

export interface ISelectedDocumentRowProps {
  index: number;
  namespace?: string;
  formikProps: FormikProps<BatchUploadFormModel>;
  document: DocumentUploadFormData;
  documentTypes: ApiGen_Concepts_DocumentType[];
  documentStatusOptions: SelectOption[];
  getDocumentMetadata: (
    documentType?: ApiGen_Concepts_DocumentType,
  ) => Promise<ApiGen_Mayan_DocumentTypeMetadataType[]>;
  onRemove: (index: number) => void;
}

export const SelectedDocumentRow: React.FunctionComponent<ISelectedDocumentRowProps> = ({
  index,
  namespace,
  formikProps,
  document,
  documentTypes,
  documentStatusOptions,
  getDocumentMetadata,
  onRemove,
}) => {
  const { setFieldValue } = formikProps;
  const errors: FormikErrors<DocumentUploadFormData> =
    getIn(formikProps.errors, namespace ?? '') || {};

  const updateDocumentType = useCallback(
    async (documentType?: ApiGen_Concepts_DocumentType) => {
      if (!exists(documentType)) {
        return;
      }
      if (documentType.mayanId) {
        const retrievedMetadata = await getDocumentMetadata(documentType);
        if (exists(retrievedMetadata)) {
          document.setMayanMetadata(retrievedMetadata);
          setFieldValue(withNameSpace(namespace, 'mayanMetadata'), retrievedMetadata);
        }
      } else {
        document.setMayanMetadata([]);
        setFieldValue(withNameSpace(namespace, 'mayanMetadata'), []);
      }
    },
    [document, getDocumentMetadata, namespace, setFieldValue],
  );

  const onDocumentTypeChange = useCallback(
    async (changeEvent: ChangeEvent<HTMLInputElement>) => {
      const documentTypeId = Number(changeEvent.target.value);
      await updateDocumentType(documentTypes.find(x => x.id === documentTypeId));
    },
    [documentTypes, updateDocumentType],
  );

  return (
    <Section
      header={
        <SelectedDocumentHeader
          index={index}
          namespace={namespace}
          formikProps={formikProps}
          document={document}
          documentTypes={documentTypes}
          documentStatusOptions={documentStatusOptions}
          onRemove={onRemove}
          onDocumentTypeChange={onDocumentTypeChange}
        />
      }
      isStyledHeader
      isCollapsable
      initiallyExpanded={false}
      noPadding
    >
      <StyledScrollable>
        <DocumentMetadataView
          namespace={namespace}
          mayanMetadata={document.mayanMetadata ?? []}
          values={document}
          errors={errors}
        />
      </StyledScrollable>
    </Section>
  );
};

export default SelectedDocumentRow;
