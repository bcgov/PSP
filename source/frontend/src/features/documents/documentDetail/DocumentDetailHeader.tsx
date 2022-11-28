import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Col, Row } from 'react-bootstrap';

import { ComposedDocument } from '../ComposedDocument';
import DownloadDocumentButton from '../DownloadDocumentButton';

interface IDocumentDetailHeaderProps {
  document: ComposedDocument;
}

const DocumentDetailHeader: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailHeaderProps>
> = props => {
  const documentTypeLabel = props.document.pimsDocument?.documentType?.documentType;
  const documentFileName = props.document.pimsDocument?.fileName;
  const mayanDocumentId = props.document.pimsDocument?.mayanDocumentId || -1;

  return (
    <>
      <SectionField
        data-testid="document-type"
        label="Document type"
        labelWidth="4"
        className="pb-2"
      >
        {documentTypeLabel}
      </SectionField>
      <SectionField label={'File name'} labelWidth="4" className="pb-3">
        <Row>
          <Col xs="auto">{documentFileName}</Col>
          <Col xs="auto">
            <DownloadDocumentButton
              mayanDocumentId={mayanDocumentId}
              mayanFileId={props.document.mayanFileId}
            />
          </Col>
        </Row>
      </SectionField>
    </>
  );
};

export default DocumentDetailHeader;
