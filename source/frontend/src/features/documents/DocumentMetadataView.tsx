import { FieldArray, FormikProps } from 'formik';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { Api_Storage_DocumentTypeMetadataType } from '@/models/api/DocumentStorage';

import { StyledNoData } from './commonStyles';
import { DocumentUpdateFormData, DocumentUploadFormData } from './ComposedDocument';

export interface IDocumentMetadataViewProps {
  mayanMetadata: Api_Storage_DocumentTypeMetadataType[];
  formikProps: FormikProps<DocumentUploadFormData> | FormikProps<DocumentUpdateFormData>;
}

export const DocumentMetadataView: React.FunctionComponent<
  React.PropsWithChildren<IDocumentMetadataViewProps>
> = props => {
  return (
    <>
      {props.mayanMetadata.length === 0 && <StyledNoData>No additional data</StyledNoData>}

      <FieldArray
        name="documentMetadata"
        render={() => (
          <>
            {props.mayanMetadata.map(meta => (
              <SectionField
                labelWidth="4"
                key={`document-metadata-${meta.metadata_type?.name}`}
                label={meta.metadata_type?.label || ''}
                required={meta.required === true}
              >
                <Input
                  data-testid={`metadata-input-${meta.metadata_type?.name}` || ''}
                  field={`documentMetadata.${meta.metadata_type?.id}`}
                  required={meta.required === true}
                  value={props.formikProps.values.documentMetadata[meta.metadata_type?.id || '']}
                />
              </SectionField>
            ))}

            <div style={{ color: 'red' }}>
              {Object.values(props.formikProps.errors).length > 0 && (
                <>Mandatory fields are required.</>
              )}
            </div>
          </>
        )}
      />
    </>
  );
};
