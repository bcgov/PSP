import { FieldArray, Formik, FormikProps } from 'formik';

import { SelectOption } from '@/components/common/form';
import FileDragAndDrop from '@/components/common/form/FileDragAndDrop';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SectionField } from '@/components/common/Section/SectionField';
import ValidDocumentExtensions from '@/constants/ValidDocumentExtensions';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Mayan_QueryResponse } from '@/models/api/generated/ApiGen_Mayan_QueryResponse';
import { ApiGen_Requests_DocumentUploadRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRequest';
import { ApiGen_Requests_ExternalResponse } from '@/models/api/generated/ApiGen_Requests_ExternalResponse';

import { BatchUploadFormModel, DocumentUploadFormData } from '../ComposedDocument';
import { getDocumentMetadataYupSchema } from '../DocumentMetadataYupSchema';
import SelectedDocumentRow from './SelectedDocumentRow';

interface IDocumentUploadFormProps {
  isLoading: boolean;
  formikRef: React.RefObject<FormikProps<BatchUploadFormModel>>;
  initialDocumentType: string;
  documentTypes: ApiGen_Concepts_DocumentType[];
  documentStatusOptions: SelectOption[];
  mayanMetadataTypes: ApiGen_Mayan_DocumentTypeMetadataType[];
  retrieveDocumentTypeMetadata: (
    mayanDocumentId: number,
  ) => Promise<
    ApiGen_Requests_ExternalResponse<
      ApiGen_Mayan_QueryResponse<ApiGen_Mayan_DocumentTypeMetadataType>
    >
  >;
  // onDocumentTypeChange: (changeEvent: ChangeEvent<HTMLInputElement>) => void;
  onDocumentsSelected: () => void;
  onUploadDocument: (uploadRequest: ApiGen_Requests_DocumentUploadRequest) => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to upload multiple documents. Can be embedded as a widget.
 */
const DocumentUploadForm: React.FunctionComponent<IDocumentUploadFormProps> = ({
  isLoading,
  formikRef,
  initialDocumentType,
  documentTypes,
  documentStatusOptions,
  mayanMetadataTypes,
  retrieveDocumentTypeMetadata,
  onDocumentsSelected,
  // onDocumentTypeChange,
  onUploadDocument,
  onCancel,
}) => {
  // const [selectedFiles, setSelectedFiles] = useState<File[]>([]);

  // const documentTypeOptions = props.documentTypes.map<SelectOption>(x => {
  //   return { label: x.documentTypeDescription || '', value: x.id?.toString() || '' };
  // });

  // useEffect(() => {
  //   const isTypeDirty =
  //     documentTypes.find(x => x.id?.toString() === initialDocumentType) !== undefined;
  //   if (isTypeDirty) {
  //     // forces formik to flag the change as dirty
  //     formikRef.current?.setFieldValue('isDocumentTypeChanged', isTypeDirty);
  //   }
  // }, [formikRef, documentTypes, initialDocumentType, mayanMetadataTypes]);

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
    onDocumentsSelected();
  };

  return (
    <>
      <LoadingBackdrop show={isLoading} />
      <Formik<BatchUploadFormModel>
        innerRef={formikRef}
        enableReinitialize
        initialValues={new BatchUploadFormModel()}
        validateOnMount={true}
        validationSchema={getDocumentMetadataYupSchema(mayanMetadataTypes)}
        onSubmit={async (values: BatchUploadFormModel, { setSubmitting }) => {
          // TODO: implement batch upload calls
          // if (selectedFiles !== null) {
          //   const selectedDocumentType = props.documentTypes.find(
          //     x => x.id === Number(values.documentTypeId),
          //   );
          //   if (selectedDocumentType !== undefined) {
          //     const request = values.toRequestApi(selectedFiles, selectedDocumentType);
          //     await props.onUploadDocument(request);
          //     setSubmitting(false);
          //   } else {
          //     console.error('Selected document type is not valid');
          //   }
          // }
        }}
      >
        {formikProps => (
          <FieldArray name="documents">
            {({ push, remove }) => (
              <>
                <SectionField
                  label="Choose a max of 10 files to attach at the time"
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
                {formikProps.values.documents.map((formDocument, index) => (
                  <SelectedDocumentRow
                    key={`document-${formDocument.documentTypeId || 'DOC_ID'}-${index}`}
                    formikProps={formikProps}
                    namespace={`documents.${index}`}
                    index={index}
                    document={formDocument}
                    documentTypes={documentTypes}
                    documentStatusOptions={documentStatusOptions}
                    retrieveDocumentTypeMetadata={retrieveDocumentTypeMetadata}
                    onRemove={remove}
                  ></SelectedDocumentRow>
                ))}
              </>
            )}
          </FieldArray>
        )}
      </Formik>
    </>
  );
};

export default DocumentUploadForm;
