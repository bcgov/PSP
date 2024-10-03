import { FieldArray, Formik, FormikProps } from 'formik';
import { Form } from 'react-bootstrap';

import { DisplayError, SelectOption } from '@/components/common/form';
import FileDragAndDrop from '@/components/common/form/FileDragAndDrop';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SectionField } from '@/components/common/Section/SectionField';
import ValidDocumentExtensions from '@/constants/ValidDocumentExtensions';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';

import { StyledScrollable } from '../commonStyles';
import { BatchUploadFormModel, DocumentUploadFormData } from '../ComposedDocument';
import { getDocumentMetadataYupSchema } from '../DocumentMetadataYupSchema';
import SelectedDocumentRow from './SelectedDocumentRow';

interface IDocumentUploadFormProps {
  isLoading: boolean;
  formikRef: React.RefObject<FormikProps<BatchUploadFormModel>>;
  initialDocumentType: string;
  maxDocumentCount: number;
  documentTypes: ApiGen_Concepts_DocumentType[];
  documentStatusOptions: SelectOption[];
  getDocumentMetadata: (
    documentType?: ApiGen_Concepts_DocumentType,
  ) => Promise<ApiGen_Mayan_DocumentTypeMetadataType[]>;
  onDocumentsSelected: (documentCount: number) => void;
  onUploadDocument: (batchRequest: BatchUploadFormModel) => void;
}

/**
 * Component that provides functionality to upload multiple documents. Can be embedded as a widget.
 */
const DocumentUploadForm: React.FunctionComponent<IDocumentUploadFormProps> = ({
  isLoading,
  formikRef,
  initialDocumentType,
  maxDocumentCount,
  documentTypes,
  documentStatusOptions,
  getDocumentMetadata,
  onDocumentsSelected,
  onUploadDocument,
}) => {
  const onSelectFiles = (files: File[], push: (obj: any) => void) => {
    for (const file of files) {
      const formDocument = new DocumentUploadFormData(
        documentStatusOptions[0]?.value?.toString(),
        initialDocumentType ?? '',
        [],
      );
      formDocument.file = file;
      push(formDocument);
    }

    // forces formik to flag the change as dirty
    formikRef.current?.setFieldValue('isSelectedFile', true);
    formikRef.current?.setFieldTouched('documents', true, true);
    onDocumentsSelected(formikRef.current.values.documents.length + files.length);
  };

  return (
    <>
      <LoadingBackdrop show={isLoading} />
      <Formik<BatchUploadFormModel>
        innerRef={formikRef}
        enableReinitialize
        initialValues={new BatchUploadFormModel()}
        validateOnMount={true}
        validationSchema={getDocumentMetadataYupSchema(maxDocumentCount)}
        onSubmit={async (values: BatchUploadFormModel) => {
          onUploadDocument(values);
        }}
      >
        {formikProps => (
          <>
            <FieldArray name="documents">
              {({ push, remove }) => (
                <>
                  <SectionField
                    label={`Choose a max of ${maxDocumentCount} files to attach at the time`}
                    labelWidth="12"
                    className="mb-4"
                  >
                    <div className="pt-2"></div>
                    <FileDragAndDrop
                      onSelectFiles={files => onSelectFiles(files, push)}
                      validExtensions={ValidDocumentExtensions}
                      multiple
                    />
                  </SectionField>
                  <StyledScrollable>
                    {formikProps.values.documents.map((formDocument, index) => (
                      <SectionField
                        key={`document-${formDocument.documentTypeId || 'DOC_ID'}-${index}`}
                        label={null}
                        contentWidth="12"
                      >
                        <SelectedDocumentRow
                          formikProps={formikProps}
                          namespace={`documents.${index}`}
                          index={index}
                          document={formDocument}
                          documentTypes={documentTypes}
                          documentStatusOptions={documentStatusOptions}
                          getDocumentMetadata={getDocumentMetadata}
                          onRemove={(index: number) => {
                            onDocumentsSelected(formikProps.values.documents.length - 1);
                            remove(index);
                          }}
                        />
                      </SectionField>
                    ))}
                  </StyledScrollable>
                </>
              )}
            </FieldArray>
            <div className="pt-4"></div>
            <DisplayError field="documents" />
            {formikProps.values.documents.length > maxDocumentCount && (
              <Form.Control.Feedback type="invalid" className="pt-0">
                {`You have a limit of ${maxDocumentCount} files per time. Some of your files have not been uploaded at this time.`}
              </Form.Control.Feedback>
            )}
            {formikProps.values.documents.length > 0 && (
              <div className="pt-1">
                {`You have attached ${formikProps.values.documents.length} files. Do you want to proceed and save?`}
              </div>
            )}
          </>
        )}
      </Formik>
    </>
  );
};

export default DocumentUploadForm;
