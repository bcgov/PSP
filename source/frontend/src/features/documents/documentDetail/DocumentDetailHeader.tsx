import styled from 'styled-components';

import { SectionField } from '@/components/common/Section/SectionField';

import { ComposedDocument } from '../ComposedDocument';
import DownloadDocumentButton from '../DownloadDocumentButton';

interface IDocumentDetailHeaderProps {
  document: ComposedDocument;
}

const DocumentDetailHeader: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailHeaderProps>
> = props => {
  const documentFileName = props.document.pimsDocumentRelationship?.document?.fileName;
  const mayanDocumentId = props.document.pimsDocumentRelationship?.document?.mayanDocumentId || -1;

  return (
    <div className="pb-4">
      <SectionField label={'File name'} labelWidth="4" className="pb-3">
        <StyledFileNameRow>
          <div>
            <label>{documentFileName}</label>
          </div>
          <div>
            <DownloadDocumentButton
              mayanDocumentId={mayanDocumentId}
              mayanFileId={props.document.mayanFileId}
              isFileAvailable={!!props.document?.documentDetail?.file_latest?.id}
            />
          </div>
        </StyledFileNameRow>
      </SectionField>
    </div>
  );
};

export default DocumentDetailHeader;

const StyledFileNameRow = styled('div')`
  display: flex;
  flex-direction: row;
  word-break: break-all;

  div:first-child {
    flex-grow: 1;
  }

  label {
    margin-right: 1rem;
  }
`;
