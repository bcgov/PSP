import { Formik, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { Button } from '@/components/common/buttons/Button';
import { Select } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as API from '@/constants/API';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Requests_DocumentUpdateRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUpdateRequest';

import { StyledH3, StyledScrollable } from '../commonStyles';
import { ComposedDocument, DocumentUpdateFormData } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { getDocumentMetadataYupSchema } from '../DocumentMetadataYupSchema';
import { StyledContainer } from '../list/styles';
import DocumentDetailHeader from './DocumentDetailHeader';

export interface IDocumentDetailFormProps {
  formikRef: React.RefObject<FormikProps<DocumentUpdateFormData>>;
  document: ComposedDocument;
  isLoading: boolean;
  mayanMetadataTypes: ApiGen_Mayan_DocumentTypeMetadataType[];
  onUpdate: (updateRequest: ApiGen_Requests_DocumentUpdateRequest) => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
export const DocumentDetailForm: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailFormProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();

  const { getOptionsByType } = useLookupCodeHelpers();
  const documentStatusTypes = getOptionsByType(API.DOCUMENT_STATUS_TYPES);
  const initialFormState = DocumentUpdateFormData.fromApi(props.document, props.mayanMetadataTypes);

  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      {hasClaim(Claims.DOCUMENT_VIEW) && (
        <>
          <DocumentDetailHeader document={props.document} />

          <Section
            noPadding
            header={
              <>
                Document Information
                <TooltipIcon
                  toolTipId="documentInfoToolTip"
                  innerClassName="documentInfoToolTip"
                  toolTip="Information you provided here will be searchable"
                />{' '}
              </>
            }
          >
            <StyledScrollable>
              <Formik<DocumentUpdateFormData>
                innerRef={props.formikRef}
                initialValues={initialFormState}
                validationSchema={getDocumentMetadataYupSchema(props.mayanMetadataTypes)}
                onSubmit={async (values: DocumentUpdateFormData, { setSubmitting }) => {
                  if (
                    props.document?.pimsDocumentRelationship?.id &&
                    values.documentStatusCode !== undefined
                  ) {
                    const request = values.toRequestApi();
                    await props.onUpdate(request);
                    setSubmitting(false);
                  } else {
                    console.error('Selected document status is not valid');
                  }
                }}
              >
                {formikProps => (
                  <>
                    <SectionField label="Status" labelWidth="4">
                      <Select field="documentStatusCode" options={documentStatusTypes} />
                    </SectionField>
                    <StyledH3>Details</StyledH3>
                    <DocumentMetadataView
                      mayanMetadata={props.mayanMetadataTypes}
                      formikProps={formikProps}
                    />
                    <div className="pt-5">Do you want to proceed?</div>
                    <hr />
                    <Row className="justify-content-end pt-4">
                      <Col xs="auto">
                        <Button
                          variant="secondary"
                          type="button"
                          onClick={props.onCancel}
                          className="px-5"
                        >
                          No
                        </Button>
                      </Col>
                      <Col xs="auto">
                        <Button type="submit" onClick={formikProps.submitForm} className="px-5">
                          Yes
                        </Button>
                      </Col>
                    </Row>
                  </>
                )}
              </Formik>
            </StyledScrollable>
          </Section>
        </>
      )}
    </StyledContainer>
  );
};
