import { FaEdit } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { StyledH3, StyledNoData, StyledScrollable } from '../commonStyles';
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

  const documentTypeLabel =
    props.document.pimsDocumentRelationship?.document?.documentType?.documentTypeDescription;

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
            {hasClaim(Claims.DOCUMENT_EDIT) && (
              <RightFlexDiv>
                <LinkButton
                  className="d-inline-block"
                  onClick={() => {
                    props.setIsEditable(true);
                  }}
                >
                  <FaEdit />
                </LinkButton>
              </RightFlexDiv>
            )}
            <SectionField
              data-testid="document-type"
              label="Document type"
              labelWidth="4"
              className="pb-2"
            >
              {documentTypeLabel}
            </SectionField>
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
                  key={`document-${value.document?.id || 'DOC_ID'}-metadata-${value.id}`}
                  label={value.metadata_type?.label || ''}
                >
                  {value.value}
                </SectionField>
              ))}
            </StyledScrollable>
          </Section>
        </>
      )}
    </StyledContainer>
  );
};

const RightFlexDiv = styled.div`
  display: flex;
  flex-direction: row-reverse;
`;
