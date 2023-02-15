import { LinkButton } from 'components/common/buttons';
import TooltipIcon from 'components/common/TooltipIcon';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import fileDownload from 'js-file-download';
import { Api_FileDownload } from 'models/api/DocumentStorage';
import { FaDownload } from 'react-icons/fa';
import { toast } from 'react-toastify';

import { useDocumentProvider } from './hooks/useDocumentProvider';

export interface IDownloadDocumentButtonProps {
  mayanDocumentId: number;
  mayanFileId?: number;
  isFileAvailable?: boolean;
}

const DownloadDocumentButton: React.FunctionComponent<
  React.PropsWithChildren<IDownloadDocumentButtonProps>
> = props => {
  let provider = useDocumentProvider();

  async function downloadFile(mayanDocumentId: number, mayanFileId?: number) {
    if (mayanFileId !== undefined) {
      const data = await provider.downloadWrappedDocumentFile(mayanDocumentId, mayanFileId);
      if (data) {
        showFile(data);
      } else {
        toast.error(
          'Failed to download document. If this error persists, contact a system administrator.',
        );
      }
    } else {
      const data = await provider.downloadWrappedDocumentFileLatest(mayanDocumentId);
      if (data) {
        showFile(data);
      } else {
        toast.error(
          'Failed to download document. If this error persists, contact a system administrator.',
        );
      }
    }
  }

  if (!props.isFileAvailable && !provider.downloadWrappedDocumentFileLoading) {
    return (
      <TooltipIcon
        toolTipId="document-not-available-tooltip"
        data-testid="document-not-available-tooltip"
        toolTip="This document is still being processed and is not yet available to view or download. Please try again in a few minutes. If you continue to see this error, please contact the system administrator."
      ></TooltipIcon>
    );
  }

  return (
    <div>
      <LoadingBackdrop show={provider.downloadWrappedDocumentFileLoading} />
      <LinkButton
        data-testid="document-download-button"
        disabled={provider.downloadWrappedDocumentFileLoading}
        onClick={() => {
          downloadFile(props.mayanDocumentId, props.mayanFileId);
        }}
      >
        <FaDownload />
      </LinkButton>
    </div>
  );
};

export const showFile = async (response?: Api_FileDownload) => {
  if (response !== undefined && response.size > 0) {
    if (response.encodingType === 'base64') {
      const blob = b64toBlob(response.filePayload, response.mimetype);
      fileDownload(blob, response.fileName);
    } else {
      throw new Error('Only base64 encoding is supported');
    }
  }
};

export const b64toBlob = (b64Data: string, contentType: string, sliceSize = 512) => {
  const byteCharacters = atob(b64Data);
  const byteArrays = [];

  for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
    const slice = byteCharacters.slice(offset, offset + sliceSize);

    const byteNumbers = new Array(slice.length);
    for (let i = 0; i < slice.length; i++) {
      byteNumbers[i] = slice.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);
    byteArrays.push(byteArray);
  }

  const blob = new Blob(byteArrays, { type: contentType });
  return blob;
};

export default DownloadDocumentButton;
