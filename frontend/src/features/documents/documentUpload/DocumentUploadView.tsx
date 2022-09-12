import { Button } from 'components/common/buttons/Button';
import { Select, SelectOption } from 'components/common/form';
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

import { DocumentMetadataForm } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { getDocumentUploadYupSchema } from './DocumentUploadYupSchema';

interface IDocumentUploadViewProps {
  documentTypes: Api_DocumentType[];
  isLoading: boolean;
  mayanMetadata: Api_Storage_DocumentTypeMetadataType[];
  onDocumentTypeChange: (changeEvent: ChangeEvent<HTMLInputElement>) => void;
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

  const initialFormState: DocumentMetadataForm = {
    documentTypeId: '',
    documentStatusCode:
      documentStatusTypes.length > 0 ? documentStatusTypes[0]?.value?.toString() || '' : '',
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
      <Formik<DocumentMetadataForm>
        enableReinitialize
        initialValues={initialFormState}
        validateOnMount={true}
        validationSchema={getDocumentUploadYupSchema(props.mayanMetadata, false)}
        onSubmit={async (values: DocumentMetadataForm, { setSubmitting }) => {
          const { documentStatusCode, documentTypeId, ...rest } = values;
          if (selectedFile !== null) {
            const selectedDocumentType = props.documentTypes.find(
              x => x.id === Number(documentTypeId),
            );
            if (selectedDocumentType !== undefined) {
              var request: Api_UploadRequest = {
                documentStatusCode: documentStatusCode,
                documentType: selectedDocumentType,
                file: selectedFile,
                documentMetadata: Object.values(rest)?.length > 0 ? rest : [],
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
            <SectionField label="Document type" labelWidth="3" className="pb-2">
              <Select
                data-testid="document-type"
                placeholder="Select Document type"
                field="documentTypeId"
                options={documentTypes}
                onChange={props.onDocumentTypeChange}
              />
            </SectionField>
            <SectionField label={'Choose document to upload'} labelWidth="12" className="mb-4">
              <div className="pt-2">
                <input
                  data-testid="upload-input"
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
                <DocumentMetadataView
                  mayanMetadata={props.mayanMetadata}
                  formikProps={formikProps}
                  edit={false}
                ></DocumentMetadataView>
              </StyledScrollable>
            </StyledGreySection>
            <Row className="justify-content-end pt-4">
              <Col xs="auto">
                <Button
                  data-testid="cancel"
                  variant="secondary"
                  type="button"
                  onClick={props.onCancel}
                >
                  Cancel
                </Button>
              </Col>
              <Col xs="auto">
                <Button
                  data-testid="save"
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
