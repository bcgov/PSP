import { FormikErrors, FormikProps, getIn } from 'formik';
import { useCallback } from 'react';

import { SelectOption } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Mayan_QueryResponse } from '@/models/api/generated/ApiGen_Mayan_QueryResponse';
import { ApiGen_Requests_ExternalResponse } from '@/models/api/generated/ApiGen_Requests_ExternalResponse';
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
  retrieveDocumentTypeMetadata: (
    mayanDocumentId: number,
  ) => Promise<
    ApiGen_Requests_ExternalResponse<
      ApiGen_Mayan_QueryResponse<ApiGen_Mayan_DocumentTypeMetadataType>
    >
  >;
  onRemove: (index: number) => void;
}

export const SelectedDocumentRow: React.FunctionComponent<ISelectedDocumentRowProps> = ({
  index,
  namespace,
  formikProps,
  document,
  documentTypes,
  documentStatusOptions,
  retrieveDocumentTypeMetadata,
  onRemove,
}) => {
  const { setFieldValue } = formikProps;
  const errors: FormikErrors<DocumentUploadFormData> =
    getIn(formikProps.errors, namespace ?? '') || {};

  const onDocumentTypeChange = async (changeEvent: React.ChangeEvent<HTMLInputElement>) => {
    const documentTypeId = Number(changeEvent.target.value);
    await updateDocumentType(documentTypes.find(x => x.id === documentTypeId));
  };

  const updateDocumentType = useCallback(
    async (documentType?: ApiGen_Concepts_DocumentType) => {
      if (!exists(documentType)) {
        return;
      }

      if (documentType.mayanId) {
        const results = await retrieveDocumentTypeMetadata(documentType.mayanId);
        if (results?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success) {
          const metadata = results?.payload?.results || [];
          document.setMayanMetadata(metadata);
          setFieldValue(withNameSpace(namespace, 'mayanMetadata'), metadata);
          // setDocumentTypeMetadataTypes(results?.payload?.results || []);
        }
      } else {
        document.setMayanMetadata([]);
        setFieldValue(withNameSpace(namespace, 'mayanMetadata'), []);
        // setDocumentTypeMetadataTypes([]);
      }
    },
    [document, namespace, retrieveDocumentTypeMetadata, setFieldValue],
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
        ></DocumentMetadataView>
      </StyledScrollable>
    </Section>
  );
};

export default SelectedDocumentRow;
