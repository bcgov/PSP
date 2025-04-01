import { Formik, FormikProps } from 'formik';
import { ChangeEvent, useCallback, useEffect, useState } from 'react';
import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { Select, SelectOption } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as API from '@/constants/API';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Requests_DocumentUpdateRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUpdateRequest';
import { exists } from '@/utils/utils';

import { StyledH3, StyledScrollable } from '../commonStyles';
import { ComposedDocument, DocumentUpdateFormData } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { StyledContainer } from '../list/styles';
import DocumentDetailHeader from './DocumentDetailHeader';
import { DocumentUpdateFormDataYupSchema } from './DocumentUpdateFormDataYupSchema';

export interface IDocumentDetailFormProps {
  formikRef: React.RefObject<FormikProps<DocumentUpdateFormData>>;
  document: ComposedDocument;
  isLoading: boolean;
  documentTypes: ApiGen_Concepts_DocumentType[];
  mayanMetadataTypes: ApiGen_Mayan_DocumentTypeMetadataType[];
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  documentTypeUpdated: boolean;
  onDocumentTypeChange: (changeEvent: React.ChangeEvent<HTMLInputElement>) => void;
  onUpdate: (updateRequest: ApiGen_Requests_DocumentUpdateRequest) => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
export const DocumentDetailForm: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailFormProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();

  const { getOptionsByType } = useLookupCodeHelpers();
  const documentStatusTypes = getOptionsByType(API.DOCUMENT_STATUS_TYPES);
  const initialFormState = DocumentUpdateFormData.fromApi(props.document, props.mayanMetadataTypes);

  const documentTypeOptions = useMemo(
    () =>
      props.documentTypes.map<SelectOption>(x => {
        return { label: x.documentTypeDescription || '', value: x.id?.toString() || '' };
      }),
    [props.documentTypes],
  );

  const [documentTypePurpose, setDocumentTypePurpose] = useState(null);

  const matchDocumentType = useCallback(
    (documentTypeId: number) => {
      const purpose = props.documentTypes.find(x => x.id === documentTypeId)?.documentTypePurpose;

      setDocumentTypePurpose(purpose);
    },
    [props.documentTypes],
  );

  useEffect(() => {
    const documentTypeId = Number(props.formikRef.current?.values?.documentTypeId);
    matchDocumentType(documentTypeId);
  }, [matchDocumentType, props.formikRef]);

  const onDocumentTypeChange = useCallback(
    async (changeEvent: ChangeEvent<HTMLInputElement>) => {
      if (changeEvent.target.value) {
        const documentTypeId = Number(changeEvent.target.value);
        matchDocumentType(documentTypeId);
      }

      props.onDocumentTypeChange(changeEvent);
    },
    [matchDocumentType, props],
  );

  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      {hasClaim(Claims.DOCUMENT_VIEW) && (
        <>
          <DocumentDetailHeader document={props.document} />
          <Section
            noPadding
            header={
              <>
                Document Information
                <TooltipIcon
                  toolTipId="documentInfoToolTip"
                  innerClassName="documentInfoToolTip"
                  toolTip="Information you provided here will be searchable"
                />{' '}
              </>
            }
          >
            <StyledScrollable>
              <Formik<DocumentUpdateFormData>
                innerRef={props.formikRef}
                initialValues={initialFormState}
                validationSchema={DocumentUpdateFormDataYupSchema}
                onSubmit={async (values: DocumentUpdateFormData, { setSubmitting }) => {
                  if (
                    props.document?.pimsDocumentRelationship?.id &&
                    values.documentStatusCode !== undefined
                  ) {
                    const request = DocumentUpdateFormData.toRequestApi(values);
                    await props.onUpdate(request);
                    setSubmitting(false);
                  } else {
                    console.error('Selected document status is not valid');
                  }
                }}
              >
                {formikProps => (
                  <>
                    <SectionField label="Document type" labelWidth={{ xs: 4 }} required>
                      <Select
                        className="mb-0"
                        placeholder={
                          documentTypeOptions.length > 1 ? 'Select Document type' : undefined
                        }
                        field={'documentTypeId'}
                        options={documentTypeOptions}
                        onChange={onDocumentTypeChange}
                        disabled={
                          documentTypeOptions.length === 1 ||
                          props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates
                        }
                      />
                    </SectionField>
                    {exists(documentTypePurpose) && (
                      <SectionField label={null}>
                        <StyledPurposeText>{documentTypePurpose}</StyledPurposeText>
                      </SectionField>
                    )}
                    <SectionField label="Status" labelWidth={{ xs: 4 }}>
                      <Select field="documentStatusCode" options={documentStatusTypes} />
                    </SectionField>
                    {props.documentTypeUpdated && (
                      <StyledDiv>
                        <em>
                          Some associated metadata may be lost if the document type is changed.
                        </em>
                      </StyledDiv>
                    )}
                    <StyledH3>Details</StyledH3>
                    <DocumentMetadataView
                      mayanMetadata={props.mayanMetadataTypes}
                      values={formikProps.values}
                      errors={formikProps.errors}
                    />
                    <div className="pt-5">Do you want to proceed?</div>
                    <hr />
                    <Row className="justify-content-end pt-4">
                      <Col xs="auto">
                        <Button
                          variant="secondary"
                          type="button"
                          onClick={props.onCancel}
                          className="px-5"
                        >
                          No
                        </Button>
                      </Col>
                      <Col xs="auto">
                        <Button type="submit" onClick={formikProps.submitForm} className="px-5">
                          Yes
                        </Button>
                      </Col>
                    </Row>
                  </>
                )}
              </Formik>
            </StyledScrollable>
          </Section>
        </>
      )}
    </StyledContainer>
  );
};

export const StyledDiv = styled.div`
  margin-bottom: 1rem;
`;

const StyledPurposeText = styled.div`
  color: black;
  font-style: italic;
`;
