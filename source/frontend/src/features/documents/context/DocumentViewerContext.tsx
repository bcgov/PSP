import { createContext, ReactNode, useState } from 'react';

export interface IDocumentViewerContext {
  setPreviewDocumentId: (documentId: number) => void;
  setShowDocumentPreview: (document: boolean) => void;
  showDocumentPreview: boolean;
  previewDocumentId: number | null;
}

export const DocumentViewerContext = createContext<IDocumentViewerContext>({
  setPreviewDocumentId: () => {
    throw Error('setDisplayModal function not defined');
  },
  setShowDocumentPreview: () => {
    throw Error('setDisplayModal function not defined');
  },
  showDocumentPreview: false,
  previewDocumentId: null,
});

export const DocumentViewerContextProvider = (props: {
  children: ReactNode;
  showDocumentPreview?: boolean;
  previewDocumentId?: number;
}) => {
  const [previewDocumentId, setPreviewDocumentId] = useState<number | null>(
    props.previewDocumentId ?? null,
  );
  const [showDocumentPreview, setShowDocumentPreview] = useState<boolean>(
    props.showDocumentPreview ?? false,
  );

  return (
    <DocumentViewerContext.Provider
      value={{
        setPreviewDocumentId: setPreviewDocumentId,
        setShowDocumentPreview: setShowDocumentPreview,
        previewDocumentId,
        showDocumentPreview,
      }}
    >
      {props.children}
    </DocumentViewerContext.Provider>
  );
};
