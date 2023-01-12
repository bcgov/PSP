import { AxiosResponse } from 'axios';
import { LinkButton } from 'components/common/buttons';
import TooltipIcon from 'components/common/TooltipIcon';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import fileDownload from 'js-file-download';
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
      const data = await provider.downloadDocumentFile(mayanDocumentId, mayanFileId);
      if (data) {
        showFile(data);
      } else {
        toast.error(
          'Failed to download document. If this error persists, contact a system administrator.',
        );
      }
    } else {
      const data = await provider.downloadDocumentFileLatest(mayanDocumentId);
      if (data) {
        showFile(data);
      } else {
        toast.error(
          'Failed to download document. If this error persists, contact a system administrator.',
        );
      }
    }
  }

  const showFile = async (
    response: AxiosResponse<string | ArrayBuffer | ArrayBufferView | Blob, any>,
  ) => {
    const groups = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/g.exec(
      response.headers['content-disposition'],
    );
    if (groups?.length) {
      const fileName = groups[1].replace(/['"]/g, '');
      fileDownload(response.data, fileName);
    }
  };

  if (!props.isFileAvailable && !provider.downloadDocumentFileLoading) {
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
      <LoadingBackdrop show={provider.downloadDocumentFileLoading} />
      <LinkButton
        data-testid="document-download-button"
        disabled={provider.downloadDocumentFileLoading}
        onClick={() => {
          downloadFile(props.mayanDocumentId, props.mayanFileId);
        }}
      >
        <FaDownload />
      </LinkButton>
    </div>
  );
};

export default DownloadDocumentButton;
