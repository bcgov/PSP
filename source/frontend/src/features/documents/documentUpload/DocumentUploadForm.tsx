import { Button } from 'components/common/buttons/Button';
import { Select, SelectOption } from 'components/common/form';
import TooltipIcon from 'components/common/TooltipIcon';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import * as API from 'constants/API';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_DocumentType, Api_DocumentUploadRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { ChangeEvent, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { StyledGreySection, StyledH2, StyledH3, StyledScrollable } from '../commonStyles';
import { DocumentUploadFormData } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { getDocumentMetadataYupSchema } from '../DocumentMetadataYupSchema';
import { StyledContainer } from '../list/styles';

interface IDocumentUploadFormProps {
  isLoading: boolean;
  formikRef: React.RefObject<FormikProps<DocumentUploadFormData>>;
  initialDocumentType: string;
  documentTypes: Api_DocumentType[];
  mayanMetadataTypes: Api_Storage_DocumentTypeMetadataType[];
  onDocumentTypeChange: (changeEvent: ChangeEvent<HTMLInputElement>) => void;
  onUploadDocument: (uploadRequest: Api_DocumentUploadRequest) => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
const DocumentUploadForm: React.FunctionComponent<IDocumentUploadFormProps> = props => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
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
        // forces formik to flag the change as dirty
        props.formikRef.current?.setFieldValue('fileSet', true);
      }
    }
  };

  const initialFormData = new DocumentUploadFormData(
    documentStatusTypes[0]?.value?.toString(),
    props.initialDocumentType,
    props.mayanMetadataTypes,
  );
  useEffect(() => {
    const isTypeDirty =
      props.documentTypes.find(x => x.id?.toString() === props.initialDocumentType) !== undefined;
    if (isTypeDirty) {
      // forces formik to flag the change as dirty
      props.formikRef.current?.setFieldValue('isDocumentTypeChanged', isTypeDirty);
    }
  }, [props.formikRef, props.documentTypes, props.initialDocumentType, props.mayanMetadataTypes]);

  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      <Formik<DocumentUploadFormData>
        innerRef={props.formikRef}
        enableReinitialize
        initialValues={initialFormData}
        validateOnMount={true}
        validationSchema={getDocumentMetadataYupSchema(props.mayanMetadataTypes)}
        onSubmit={async (values: DocumentUploadFormData, { setSubmitting }) => {
          if (selectedFile !== null) {
            const selectedDocumentType = props.documentTypes.find(
              x => x.id === Number(values.documentTypeId),
            );
            if (selectedDocumentType !== undefined) {
              var request = values.toRequestApi(selectedFile, selectedDocumentType);

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
            <SectionField label="Choose document to upload" labelWidth="12" className="mb-4">
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
                  mayanMetadata={props.mayanMetadataTypes}
                  formikProps={formikProps}
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

export default DocumentUploadForm;
