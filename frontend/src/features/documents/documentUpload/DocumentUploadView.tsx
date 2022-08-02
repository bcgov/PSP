import { Button } from 'components/common/buttons/Button';
import { Input, Select, SelectOption } from 'components/common/form';
import { Scrollable } from 'components/common/Scrollable/Scrollable';
import TooltipIcon from 'components/common/TooltipIcon';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import * as API from 'constants/API';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_DocumentType, Api_UploadRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { ChangeEvent, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface DocumentUploadForm {
  documentTypeId: string;
  documentStatusCode: string;
}

interface IDocumentUploadViewProps {
  documentTypes: Api_DocumentType[];
  isLoading: boolean;
  mayanMetadata: Api_Storage_DocumentTypeMetadataType[];
  onDocumentTypeChange: (param: any) => void;
  onUploadDocument: (uploadRequest: Api_UploadRequest) => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
const DocumentUploadView: React.FunctionComponent<IDocumentUploadViewProps> = props => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const documentTypes = props.documentTypes.map<SelectOption>(x => {
    return { label: x.documentType || '', value: x.id?.toString() || '' };
  });

  const { getOptionsByType } = useLookupCodeHelpers();
  const documentStatusTypes = getOptionsByType(API.DOCUMENT_STATUS_TYPES);

  const initialFormState: DocumentUploadForm = {
    documentTypeId: documentTypes.length > 0 ? documentTypes[0].value.toString() || '' : '',
    documentStatusCode:
      documentStatusTypes.length > 0 ? documentStatusTypes[0].value.toString() || '' : '',
  };

  const handleFileInput = (changeEvent: ChangeEvent<HTMLInputElement>) => {
    // handle validations
    if (changeEvent.target !== null) {
      var target = changeEvent.target;
      if (target.files !== null) {
        setSelectedFile(target.files[0]);
      }
    }
  };

  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      <Formik<DocumentUploadForm>
        enableReinitialize
        initialValues={initialFormState}
        onSubmit={async (values: DocumentUploadForm, { setSubmitting }) => {
          if (selectedFile !== null) {
            const selectedDocumentType = props.documentTypes.find(
              x => x.id === Number(values.documentTypeId),
            );

            if (selectedDocumentType !== undefined) {
              var request: Api_UploadRequest = {
                documentStatusCode: values.documentStatusCode,
                documentType: selectedDocumentType,
                file: selectedFile,
              };

              await props.onUploadDocument(request);
              setSubmitting(false);
            } else {
              console.error('Selected document type is not valid');
            }
          }
        }}
      >
        {formikProps => (
          <>
            <div className="pb-4">
              Choose the document type and select "Browse" to choose the file to upload from your
              computer or network to PIMS.
            </div>
            <SectionField label={'Document type'} labelWidth="3" className="pb-2">
              <Select
                field="documentTypeId"
                options={documentTypes}
                onChange={props.onDocumentTypeChange}
              />
            </SectionField>
            <SectionField label={'Choose document to upload'} labelWidth="12" className="mb-4">
              <div className="pt-2">
                <input
                  id="uploadInput"
                  type="file"
                  name="documentFile"
                  onChange={handleFileInput}
                />
              </div>
            </SectionField>
            <StyledGreySection>
              <Row className="pb-3">
                <Col xs="auto">
                  <StyledH2>Document information</StyledH2>
                </Col>
                <Col xs="auto">
                  <TooltipIcon
                    toolTipId="initiator-tooltip"
                    toolTip="Information you provide here will be searchable"
                  />
                </Col>
              </Row>
              <SectionField label="Status" labelWidth="4">
                <Select field="documentStatusCode" options={documentStatusTypes} />
              </SectionField>

              <StyledH3>Details</StyledH3>
              <StyledScrollable>
                {props.mayanMetadata?.length === 0 && (
                  <StyledNoData>No additional data</StyledNoData>
                )}
                {props.mayanMetadata?.map(value => (
                  <SectionField
                    labelWidth="4"
                    key={`document-${value.metadata_type?.id}-metadata-${value.id}`}
                    label={value.metadata_type?.label || ''}
                    required={value.required === true}
                  >
                    <Input
                      field={value.metadata_type?.name || ''}
                      required={value.required === true}
                    />
                  </SectionField>
                ))}
              </StyledScrollable>
            </StyledGreySection>
            <Row className="justify-content-end pt-4">
              <Col xs="auto">
                <Button variant="secondary" type="button" onClick={props.onCancel}>
                  Cancel
                </Button>
              </Col>
              <Col xs="auto">
                <Button
                  type="submit"
                  onClick={formikProps.submitForm}
                  disabled={selectedFile === null}
                >
                  Save
                </Button>
              </Col>
            </Row>
          </>
        )}
      </Formik>
    </StyledContainer>
  );
};

export default DocumentUploadView;

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
const StyledH3 = styled.h3`
  font-weight: 700;
  font-size: 1.7rem;
  text-align: left;

  margin-top: 0rem;
  padding-top: 0rem;

  margin-bottom: 1rem;
  padding-bottom: 1rem;

  color: ${props => props.theme.css.primaryColor};
  border-bottom: solid 0.1rem ${props => props.theme.css.primaryColor};
`;

const StyledScrollable = styled(Scrollable)`
  overflow-x: hidden;
  max-height: 50rem;
`;

const StyledNoData = styled.div`
  text-align: center;
  font-style: italic;
`;
