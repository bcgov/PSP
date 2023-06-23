import { Col, Row } from 'react-bootstrap';
import { FaEdit } from 'react-icons/fa';

import { LinkButton } from '@/components/common/buttons';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import {
  StyledGreySection,
  StyledH2,
  StyledH3,
  StyledHeader,
  StyledNoData,
  StyledScrollable,
} from '../commonStyles';
import { ComposedDocument } from '../ComposedDocument';
import { StyledContainer } from '../list/styles';
import DocumentDetailHeader from './DocumentDetailHeader';

export interface IDocumentDetailsViewProps {
  document: ComposedDocument;
  isLoading: boolean;
  setIsEditable: (isEditable: boolean) => void;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
export const DocumentDetailView: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailsViewProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();

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
                    toolTipId="documentInfoToolTip"
                    innerClassName="documentInfoToolTip"
                    toolTip="Information you provided here will be searchable"
                  ></TooltipIcon>
                </StyledHeader>
              </Col>
              {hasClaim(Claims.DOCUMENT_EDIT) && (
                <Col xs="2">
                  {' '}
                  <LinkButton
                    onClick={() => {
                      props.setIsEditable(true);
                    }}
                  >
                    <FaEdit />
                  </LinkButton>
                </Col>
              )}
            </Row>
            <SectionField label="Status" labelWidth="4">
              {props.document.pimsDocumentRelationship?.document?.statusTypeCode?.description}
            </SectionField>
            <StyledH3>Details</StyledH3>
            <StyledScrollable>
              {(props.document.mayanMetadata ?? []).length === 0 && (
                <StyledNoData>No additional data</StyledNoData>
              )}
              {props.document.mayanMetadata?.map(value => (
                <SectionField
                  labelWidth="4"
                  key={`document-${value.document.id}-metadata-${value.id}`}
                  label={value.metadata_type.label || ''}
                >
                  {value.value}
                </SectionField>
              ))}
            </StyledScrollable>
          </StyledGreySection>
        </>
      )}
    </StyledContainer>
  );
};
