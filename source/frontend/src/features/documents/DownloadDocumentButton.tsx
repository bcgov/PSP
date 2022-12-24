import { AxiosResponse } from 'axios';
import { LinkButton } from 'components/common/buttons';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import fileDownload from 'js-file-download';
import { FaDownload } from 'react-icons/fa';

import { useDocumentProvider } from './hooks/useDocumentProvider';

export interface IDownloadDocumentButtonProps {
  mayanDocumentId: number;
  mayanFileId?: number;
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
      }
    } else {
      const data = await provider.downloadDocumentFileLatest(mayanDocumentId);
      if (data) {
        showFile(data);
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
