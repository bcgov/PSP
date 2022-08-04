import { LinkButton } from 'components/common/buttons';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { FileDownload } from 'models/api/DocumentStorage';
import { FaDownload } from 'react-icons/fa';

import { useDocumentProvider } from './hooks/useDocumentProvider';

export interface IDownloadDocumentButtonProps {
  mayanDocumentId: number;
  mayanFileId?: number;
}

const DownloadDocumentButton: React.FunctionComponent<IDownloadDocumentButtonProps> = props => {
  let provider = useDocumentProvider();

  async function downloadFile(mayanDocumentId: number, mayanFileId?: number) {
    if (mayanFileId !== undefined) {
      const data = await provider.downloadDocumentFile(mayanDocumentId, mayanFileId);
      if (data) {
        showFile(data.payload);
      }
    } else {
      const data = await provider.downloadDocumentFileLatest(mayanDocumentId);
      if (data) {
        showFile(data.payload);
      }
    }
  }

  const showFile = (file: FileDownload) => {
    const aElement = document.createElement('a');
    aElement.href = `data:${file.mimetype};base64,` + file.filePayload;
    aElement.download = file.fileName;
    aElement.click();
    window.URL.revokeObjectURL(aElement.href);
  };

  return (
    <div>
      <LoadingBackdrop show={provider.downloadDocumentFileLoading} />
      <LinkButton
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
