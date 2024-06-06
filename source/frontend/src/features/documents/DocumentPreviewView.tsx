import * as React from 'react';
import { Spinner } from 'react-bootstrap';
import { GrDocumentMissing } from 'react-icons/gr';
import Lightbox from 'yet-another-react-lightbox';
import { Captions, Counter, Download, Fullscreen, Zoom } from 'yet-another-react-lightbox/plugins';

import TooltipIcon from '@/components/common/TooltipIcon';

import { LoadedPage } from './DocumentPreviewContainer';

export interface IDocumentPreviewViewProps {
  currentPage: number;
  setCurrentPage: (page: number) => void;
  showDocumentPreview: boolean;
  pages: LoadedPage[];
  resetDocumentPreview: () => void;
  handleDownload: () => void;
}

export const DocumentPreviewView: React.FunctionComponent<IDocumentPreviewViewProps> = ({
  currentPage,
  showDocumentPreview,
  pages,
  setCurrentPage,
  handleDownload,
  resetDocumentPreview,
}) => {
  return (
    <Lightbox
      index={currentPage}
      open={pages.length > 0 && showDocumentPreview}
      slides={pages.map(page => ({
        src: page.loadedDocumentImageDataUrl,
        description: (
          <TooltipIcon
            toolTipId="document-preview-tip"
            innerClassName="text-white"
            toolTip={
              <>
                <p>
                  You are viewing a preview of the selected document, with a limited number of
                  pages.
                </p>
                <p>To view the full document, please download it.</p>
              </>
            }
          />
        ),
      }))}
      animation={{ fade: 500, swipe: 750 }}
      render={{
        buttonPrev: currentPage > 0 ? undefined : () => null,
        buttonNext: currentPage + 1 === pages.length ? () => null : undefined,
        slide: () => {
          const page = pages[currentPage];
          if (page.error) {
            return <GrDocumentMissing size={48} color="red" />;
          } else if (!page.loadedDocumentImageDataUrl) {
            return (
              <Spinner animation="border" variant="warning" data-testid="filter-backdrop-loading" />
            );
          }
          // this will fall back to the default slide.
          return;
        },
      }}
      on={{
        view: async ({ index: currentIndex }) => {
          setCurrentPage(currentIndex);
        },

        exited: () => {
          resetDocumentPreview();
        },
      }}
      download={{
        download: () => {
          handleDownload();
        },
      }}
      plugins={[Captions, Counter, Fullscreen, Zoom, Download]}
    />
  );
};

export default DocumentPreviewView;
