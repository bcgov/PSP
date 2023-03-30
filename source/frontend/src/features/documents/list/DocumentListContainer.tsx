import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { SideBarContext } from 'features/properties/map/context/sidebarContext';
import useIsMounted from 'hooks/useIsMounted';
import { Api_DocumentRelationship } from 'models/api/Document';
import { useCallback, useContext, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { DocumentRow } from '../ComposedDocument';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentListView from './DocumentListView';

interface IDocumentListContainerProps {
  parentId: string;
  relationshipType: DocumentRelationshipType;
  disableAdd?: boolean;
  addButtonText?: string;
  title?: string;
}

const DocumentListContainer: React.FunctionComponent<
  React.PropsWithChildren<IDocumentListContainerProps>
> = props => {
  const isMounted = useIsMounted();

  const [documentResults, setDocumentResults] = useState<DocumentRow[]>([]);

  const { file, staleFile, setStaleFile } = useContext(SideBarContext);

  const [pageProps, setPageProps] = useState<{ pageIndex?: number; pageSize: number }>({
    pageIndex: 0,
    pageSize: 10,
  });

  const {
    retrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading,
    deleteDocumentRelationship,
  } = useDocumentRelationshipProvider();

  const retrieveDocuments = useCallback(async () => {
    const documents = await retrieveDocumentRelationship(props.relationshipType, props.parentId);
    if (documents !== undefined && isMounted()) {
      setDocumentResults([
        ...documents
          .filter((x): x is Api_DocumentRelationship => !!x?.document)
          .map(x => DocumentRow.fromApi(x)),
      ]);
    }
  }, [isMounted, retrieveDocumentRelationship, props.relationshipType, props.parentId]);

  useEffect(() => {
    retrieveDocuments();
  }, [retrieveDocuments]);

  useEffect(() => {
    if (staleFile) {
      retrieveDocuments();
    }
  }, [staleFile, retrieveDocuments]);

  const onDelete = async (
    documentRelationship: Api_DocumentRelationship,
  ): Promise<boolean | undefined> => {
    if (documentRelationship.relationshipType !== undefined) {
      let result = await deleteDocumentRelationship(
        documentRelationship.relationshipType,
        documentRelationship,
      );
      if (result && isMounted()) {
        updateCallback();
      }

      return result;
    } else {
      toast.error(
        'Invalid document relationship error during delete. Check responses and try again.',
      );
    }
  };

  const onSuccess = async () => {
    updateCallback();
  };

  const updateCallback = useCallback(() => {
    // Check if the component is working in a File context. If it is, delegate the update.
    if (file === undefined) {
      retrieveDocuments();
    } else {
      setStaleFile(true);
    }
  }, [file, setStaleFile, retrieveDocuments]);

  const currentPageIndex = pageProps.pageIndex;
  const onPageChange = useCallback(
    ({ pageIndex, pageSize }: { pageIndex?: number; pageSize: number }) => {
      setPageProps({ pageIndex: pageIndex ?? currentPageIndex, pageSize });
    },
    [currentPageIndex],
  );

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading}
      documentResults={documentResults}
      onDelete={onDelete}
      onSuccess={onSuccess}
      disableAdd={props.disableAdd}
      onPageChange={onPageChange}
      pageProps={pageProps}
      title={props.title}
    />
  );
};

export default DocumentListContainer;
