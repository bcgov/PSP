import { Input } from 'components/common/form';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps } from 'formik';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import styled from 'styled-components';

import { DocumentMetadataForm } from './ComposedDocument';

export interface IDocumentMetadataViewProps {
  mayanMetadata?: Api_Storage_DocumentTypeMetadataType[];
  formikProps: FormikProps<DocumentMetadataForm>;
  edit: boolean;
}

export const DocumentMetadataView: React.FunctionComponent<IDocumentMetadataViewProps> = props => {
  return (
    <>
      {props.mayanMetadata?.length === 0 && <StyledNoData>No additional data</StyledNoData>}
      {props.mayanMetadata?.map(value => (
        <SectionField
          labelWidth="4"
          key={
            props.edit
              ? `document-metadata-${value.id}`
              : `document-${value.metadata_type?.id}-metadata-${value.id}`
          }
          label={value.metadata_type?.label || ''}
          required={value.required === true}
        >
          <Input
            field={(props.edit ? value?.id?.toString() : value.metadata_type?.id?.toString()) || ''}
            required={value.required === true}
            defaultValue={value.value}
          />
        </SectionField>
      ))}
      <div style={{ color: 'red' }}>
        {Object.values(props.formikProps.errors).length > 0 && <>Mandatory fields are required.</>}
      </div>
    </>
  );
};

const StyledNoData = styled.div`
  text-align: center;
  font-style: italic;
`;
