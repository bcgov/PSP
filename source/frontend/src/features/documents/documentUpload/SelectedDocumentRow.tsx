import clsx from 'classnames';
import { FormikErrors, FormikProps, getIn } from 'formik';
import { truncate } from 'lodash';
import { ChangeEvent, useCallback, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTimesCircle, FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledRemoveIconButton } from '@/components/common/buttons/RemoveButton';
import { SelectOption } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { exists } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import { StyledScrollable } from '../commonStyles';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { BatchUploadFormModel, DocumentUploadFormData } from '../models';
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
  const [replacingFile, setReplacingFile] = useState<boolean>(false);

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

  const onConfirmDocumentReplace = (file: File) => {
    document.setFile(file);
    setReplacingFile(!replacingFile);
  };

  const documentErrors = errors;
  const fileError =
    typeof documentErrors !== 'string' && exists(documentErrors?.file)
      ? documentErrors.file
      : undefined;

  if (exists(fileError) && typeof fileError === 'string') {
    return (
      <Section
        header={
          <div style={{ fontSize: '1.6rem' }}>
            <Row className={clsx('no-gutters')}>
              <Col data-testid={`document[${index}]-error`}>
                <span>File {index + 1}:</span>
                <StyledErrorDiv>
                  <span className="ml-2">{truncate(document.file.name, { length: 50 })}</span>
                  <FaTimesCircle className="ml-2" size="1.6rem" />
                </StyledErrorDiv>
              </Col>
              <Col xs="auto" className="p-0 m-0">
                <StyledRemoveIconButton
                  id={withNameSpace(namespace, 'document-delete')}
                  data-testid={withNameSpace(namespace, 'document-delete')}
                  onClick={() => onRemove(index)}
                  title="Delete document"
                >
                  <FaTrash size="1.6rem" />
                </StyledRemoveIconButton>
              </Col>
            </Row>

            <StyledErrorDiv
              className={clsx('ml-0 pb-1')}
              data-testid={`document[${index}]-error-message`}
            >
              {fileError}
            </StyledErrorDiv>
          </div>
        }
        isCollapsable={false}
        isStyledHeader
        noPadding
      ></Section>
    );
  }

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
          replacingFile={replacingFile}
          toggleReplacingFile={() => setReplacingFile(!replacingFile)}
          onConfirmDocumentReplace={onConfirmDocumentReplace}
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

const StyledErrorDiv = styled.div`
  display: inline-block;
  color: ${props => props.theme.bcTokens.iconsColorDanger};
`;
