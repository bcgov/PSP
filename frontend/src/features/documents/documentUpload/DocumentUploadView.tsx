import { Input, Select, SelectOption } from 'components/common/form';
import { Scrollable } from 'components/common/Scrollable/Scrollable';
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

import DownloadDocumentButton from '../DownloadDocumentButton';
import { ComposedDocument } from './ComposedDocument';

interface DocumentUploadForm {
  documentTypeId: number;
  documentStatusCode: string;
}

interface IDocumentUploadViewProps {
  documentTypes: Api_DocumentType[];
  //statusTypes: Api_DocumentStatus[];
  isLoading: boolean;
  mayanMetadata: Api_Storage_DocumentTypeMetadataType[];
  onDocumentTypeChange: (param: any) => void;
  onUploadDocument: (uploadRequest: Api_UploadRequest) => void;

  onSave: () => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
const DocumentUploadView: React.FunctionComponent<IDocumentUploadViewProps> = props => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  //const documentStatusOptions = [];
  const documentTypes = props.documentTypes.map<SelectOption>(x => {
    return { label: x.documentType || '', value: x.id?.toString() || '' };
  });

  const { getOptionsByType } = useLookupCodeHelpers();

  const documentStatusTypes = getOptionsByType(API.DOCUMENT_STATUS_TYPES);

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
        initialValues={{ documentTypeId: 1, documentStatusCode: '' }}
        onSubmit={async (values: DocumentUploadForm, { setSubmitting }) => {
          /*const researchFile: Api_ResearchFileProperty = values.toApi();
        await savePropertyFile(researchFile);*/
          if (selectedFile !== null) {
            /*const formData = new FormData();
            formData.append('documentType', values.documentTypeId.toString());
            formData.append('file', selectedFile);*/

            var documentType = props.documentTypes.find(x => x.id === values.documentTypeId);
            if (documentType !== undefined) {
              var request: Api_UploadRequest = {
                documentStatusCode: values.documentStatusCode,
                documentType: documentType,
                file: selectedFile,
              };

              await props.onUploadDocument(request);
            } else {
              console.log('woooops!');
            }

            /*api
              .post(`/documents/`, formData)
              .then(() => {
                alert('File Upload success');
                retrieveDocumentList();
                setSelectedFile(null);
                setSelectedType(1);
              })
              .catch(err => alert('File Upload Error'));*/
          }
        }}
      >
        {formikProps => (
          <>
            <div className="pb-3">
              Choose the document type and select "Browse" to choose the file to upload from your
              computer or network to PIMS.
            </div>
            <SectionField label={'Document type'} labelWidth="4" className="pb-2">
              <Select
                field="documentTypeId"
                options={documentTypes}
                onChange={props.onDocumentTypeChange}
              />
            </SectionField>
            <SectionField label={'Choose document to upload'} labelWidth="12" className="pb-4">
              <input id="uploadInput" type="file" name="documentFile" onChange={handleFileInput} />
            </SectionField>
            <StyledGreySection>
              <Row className="pb-2">
                <Col className="text-left">
                  <StyledH2>Document information</StyledH2>
                </Col>
                <Col xs="2">tooltip</Col>
              </Row>
              <SectionField label="Status" labelWidth="4">
                <Select field="documentStatusCode" options={documentStatusTypes} />
              </SectionField>

              <StyledH3>Details</StyledH3>
              <StyledScrollable>
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
