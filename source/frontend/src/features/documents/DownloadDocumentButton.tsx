import { AxiosResponse } from 'axios';
import { File } from 'buffer';
import fileDownload from 'js-file-download';
import { FaDownload } from 'react-icons/fa';
import { toast } from 'react-toastify';

import { LinkButton } from '@/components/common/buttons';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ApiGen_Requests_FileDownloadResponse } from '@/models/api/generated/ApiGen_Requests_FileDownloadResponse';

import { useDocumentProvider } from './hooks/useDocumentProvider';

export interface IDownloadDocumentButtonProps {
  mayanDocumentId: number;
  mayanFileId?: number;
  isFileAvailable?: boolean;
}

const DownloadDocumentButton: React.FunctionComponent<
  React.PropsWithChildren<IDownloadDocumentButtonProps>
> = props => {
  const provider = useDocumentProvider();

  async function downloadFile(mayanDocumentId: number, mayanFileId?: number) {
    if (mayanFileId !== undefined) {
      const data = await provider.streamDocumentFile(mayanDocumentId, mayanFileId);
      if (data) {
        showFile(data);
      } else {
        toast.error(
          'Failed to download document. If this error persists, contact a system administrator.',
        );
      }
    } else {
      const data = await provider.streamDocumentFileLatest(mayanDocumentId);
      if (data) {
        showFile(data);
      } else {
        toast.error(
          'Failed to download document. If this error persists, contact a system administrator.',
        );
      }
    }
  }

  if (!props.isFileAvailable && !provider.streamDocumentFileLoading) {
    return (
      <TooltipIcon
        toolTipId="document-not-available-tooltip"
        data-testid="document-not-available-tooltip"
        toolTip="This document is still being processed and is not yet available to view or download. Please try again in a few minutes. If you continue to see this error, please contact the system administrator"
      ></TooltipIcon>
    );
  }

  return (
    <div>
      <LoadingBackdrop
        show={provider.streamDocumentFileLoading || provider.streamDocumentFileLatestLoading}
      />
      <LinkButton
        data-testid="document-download-button"
        disabled={provider.streamDocumentFileLoading || provider.streamDocumentFileLatestLoading}
        onClick={() => {
          downloadFile(props.mayanDocumentId, props.mayanFileId);
        }}
      >
        <FaDownload />
      </LinkButton>
    </div>
  );
};

/**
 * Javascript function to trigger browser to save data to file as if it was downloaded.
 * @param response The API file download response.
 * @param fileName Optional file name to override default file name returned from API.
 */
export const createFileDownload = async (
  response?: ApiGen_Requests_FileDownloadResponse,
  fileName?: string,
) => {
  if (
    response !== undefined &&
    response.size > 0 &&
    response.filePayload !== null &&
    response.mimetype !== null &&
    response.fileName !== null
  ) {
    if (response.encodingType === 'base64') {
      const blob = b64toBlob(response.filePayload, response.mimetype);
      const displayedFileName = fileName ? fileName : response.fileName;
      fileDownload(blob, displayedFileName);
    } else {
      throw new Error('Only base64 encoding is supported');
    }
  }
};

const showFile = async (response: AxiosResponse<File, any>, fileName?: string) => {
  const groups = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/g.exec(
    response.headers['content-disposition'],
  );
  if (groups?.length) {
    fileDownload(response.data, fileName ?? groups[1].replace(/['"]/g, ''));
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
