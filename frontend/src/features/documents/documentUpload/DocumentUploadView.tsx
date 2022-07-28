import { Scrollable } from 'components/common/Scrollable/Scrollable';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Api_DocumentType } from 'models/api/Document';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import DownloadDocumentButton from '../DownloadDocumentButton';
import { ComposedDocument } from './ComposedDocument';

interface IDocumentUploadViewProps {
  documentTypes: Api_DocumentType[];
  isLoading: boolean;
}

/**
 * Component that provides functionality to see document information. Can be embedded as a widget.
 */
const DocumentUploadView: React.FunctionComponent<IDocumentUploadViewProps> = props => {
  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      Choose the document type and select "Browse" to choose the file to upload from your computer
      or network to PIMS.
    </StyledContainer>
  );
};

export default DocumentUploadView;

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
