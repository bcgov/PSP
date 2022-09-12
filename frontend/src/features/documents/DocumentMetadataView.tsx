import { Input } from 'components/common/form';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps } from 'formik';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { withNameSpace } from 'utils/formUtils';

import { StyledNoData } from './commonStyles';
import { DocumentUpdateFormData, DocumentUploadFormData } from './ComposedDocument';

export interface IDocumentMetadataViewProps {
  mayanMetadata: Api_Storage_DocumentTypeMetadataType[];
  formikProps: FormikProps<DocumentUploadFormData> | FormikProps<DocumentUpdateFormData>;
}

export const DocumentMetadataView: React.FunctionComponent<IDocumentMetadataViewProps> = props => {
  return (
    <>
      {props.mayanMetadata.length === 0 && <StyledNoData>No additional data</StyledNoData>}
      {props.mayanMetadata.map(meta => (
        <SectionField
          labelWidth="4"
          key={`document-metadata-${meta.metadata_type?.name}`}
          label={meta.metadata_type?.label || ''}
          required={meta.required === true}
        >
          <Input
            data-testid={`metadata-input-${meta.metadata_type?.name}` || ''}
            field={withNameSpace('documentMetadata', meta.metadata_type?.id || '')}
            required={meta.required === true}
          />
        </SectionField>
      ))}
      <div style={{ color: 'red' }}>
        {Object.values(props.formikProps.errors).length > 0 && <>Mandatory fields are required.</>}
      </div>
    </>
  );
};
