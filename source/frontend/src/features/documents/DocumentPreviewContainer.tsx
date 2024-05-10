import moment from 'moment';
import { FC, useCallback, useContext, useEffect, useState } from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { ModalContext } from '@/contexts/modalContext';
import { ApiGen_Mayan_FilePage } from '@/models/api/generated/ApiGen_Mayan_FilePage';
import { ApiGen_System_HttpStatusCode } from '@/models/api/generated/ApiGen_System_HttpStatusCode';

import { DocumentViewerContext } from './context/DocumentViewerContext';
import { IDocumentPreviewViewProps } from './DocumentPreviewView';
import { createFileDownload } from './DownloadDocumentButton';
import { useDocumentProvider } from './hooks/useDocumentProvider';

export interface LoadedPages extends ApiGen_Mayan_FilePage {
  loadedDocumentImageDataUrl?: string;
  error: string;
}

export interface IDocumentPreviewContainerProps {
  View: React.FC<IDocumentPreviewViewProps>;
  createFileUrl?: (obj: Blob | MediaSource) => string;
}

export const DocumentPreviewContainer: FC<
  React.PropsWithChildren<IDocumentPreviewContainerProps>
> = ({ View, createFileUrl }) => {
  const { showDocumentPreview, previewDocumentId, setShowDocumentPreview, setPreviewDocumentId } =
    useContext(DocumentViewerContext);
  const { setModalContent, setDisplayModal } = useContext(ModalContext);
  const {
    retrieveDocumentDetail,
    downloadDocumentFilePageImage,
    getDocumentFilePageList,
    downloadWrappedDocumentFileLatest,
    retrieveDocumentDetailLoading,
    getDocumentFilePageListLoading,
  } = useDocumentProvider();
  const [documentPages, setDocumentPages] = useState<LoadedPages[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(0);

  const loadDocumentPageImage = useCallback(
    async (documentId: number, documentFileId: number, documentFilePageId: number) => {
      //todo: handle image failure
      return (createFileUrl ? createFileUrl : URL.createObjectURL)(
        await downloadDocumentFilePageImage(documentId, documentFileId, documentFilePageId),
      );
    },
    [downloadDocumentFilePageImage, createFileUrl],
  );

  const resetDocumentPreview = useCallback(() => {
    setShowDocumentPreview(false);
    setPreviewDocumentId(null);
    setCurrentPage(0);
    setDocumentPages([]);
  }, [setPreviewDocumentId, setShowDocumentPreview]);

  const loadPagesForDocumentId = useCallback(async () => {
    const data = await retrieveDocumentDetail(previewDocumentId);
    if (
      data?.payload?.datetime_created &&
      moment(data.payload.datetime_created).add(1, 'minutes').isAfter(moment())
    ) {
      setModalContent({
        title: 'Document Still Processing',
        message:
          'This document has been uploaded very recently, please wait a few minutes before trying to preview it. You may download the document in the meantime (using the "eye" icon).',
        variant: 'info',
        handleOk: () => {
          setDisplayModal(false);
          resetDocumentPreview();
        },
      });
      setDisplayModal(true);
    } else if (
      data?.httpStatusCode !== ApiGen_System_HttpStatusCode.OK ||
      !data?.payload?.file_latest
    ) {
      setModalContent({
        title: 'Document Preview Error',
        message:
          'Failed to retrieve document details. This may indicate the document is still being processed, or failed to upload. If the issue persists, please contact the system administrator.',
        variant: 'error',
        handleOk: () => {
          setDisplayModal(false);
          resetDocumentPreview();
        },
      });
      setDisplayModal(true);
    } else if (data?.payload?.file_latest) {
      const pages = await getDocumentFilePageList(previewDocumentId, data.payload.file_latest.id);

      //todo: handle various doc failures.
      if (pages?.length > 0) {
        const documentPageImage = await loadDocumentPageImage(
          previewDocumentId,
          data.payload.file_latest.id,
          pages[0].id,
        );
        const lightboxPages = pages.map(page => ({ ...page } as LoadedPages));
        lightboxPages[0].loadedDocumentImageDataUrl = documentPageImage;
        setDocumentPages(lightboxPages);
      } else {
        setModalContent({
          title: 'Document Preview Error',
          message:
            'Unable to retrieve preview pages for this document. Confirm this document is in a valid format for file preview (document, image, pdf). If you believe that this is a valid file type, and the issue persists, please contact the system administrator.',
          variant: 'error',
          handleOk: () => {
            setDisplayModal(false);
            resetDocumentPreview();
          },
        });
        setDisplayModal(true);
      }
    }
  }, [
    getDocumentFilePageList,
    loadDocumentPageImage,
    previewDocumentId,
    resetDocumentPreview,
    retrieveDocumentDetail,
    setDisplayModal,
    setModalContent,
  ]);

  useEffect(() => {
    if (previewDocumentId) {
      loadPagesForDocumentId();
    }
  }, [previewDocumentId, loadPagesForDocumentId]);

  useEffect(() => {
    const page = documentPages[currentPage + 1];
    // lazy load the next page, but do not replace lazy loaded data.
    if (previewDocumentId && page && !page.loadedDocumentImageDataUrl) {
      loadDocumentPageImage(previewDocumentId, page.document_file_id, page.id)
        .then(imageUrl => {
          page.loadedDocumentImageDataUrl = imageUrl;
        })
        .catch(e => {
          console.error(e);
          page.error = e.message;
        });
    }
  }, [currentPage, documentPages, loadDocumentPageImage, previewDocumentId]);
  return (
    <>
      <LoadingBackdrop
        show={retrieveDocumentDetailLoading || getDocumentFilePageListLoading}
        parentScreen
      />
      <View
        currentPage={currentPage}
        setCurrentPage={setCurrentPage}
        showDocumentPreview={showDocumentPreview}
        pages={documentPages}
        resetDocumentPreview={resetDocumentPreview}
        handleDownload={() => {
          downloadWrappedDocumentFileLatest(previewDocumentId).then(file =>
            createFileDownload(file),
          );
        }}
      />
    </>
  );
};
