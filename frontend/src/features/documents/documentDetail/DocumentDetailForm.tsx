import { Button } from 'components/common/buttons/Button';
import { Select } from 'components/common/form';
import TooltipIcon from 'components/common/TooltipIcon';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import * as API from 'constants/API';
import Claims from 'constants/claims';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_DocumentUpdateRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { Col, Row } from 'react-bootstrap';

import {
  StyledGreySection,
  StyledH2,
  StyledH3,
  StyledHeader,
  StyledScrollable,
} from '../commonStyles';
import { ComposedDocument, DocumentUpdateFormData } from '../ComposedDocument';
import { DocumentMetadataView } from '../DocumentMetadataView';
import { StyledContainer } from '../list/styles';
import DocumentDetailHeader from './DocumentDetailHeader';

export interface IDocumentDetailFormProps {
  document: ComposedDocument;
  isLoading: boolean;
  mayanMetadataTypes: Api_Storage_DocumentTypeMetadataType[];
  onUpdate: (updateRequest: Api_DocumentUpdateRequest) => void;
  onCancel: () => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
export const DocumentDetailForm: React.FunctionComponent<IDocumentDetailFormProps> = props => {
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
          <StyledGreySection>
            <Row className="pb-3">
              <Col className="text-left">
                <StyledHeader>
                  <StyledH2>Document information</StyledH2>
                  <TooltipIcon
                    toolTipId={'documentInfoToolTip'}
                    className={'documentInfoToolTip'}
                    toolTip="Information you provided here will be searchable"
                  ></TooltipIcon>
                </StyledHeader>
              </Col>
            </Row>

            <StyledScrollable>
              <Formik<DocumentUpdateFormData>
                initialValues={initialFormState}
                onSubmit={async (values: DocumentUpdateFormData, { setSubmitting }) => {
                  if (props.document?.pimsDocument?.id && values.documentStatusCode !== undefined) {
                    var request = values.toRequestApi();
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
                    <Row className="justify-content-end pt-4">
                      <Col xs="auto">
                        <Button variant="secondary" type="button" onClick={props.onCancel}>
                          Cancel
                        </Button>
                      </Col>
                      <Col xs="auto">
                        <Button type="submit" onClick={formikProps.submitForm}>
                          Save
                        </Button>
                      </Col>
                    </Row>
                  </>
                )}
              </Formik>
            </StyledScrollable>
          </StyledGreySection>
        </>
      )}
    </StyledContainer>
  );
};
