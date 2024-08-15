import { FieldArray, FormikErrors } from 'formik';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { withNameSpace } from '@/utils/formUtils';

import { StyledNoData } from './commonStyles';
import { DocumentUpdateFormData, DocumentUploadFormData } from './ComposedDocument';

export interface IDocumentMetadataViewProps {
  namespace?: string;
  mayanMetadata: ApiGen_Mayan_DocumentTypeMetadataType[];
  values: DocumentUploadFormData | DocumentUpdateFormData;
  errors: FormikErrors<DocumentUploadFormData> | FormikErrors<DocumentUpdateFormData>;
}

export const DocumentMetadataView: React.FunctionComponent<IDocumentMetadataViewProps> = ({
  namespace,
  mayanMetadata,
  values,
  errors,
}) => {
  return (
    <>
      {mayanMetadata.length === 0 && <StyledNoData>No additional data</StyledNoData>}

      <FieldArray
        name="documentMetadata"
        render={() => (
          <>
            {mayanMetadata.map(meta => (
              <SectionField
                labelWidth="4"
                key={withNameSpace(namespace, `document-metadata-${meta.metadata_type?.name}`)}
                label={meta.metadata_type?.label || ''}
                required={meta.required === true}
              >
                <Input
                  data-testid={withNameSpace(
                    namespace,
                    `metadata-input-${meta.metadata_type?.name ?? ''}`,
                  )}
                  field={withNameSpace(namespace, `documentMetadata.${meta.metadata_type?.id}`)}
                  required={meta.required === true}
                  value={values.documentMetadata[meta.metadata_type?.id || '']}
                />
              </SectionField>
            ))}

            <div style={{ color: 'red' }}>
              {Object.values(errors).length > 0 && <>Mandatory fields are required.</>}
            </div>
          </>
        )}
      />
    </>
  );
};
