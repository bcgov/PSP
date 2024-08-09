import clsx from 'classnames';
import { FormikProps } from 'formik';
import truncate from 'lodash/truncate';
import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import { FiCheck } from 'react-icons/fi';
import styled, { useTheme } from 'styled-components';

import { StyledRemoveIconButton } from '@/components/common/buttons';
import { Select, SelectOption } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { exists } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import { BatchUploadFormModel, DocumentUploadFormData } from '../ComposedDocument';

export interface ISelectedDocumentHeaderProps {
  // props
  index: number;
  namespace?: string;
  className?: string;
  'data-testId'?: string;
  formikProps: FormikProps<BatchUploadFormModel>;
  document: DocumentUploadFormData;
  documentTypes: ApiGen_Concepts_DocumentType[];
  documentStatusOptions: SelectOption[];
  // event handlers
  onDocumentTypeChange: (changeEvent: React.ChangeEvent<HTMLInputElement>) => void;
  onRemove: (index: number) => void;
}

export const SelectedDocumentHeader: React.FunctionComponent<ISelectedDocumentHeaderProps> = ({
  index,
  namespace,
  className,
  'data-testId': dataTestId,
  formikProps,
  document,
  documentTypes,
  documentStatusOptions,
  onDocumentTypeChange,
  onRemove,
}) => {
  const theme = useTheme();
  const isMounted = useIsMounted();
  const { setFieldValue } = formikProps;

  const documentTypeOptions = documentTypes.map<SelectOption>(x => {
    return { label: x.documentTypeDescription || '', value: x.id?.toString() || '' };
  });

  // Ensure the drop-down fields are selected when the supplied options have only one item.
  // We do this for CDOGS templates which have a single document type (TEMPLATE)
  useEffect(() => {
    if (isMounted() && exists(documentTypeOptions) && documentTypeOptions.length === 1) {
      const defaultValue = documentTypeOptions[0].value;
      setFieldValue(withNameSpace(namespace, 'documentTypeId'), defaultValue);
      // call onChange event programmatically
      const eventObj = { target: { value: defaultValue } } as React.ChangeEvent<HTMLInputElement>;
      onDocumentTypeChange(eventObj);
    }

    if (isMounted() && exists(documentStatusOptions) && documentStatusOptions.length === 1) {
      setFieldValue(withNameSpace(namespace, 'documentStatusCode'), documentStatusOptions[0].value);
    }
  }, [
    documentStatusOptions,
    documentTypeOptions,
    isMounted,
    namespace,
    onDocumentTypeChange,
    setFieldValue,
  ]);

  // An attached file is required to render this component
  if (!exists(document.file)) {
    return null;
  }

  return (
    <>
      <Row className={clsx('no-gutters', className)}>
        <Col>
          <span>File {index}:</span>
          <span className="ml-4">{truncate(document.file.name, { length: 20 })}</span>
          <FiCheck className="ml-2" size="1.6rem" color={theme.css.uploadFileCheckColor} />
        </Col>
      </Row>
      <StyledRow className={clsx('no-gutters', className)}>
        <Col xs="auto">
          <SectionField label={null} contentWidth="12" required>
            <Select
              data-testid={withNameSpace(namespace, 'document-type')}
              placeholder={documentTypeOptions.length > 1 ? 'Select Document type' : undefined}
              field={withNameSpace(namespace, 'documentTypeId')}
              options={documentTypeOptions}
              onChange={onDocumentTypeChange}
              disabled={documentTypeOptions.length === 1}
            />
          </SectionField>
        </Col>
        <Col xs="auto">
          <SectionField label={null} contentWidth="12">
            <Select
              field={withNameSpace(namespace, 'documentStatusCode')}
              options={documentStatusOptions}
              disabled={documentStatusOptions.length === 1}
            />
          </SectionField>
        </Col>
        <Col>
          <StyledRemoveIconButton
            id={withNameSpace(namespace, 'document-delete')}
            data-testid={dataTestId ?? withNameSpace(namespace, 'document-delete')}
            onClick={() => onRemove(index)}
            title="Delete document"
          >
            <FaTrash size="2rem" />
          </StyledRemoveIconButton>
        </Col>
      </StyledRow>
    </>
  );
};

const StyledRow = styled(Row)`
  justify-content: space-between;
  align-items: end;
  min-height: 4.5rem;
`;
