import clsx from 'classnames';
import { FormikProps } from 'formik';
import truncate from 'lodash/truncate';
import { useEffect, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaCheck, FaTrash } from 'react-icons/fa';
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

  const documentTypeOptions = useMemo(
    () =>
      documentTypes.map<SelectOption>(x => {
        return { label: x.documentTypeDescription || '', value: x.id?.toString() || '' };
      }),
    [documentTypes],
  );

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
      <Row className={clsx('no-gutters', 'pb-3', className)}>
        <Col>
          <span>File {index + 1}:</span>
          <span className="ml-4">{truncate(document.file.name, { length: 50 })}</span>
          <FaCheck className="ml-2" size="1.6rem" color={theme.css.uploadFileCheckColor} />
        </Col>
      </Row>
      <StyledRow className={clsx('ml-0', className)}>
        <Col md="5">
          <SectionField label={null} contentWidth="12" required>
            <Select
              className="mb-0"
              data-testid={withNameSpace(namespace, 'document-type')}
              placeholder={documentTypeOptions.length > 1 ? 'Select Document type' : undefined}
              field={withNameSpace(namespace, 'documentTypeId')}
              options={documentTypeOptions}
              onChange={onDocumentTypeChange}
              disabled={documentTypeOptions.length === 1}
            />
          </SectionField>
        </Col>
        <Col md="auto">
          <SectionField label={null} contentWidth="12">
            <Select
              className="mb-0"
              field={withNameSpace(namespace, 'documentStatusCode')}
              options={documentStatusOptions}
              disabled={documentStatusOptions.length === 1}
            />
          </SectionField>
        </Col>
        <Col></Col>
        <Col xs="auto" className="p-0 m-0">
          <StyledRemoveIconButton
            id={withNameSpace(namespace, 'document-delete')}
            data-testid={dataTestId ?? withNameSpace(namespace, 'document-delete')}
            onClick={() => onRemove(index)}
            title="Delete document"
          >
            <FaTrash size="1.6rem" />
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
