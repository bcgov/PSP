import { orderBy } from 'lodash';
import React, { useEffect, useState } from 'react';

import GenericModal from '@/components/common/GenericModal';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableSort } from '@/components/Table/TableSort';
import Claims from '@/constants/claims';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import { DocumentTypeName } from '@/constants/documentType';
import { defaultDocumentFilter, IDocumentFilter } from '@/interfaces/IDocumentResults';
import { Api_Document, Api_DocumentRelationship, Api_DocumentType } from '@/models/api/Document';

import { DocumentRow } from '../ComposedDocument';
import { DocumentDetailModal } from '../documentDetail/DocumentDetailModal';
import { DocumentUploadModal } from '../documentUpload/DocumentUploadModal';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { DocumentFilterForm } from './DocumentFilter/DocumentFilterForm';
import { DocumentResults } from './DocumentResults/DocumentResults';

export interface IDocumentListViewProps {
  parentId: string;
  relationshipType: DocumentRelationshipType;
  isLoading: boolean;
  documentResults: DocumentRow[];
  hideFilters?: boolean;
  defaultFilters?: IDocumentFilter;
  addButtonText?: string;
  onDelete: (relationship: Api_DocumentRelationship) => Promise<boolean | undefined>;
  onSuccess: () => void;
  disableAdd?: boolean;
  title?: string;
}
/**
 * Page that displays document information as a list.
 */
export const DocumentListView: React.FunctionComponent<
  React.PropsWithChildren<IDocumentListViewProps>
> = (props: IDocumentListViewProps) => {
  const { documentResults, isLoading, defaultFilters, hideFilters, title } = props;

  const [showDeleteConfirmModal, setShowDeleteConfirmModal] = useState<boolean>(false);

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);

  const [sort, setSort] = React.useState<TableSort<Api_Document>>({});

  const [filters, setFilters] = React.useState<IDocumentFilter>(
    defaultFilters ?? defaultDocumentFilter,
  );

  const { getDocumentRelationshipTypes, getDocumentTypes } = useDocumentProvider();

  useEffect(() => {
    const fetch = async () => {
      if (props.relationshipType === DocumentRelationshipType.TEMPLATES) {
        const axiosResponse = await getDocumentTypes();
        if (axiosResponse !== undefined) {
          setDocumentTypes(axiosResponse?.filter(x => x.documentType === DocumentTypeName.CDOGS));
        }
      } else {
        const axiosResponse = await getDocumentRelationshipTypes(props.relationshipType);
        if (axiosResponse !== undefined) {
          setDocumentTypes(axiosResponse?.filter(x => x.isDisabled !== true) || []);
        }
      }
    };

    fetch();
  }, [props.relationshipType, getDocumentTypes, getDocumentRelationshipTypes]);

  const mapSortField = (sortField: string) => {
    if (sortField === 'documentType') {
      return 'documentType.documentType';
    } else if (sortField === 'statusTypeCode') {
      return 'statusTypeCode.description';
    }
    return sortField;
  };

  const sortedFilteredDocuments = React.useMemo(() => {
    if (documentResults?.length > 0) {
      let documentItems: DocumentRow[] = [];
      if (filters) {
        documentItems = documentResults.filter(document => {
          const matchesDocumentType =
            !filters.documentTypeId ||
            document?.documentType?.id === Number(filters.documentTypeId);
          const matchesStatus = !filters.status || document?.statusTypeCode?.id === filters.status;
          const filename = document.fileName?.toLowerCase() || '';
          const matchesFilename: boolean =
            filters.filename !== ''
              ? filename.indexOf(filters.filename.toLowerCase() || '') > -1
              : true;

          return matchesDocumentType && matchesStatus && matchesFilename;
        });
      }
      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = (sort as any)[sortFields[0]];
          return orderBy(documentItems, mapSortField(sortFields[0]), keyName);
        }
      }
      return documentItems;
    }
    return [];
  }, [documentResults, sort, filters]);

  const [isDetailsVisible, setIsDetailsVisible] = useState(false);
  const [isUploadVisible, setIsUploadVisible] = useState(false);
  const [selectedDocument, setSelectedDocument] = useState<Api_DocumentRelationship | undefined>(
    undefined,
  );

  const handleModalUploadClose = () => {
    setIsUploadVisible(false);
  };

  const handleViewDetails = (document: Api_DocumentRelationship) => {
    setIsDetailsVisible(true);
    setSelectedDocument(document);
  };

  const handleModalDetailsClose = () => {
    setIsDetailsVisible(false);
    setSelectedDocument(undefined);
  };

  const handleDeleteClick = (document: Api_DocumentRelationship) => {
    setShowDeleteConfirmModal(true);
    setSelectedDocument(document);
  };

  const onDeleteConfirm = async () => {
    if (selectedDocument) {
      const documentToDelete = documentResults.find(x => x?.id === selectedDocument.document?.id);

      if (documentToDelete !== undefined) {
        let result = await props.onDelete(DocumentRow.toApi(documentToDelete));
        if (result) {
          setShowDeleteConfirmModal(false);
          setSelectedDocument(undefined);
        }
      }
    }
  };

  const onUploadSuccess = () => {
    handleModalUploadClose();
    props.onSuccess();
  };

  const onUpdateSuccess = () => {
    handleModalDetailsClose();
    props.onSuccess();
  };

  const getHeader = () => {
    if (props.disableAdd === true) {
      return title ?? 'Documents';
    }
    return (
      <SectionListHeader
        claims={[Claims.DOCUMENT_ADD]}
        title={title ?? 'Documents'}
        addButtonText={props.addButtonText || 'Add a Document'}
        onAdd={() => setIsUploadVisible(true)}
      />
    );
  };
  return (
    <>
      <Section header={getHeader()} title="documents" isCollapsable initiallyExpanded>
        {!hideFilters && (
          <DocumentFilterForm
            onSetFilter={setFilters}
            documentFilter={filters}
            documentTypes={documentTypes}
          />
        )}
        <DocumentResults
          results={sortedFilteredDocuments}
          loading={isLoading}
          sort={sort}
          setSort={setSort}
          onViewDetails={handleViewDetails}
          onDelete={handleDeleteClick}
        />
      </Section>
      <DocumentDetailModal
        display={isDetailsVisible}
        relationshipType={props.relationshipType}
        setDisplay={setIsDetailsVisible}
        pimsDocument={selectedDocument ? DocumentRow.fromApi(selectedDocument) : undefined}
        onUpdateSuccess={onUpdateSuccess}
        onClose={handleModalDetailsClose}
      />
      <DocumentUploadModal
        parentId={props.parentId}
        relationshipType={props.relationshipType}
        display={isUploadVisible}
        setDisplay={setIsUploadVisible}
        onUploadSuccess={onUploadSuccess}
        onClose={handleModalUploadClose}
      />
      <GenericModal
        display={showDeleteConfirmModal}
        title={'Delete a document'}
        message={
          <div className="p-3">
            <div>You have chosen to delete this document. </div>
            <br />
            <div>
              If the document is linked to other files or entities in PIMS it will still be
              accessible from there, however if this the only instance then the file will be removed
              from the document store completely.
            </div>
            <br />
            <strong>Do you wish to continue deleting this document?</strong>
          </div>
        }
        handleOk={onDeleteConfirm}
        handleCancel={() => setShowDeleteConfirmModal(false)}
        okButtonText="Continue"
        cancelButtonText="Cancel"
      />
    </>
  );
};

export default DocumentListView;
