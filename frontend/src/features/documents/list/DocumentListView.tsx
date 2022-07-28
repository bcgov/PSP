import GenericModal from 'components/common/GenericModal';
import { TableSort } from 'components/Table/TableSort';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { defaultDocumentFilter, IDocumentFilter } from 'interfaces/IDocumentResults';
import { orderBy } from 'lodash';
import { Api_Document, Api_DocumentRelationship } from 'models/api/Document';
import React, { useState } from 'react';

import { DocumentDetailModal } from '../documentDetail/DocumentDetailModal';
import { DocumentFilterForm } from './DocumentFilter/DocumentFilterForm';
import { DocumentResults } from './DocumentResults/DocumentResults';
import * as Styled from './styles';

export interface IDocumentListViewProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
  isLoading: boolean;
  documentResults: Api_DocumentRelationship[];
  hideFilters?: boolean;
  defaultFilters?: IDocumentFilter;
  deleteHandle: (relationship: Api_DocumentRelationship) => Promise<boolean>;
}
/**
 * Page that displays documents information.
 */
export const DocumentListView: React.FunctionComponent<IDocumentListViewProps> = (
  props: IDocumentListViewProps,
) => {
  const { documentResults, isLoading, defaultFilters, hideFilters } = props;

  const [showDeleteConfirmModal, setShowDeleteConfirmModal] = useState<boolean>(false);

  const [sort, setSort] = React.useState<TableSort<Api_Document>>({});

  const [filters, setFilters] = React.useState<IDocumentFilter>(
    defaultFilters ?? defaultDocumentFilter,
  );

  const sortedFilteredDocuments = React.useMemo(() => {
    if (documentResults?.length > 0) {
      let documentItems: Api_Document[] = [
        ...documentResults.map(x => x.document).filter((x): x is Api_Document => !!x),
      ];

      if (filters) {
        documentItems = documentItems.filter(document => {
          return (
            (!filters.documentTypeId ||
              document?.documentType?.id === Number(filters.documentTypeId)) &&
            (!filters.status || document?.statusTypeCode?.id === filters.status)
          );
        });
      }
      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = (sort as any)[sortFields[0]];
          return orderBy(
            documentItems,
            sortFields[0] === 'statusTypeCode' ? 'statusTypeCode.description' : sortFields[0],
            keyName,
          );
        }
      }
      return documentItems;
    }
    return [];
  }, [documentResults, sort, filters]);

  const [isDetailsVisible, setIsDetailsVisible] = useState(false);
  const [selectedDocument, setSelectedDocument] = useState<Api_Document | undefined>(undefined);

  const handleViewDetails = (document: Api_Document) => {
    setIsDetailsVisible(true);
    setSelectedDocument(document);
  };

  const handleViewDetailsClose = () => {
    setIsDetailsVisible(false);
    setSelectedDocument(undefined);
  };
  const handleDeleteClicked = (document: Api_Document) => {
    setShowDeleteConfirmModal(true);
    setSelectedDocument(document);
  };

  const onDeleteConfirm = async () => {
    if (selectedDocument) {
      const documentRelationship = documentResults.find(
        x => x.document?.id === selectedDocument.id,
      );

      if (documentRelationship !== undefined) {
        props.deleteHandle(documentRelationship).then(result => {
          console.log(result);
          if (result) {
            setShowDeleteConfirmModal(false);
            setSelectedDocument(undefined);
          }
        });
      }
    }
  };

  return (
    <Styled.ListPage>
      <Styled.Scrollable vertical={true}>
        <Styled.PageHeader>Documents</Styled.PageHeader>
        {!hideFilters && <DocumentFilterForm onSetFilter={setFilters} documentFilter={filters} />}
        <DocumentResults
          results={sortedFilteredDocuments}
          loading={isLoading}
          sort={sort}
          setSort={setSort}
          onViewDetails={handleViewDetails}
          onDelete={handleDeleteClicked}
        />
      </Styled.Scrollable>
      <DocumentDetailModal
        display={isDetailsVisible}
        setDisplay={setIsDetailsVisible}
        pimsDocument={selectedDocument}
        handleClose={handleViewDetailsClose}
      />
      <GenericModal
        display={showDeleteConfirmModal}
        title={'Delete a document'}
        message={
          <>
            <div>You have chosen to delete this document. </div>
            <br />
            <div>
              If the document is linked to other files or entities in PIMS it will still be
              accessible from there, however if this the only instance then the file will be removed
              from the document store completely.
            </div>
            <br />
            <strong>Do you wish to continue deleting this document?</strong>
          </>
        }
        handleOk={onDeleteConfirm}
        handleCancel={() => setShowDeleteConfirmModal(false)}
        okButtonText="Continue"
        cancelButtonText="Cancel"
        show
      />
    </Styled.ListPage>
  );
};

export default DocumentListView;
