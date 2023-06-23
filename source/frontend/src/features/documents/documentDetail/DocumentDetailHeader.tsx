import { Col, Row } from 'react-bootstrap';

import { SectionField } from '@/components/common/Section/SectionField';

import { ComposedDocument } from '../ComposedDocument';
import DownloadDocumentButton from '../DownloadDocumentButton';

interface IDocumentDetailHeaderProps {
  document: ComposedDocument;
}

const DocumentDetailHeader: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailHeaderProps>
> = props => {
  const documentTypeLabel =
    props.document.pimsDocumentRelationship?.document?.documentType?.documentTypeDescription;
  const documentFileName = props.document.pimsDocumentRelationship?.document?.fileName;
  const mayanDocumentId = props.document.pimsDocumentRelationship?.document?.mayanDocumentId || -1;

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
              isFileAvailable={!!props.document.documentDetail?.file_latest.id}
            />
          </Col>
        </Row>
      </SectionField>
    </>
  );
};

export default DocumentDetailHeader;
