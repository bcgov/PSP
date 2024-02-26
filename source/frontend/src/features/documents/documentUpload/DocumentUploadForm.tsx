import { Formik, FormikProps } from 'formik';
import { ChangeEvent, useEffect, useState } from 'react';

import { Select, SelectOption } from '@/components/common/form';
import FileDragAndDrop from '@/components/common/form/FileDragAndDrop';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import ValidDocumentExtensions from '@/constants/ValidDocumentExtensions';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Requests_DocumentUploadRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRequest';

import { StyledH3, StyledScrollable } from '../commonStyles';
import { DocumentUploadFormData } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { getDocumentMetadataYupSchema } from '../DocumentMetadataYupSchema';

interface IDocumentUploadFormProps {
  isLoading: boolean;
  formikRef: React.RefObject<FormikProps<DocumentUploadFormData>>;
  initialDocumentType: string;
  documentTypes: ApiGen_Concepts_DocumentType[];
  documentStatusOptions: SelectOption[];
  mayanMetadataTypes: ApiGen_Mayan_DocumentTypeMetadataType[];
  onDocumentSelected: () => void;
  onDocumentTypeChange: (changeEvent: ChangeEvent<HTMLInputElement>) => void;
  onUploadDocument: (uploadRequest: ApiGen_Requests_DocumentUploadRequest) => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
const DocumentUploadForm: React.FunctionComponent<
  React.PropsWithChildren<IDocumentUploadFormProps>
> = props => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const documentTypes = props.documentTypes.map<SelectOption>(x => {
    return { label: x.documentTypeDescription || '', value: x.id?.toString() || '' };
  });

  const initialFormData = new DocumentUploadFormData(
    props.documentStatusOptions[0]?.value?.toString(),
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

  const onSelectFile = (selectedFile: File | null) => {
    setSelectedFile(selectedFile);
    // forces formik to flag the change as dirty
    props.formikRef.current?.setFieldValue('isSelectedFile', true);
    props.onDocumentSelected();
  };

  return (
    <>
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
              const request = values.toRequestApi(selectedFile, selectedDocumentType);

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
            <SectionField label="Document type" labelWidth="4" className="pb-2" required>
              <Select
                data-testid="document-type"
                placeholder={documentTypes.length > 1 ? 'Select Document type' : undefined}
                field="documentTypeId"
                options={documentTypes}
                onChange={props.onDocumentTypeChange}
                disabled={documentTypes.length === 1}
              />
            </SectionField>
            <SectionField label="Choose document to upload" labelWidth="12" className="mb-4">
              <div className="pt-2"></div>
              <FileDragAndDrop
                onSelectFile={onSelectFile}
                selectedFile={selectedFile}
                validExtensions={ValidDocumentExtensions}
              />
            </SectionField>
            <Section
              className="pt-4"
              header={
                <>
                  Document Information{' '}
                  <TooltipIcon
                    toolTipId="initiator-tooltip"
                    toolTip="Information you provide here will be searchable"
                  />
                </>
              }
              noPadding
            >
              <SectionField label="Status" labelWidth="4">
                <Select
                  field="documentStatusCode"
                  options={props.documentStatusOptions}
                  disabled={props.documentStatusOptions.length === 1}
                />
              </SectionField>
              <StyledH3>Details</StyledH3>
              <StyledScrollable>
                <DocumentMetadataView
                  mayanMetadata={props.mayanMetadataTypes}
                  formikProps={formikProps}
                ></DocumentMetadataView>
              </StyledScrollable>
              <div className="pt-5 pb-0 mb-0">Do you want to proceed?</div>
            </Section>
          </>
        )}
      </Formik>
    </>
  );
};

export default DocumentUploadForm;
