import { LinkButton } from 'components/common/buttons';
import { Button } from 'components/common/buttons/Button';
import { Select } from 'components/common/form';
import { Scrollable } from 'components/common/Scrollable/Scrollable';
import TooltipIcon from 'components/common/TooltipIcon';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import * as API from 'constants/API';
import Claims from 'constants/claims';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_DocumentMetadata, Api_DocumentUpdateRequest } from 'models/api/Document';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaEdit } from 'react-icons/fa';
import styled from 'styled-components';

import { ComposedDocument, DocumentMetadataForm } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import DownloadDocumentButton from '../DownloadDocumentButton';

interface IDocumentDetailsViewProps {
  document: ComposedDocument;
  isLoading: boolean;
  onUpdate: (updateRequest: Api_DocumentUpdateRequest) => void;
  onCancel?: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
const DocumentDetailView: React.FunctionComponent<IDocumentDetailsViewProps> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const [editable, setEditable] = useState<boolean>(false);

  const documentTypeLabel = props.document.pimsDocument?.documentType?.documentType;
  const documentFileName = props.document.pimsDocument?.fileName;
  const mayanDocumentId = props.document.pimsDocument?.mayanDocumentId || -1;

  const { getOptionsByType } = useLookupCodeHelpers();
  const documentStatusTypes = getOptionsByType(API.DOCUMENT_STATUS_TYPES);

  let mayanFileId = undefined;
  if (props.document.mayanMetadata !== undefined && props.document.mayanMetadata?.length > 0) {
    const document = props.document.mayanMetadata[0]?.document;
    mayanFileId = document.file_latest.id;
  }

  const initialFormState: DocumentMetadataForm = {
    documentTypeId: '',
    documentStatusCode: props.document.pimsDocument?.statusTypeCode?.id?.toString() || '',
  };
  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      {hasClaim(Claims.DOCUMENT_VIEW) && (
        <>
          <SectionField label="Document type" labelWidth="4" className="pb-2">
            {documentTypeLabel}
          </SectionField>
          <SectionField label={'File name'} labelWidth="4" className="pb-3">
            <Row>
              <Col xs="auto">{documentFileName}</Col>
              <Col xs="auto">
                <DownloadDocumentButton
                  mayanDocumentId={mayanDocumentId}
                  mayanFileId={mayanFileId}
                />
              </Col>
            </Row>
          </SectionField>

          <StyledGreySection>
            <Row className="pb-3">
              <Col className="text-left">
                <StyledHeader>
                  <StyledH2>Document information</StyledH2>
                  <TooltipIcon
                    toolTipId={'documentInfoToolTip'}
                    className={'documentInfoToolTip'}
                    toolTip="Information you provided here will be searchable"
                  ></TooltipIcon>
                </StyledHeader>
              </Col>
              {hasClaim(Claims.DOCUMENT_EDIT) && !editable && (
                <Col xs="2">
                  {' '}
                  <LinkButton
                    onClick={() => {
                      setEditable(true);
                    }}
                  >
                    <FaEdit />
                  </LinkButton>
                </Col>
              )}
            </Row>
            {!editable && (
              <>
                <SectionField label="Status" labelWidth="4">
                  {props.document.pimsDocument?.statusTypeCode?.description}
                </SectionField>
                <StyledScrollable>
                  {(props.document.mayanMetadata ?? []).length === 0 && (
                    <StyledNoData>No additional data</StyledNoData>
                  )}
                  {props.document.mayanMetadata?.map(value => (
                    <SectionField
                      labelWidth="4"
                      key={`document-${value.document.id}-metadata-${value.id}`}
                      label={value.metadata_type.label || ''}
                    >
                      {value.value}
                    </SectionField>
                  ))}
                </StyledScrollable>
              </>
            )}
            {editable && (
              <StyledScrollable>
                <Formik<DocumentMetadataForm>
                  initialValues={initialFormState}
                  onSubmit={async (values: DocumentMetadataForm, { setSubmitting }) => {
                    const { documentStatusCode, documentTypeId, ...rest } = values;
                    if (props.document?.pimsDocument?.id && documentStatusCode !== undefined) {
                      var request: Api_DocumentUpdateRequest = {
                        documentId: props.document.pimsDocument.id,
                        mayanDocumentId: props.document.pimsDocument?.mayanDocumentId,
                        documentStatusCode: documentStatusCode,
                        documentMetadata: [],
                      };
                      for (const [key, value] of Object.entries(rest)) {
                        const metadata: Api_DocumentMetadata = {
                          id: Number(key),
                          metadataTypeId: 0,
                          value: value,
                        };
                        request.documentMetadata.push(metadata);
                      }
                      await props.onUpdate(request);
                      setSubmitting(false);
                    } else {
                      console.error('Selected document type is not valid');
                    }
                  }}
                >
                  {formikProps => (
                    <>
                      <StyledGreySection>
                        <SectionField label="Status" labelWidth="4">
                          <Select field="documentStatusCode" options={documentStatusTypes} />
                        </SectionField>
                        <DocumentMetadataView
                          mayanMetadata={props.document.mayanMetadata}
                          formikProps={formikProps}
                        ></DocumentMetadataView>
                      </StyledGreySection>
                      <Row className="justify-content-end pt-4">
                        <Col xs="auto">
                          <Button variant="secondary" type="button" onClick={props.onCancel}>
                            Cancel
                          </Button>
                        </Col>
                        <Col xs="auto">
                          <Button type="submit" onClick={formikProps.submitForm}>
                            Save
                          </Button>
                        </Col>
                      </Row>
                    </>
                  )}
                </Formik>
              </StyledScrollable>
            )}
          </StyledGreySection>
        </>
      )}
    </StyledContainer>
  );
};

export default DocumentDetailView;

const StyledContainer = styled.div`
  padding: 1rem;
`;

const StyledGreySection = styled.div`
  padding: 1rem;
  background-color: ${({ theme }) => theme.css.filterBackgroundColor};
`;

const StyledH2 = styled.h2`
  font-weight: 700;
  color: ${props => props.theme.css.primaryColor};
`;

const StyledHeader = styled.div`
  text-align: left !important;
  display: flex;
`;
const StyledScrollable = styled(Scrollable)`
  overflow-x: hidden;
  max-height: 50rem;
`;

const StyledNoData = styled.div`
  text-align: center;
  font-style: italic;
`;
