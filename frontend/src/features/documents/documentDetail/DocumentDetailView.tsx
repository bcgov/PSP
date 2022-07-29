import { Scrollable } from 'components/common/Scrollable/Scrollable';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import DownloadDocumentButton from '../DownloadDocumentButton';
import { ComposedDocument } from './ComposedDocument';

interface IDocumentDetailsViewProps {
  document: ComposedDocument;
  isLoading: boolean;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
const DocumentDetailView: React.FunctionComponent<IDocumentDetailsViewProps> = props => {
  let documentTypeLabel = '';
  let documentFileName = '';

  let mayanDocumentId = -1;
  let mayanFileId = -1;

  if (props.document.mayanMetadata !== undefined && props.document.mayanMetadata?.length > 0) {
    const document = props.document.mayanMetadata[0].document;
    documentTypeLabel = document.document_type.label || '';
    documentFileName = document.label;
    mayanDocumentId = document.id;
    mayanFileId = document.file_latest.id;
  }

  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      <SectionField label={'Document type'} labelWidth="4" className="pb-2">
        {documentTypeLabel}
      </SectionField>
      <SectionField label={'File name'} labelWidth="4" className="pb-3">
        <Row>
          <Col xs="auto">{documentFileName}</Col>
          <Col xs="auto">
            <DownloadDocumentButton mayanDocumentId={mayanDocumentId} mayanFileId={mayanFileId} />
          </Col>
        </Row>
      </SectionField>

      <StyledGreySection>
        <Row className="pb-3">
          <Col className="text-left">
            <StyledH2>Document information</StyledH2>
          </Col>
          <Col xs="2">Edit</Col>
        </Row>
        <SectionField label="Status" labelWidth="4">
          {props.document.pimsDocument?.statusTypeCode?.description}
        </SectionField>

        <StyledH3>Details</StyledH3>
        <StyledScrollable>
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
    </StyledContainer>
  );
};

export default DocumentDetailView;

const StyledContainer = styled.div`
  padding: 1rem;
`;

const StyledGreySection = styled.div`
  padding: 1rem;
  background-color: ${({ theme }) => theme.css.filterBackgroundColor};
`;

const StyledH2 = styled.h2`
  font-weight: 700;
  color: ${props => props.theme.css.primaryColor};
`;
const StyledH3 = styled.h3`
  font-weight: 700;
  font-size: 1.7rem;
  margin-bottom: 1rem;
  text-align: left;
  padding-top: 1rem;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: solid 0.1rem ${props => props.theme.css.primaryColor};
`;

const StyledScrollable = styled(Scrollable)`
  overflow-x: hidden;
  max-height: 50rem;
`;
