import orderBy from 'lodash/orderBy';
import React, { useContext, useEffect, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import styled from 'styled-components';

import RefreshButton from '@/components/common/buttons/RefreshButton';
import GenericModal from '@/components/common/GenericModal';
import { Section } from '@/components/common/Section/Section';
import { InlineFlexDiv, StyledSectionAddButton } from '@/components/common/styles';
import { TableSort } from '@/components/Table/TableSort';
import Claims from '@/constants/claims';
import { DocumentTypeName } from '@/constants/documentType';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { defaultDocumentFilter, IDocumentFilter } from '@/interfaces/IDocumentResults';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_Document } from '@/models/api/generated/ApiGen_Concepts_Document';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';

import { DocumentRow } from '../ComposedDocument';
import { DocumentViewerContext } from '../context/DocumentViewerContext';
import { DocumentDetailModal } from '../documentDetail/DocumentDetailModal';
import { DocumentUploadModal } from '../documentUpload/DocumentUploadModal';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { DocumentFilterForm } from './DocumentFilter/DocumentFilterForm';
import { DocumentResults } from './DocumentResults/DocumentResults';

export interface IDocumentListViewProps {
  parentId: string;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  isLoading: boolean;
  documentResults: DocumentRow[];
  hideFilters?: boolean;
  defaultFilters?: IDocumentFilter;
  addButtonText?: string;
  disableAdd?: boolean;
  title?: string;
  onDelete: (relationship: ApiGen_Concepts_DocumentRelationship) => Promise<boolean | undefined>;
  onSuccess: () => void;
  onRefresh: () => void;
}
/**
 * Page that displays document information as a list.
 */
export const DocumentListView: React.FunctionComponent<IDocumentListViewProps> = (
  props: IDocumentListViewProps,
) => {
  const { hasClaim } = useKeycloakWrapper();

  const { documentResults, isLoading, defaultFilters, hideFilters, title } = props;

  const [showDeleteConfirmModal, setShowDeleteConfirmModal] = useState<boolean>(false);

  const [documentTypes, setDocumentTypes] = useState<ApiGen_Concepts_DocumentType[]>([]);

  const [sort, setSort] = React.useState<TableSort<ApiGen_Concepts_Document>>({});

  const [filters, setFilters] = React.useState<IDocumentFilter>(
    defaultFilters ?? defaultDocumentFilter,
  );

  const { getDocumentRelationshipTypes, getDocumentTypes } = useDocumentProvider();

  const { setPreviewDocumentId, setShowDocumentPreview } = useContext(DocumentViewerContext);

  useEffect(() => {
    const fetch = async () => {
      if (props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates) {
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

  const maxDocumentCount = useMemo(() => {
    return props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates ? 1 : 10;
  }, [props.relationshipType]);

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

  async function previewDocumentFile(mayanDocumentId: number) {
    setPreviewDocumentId(mayanDocumentId);
    setShowDocumentPreview(true);
  }

  const [isDetailsVisible, setIsDetailsVisible] = useState(false);
  const [isUploadVisible, setIsUploadVisible] = useState(false);
  const [selectedDocument, setSelectedDocument] = useState<
    ApiGen_Concepts_DocumentRelationship | undefined
  >(undefined);

  const handleModalUploadClose = () => {
    setIsUploadVisible(false);
    props.onSuccess();
  };

  const handlePreview = (documentRelationship: ApiGen_Concepts_DocumentRelationship) => {
    previewDocumentFile(documentRelationship?.document?.mayanDocumentId);
  };

  const handleViewDetails = (document: ApiGen_Concepts_DocumentRelationship) => {
    setIsDetailsVisible(true);
    setSelectedDocument(document);
  };

  const handleModalDetailsClose = () => {
    setIsDetailsVisible(false);
    setSelectedDocument(undefined);
  };

  const handleDeleteClick = (document: ApiGen_Concepts_DocumentRelationship) => {
    setShowDeleteConfirmModal(true);
    setSelectedDocument(document);
  };

  const onDeleteConfirm = async () => {
    if (selectedDocument) {
      const documentToDelete = documentResults.find(x => x?.id === selectedDocument.document?.id);

      if (documentToDelete !== undefined) {
        const result = await props.onDelete(DocumentRow.toApi(documentToDelete));
        if (result) {
          setShowDeleteConfirmModal(false);
          setSelectedDocument(undefined);
        }
      }
    }
  };

  const onUpdateSuccess = () => {
    handleModalDetailsClose();
    props.onSuccess();
  };

  const getHeader = (): React.ReactNode => {
    if (props.disableAdd === true) {
      return title ?? 'Documents';
    }

    return (
      <>
        <StyledRow className="no-gutters">
          <Col xs="auto" className="px-2 my-1">
            <span>{title ?? 'Documents'}</span>
          </Col>
          <Col xs="auto" className="my-1">
            <ListHeaderActionsDiv>
              {hasClaim([Claims.DOCUMENT_ADD]) && (
                <StyledSectionAddButton
                  onClick={() => setIsUploadVisible && setIsUploadVisible(true)}
                  data-testid={props['data-testId']}
                >
                  <FaPlus size={'2rem'} />
                  &nbsp;{'Add Document'}
                </StyledSectionAddButton>
              )}
              <RefreshButton
                onClick={() => props.onRefresh && props.onRefresh()}
                type="button"
                toolText="Refresh"
                toolId="btn-refresh-tooltip"
              ></RefreshButton>
            </ListHeaderActionsDiv>
          </Col>
        </StyledRow>
      </>
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
          onPreview={handlePreview}
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
        maxDocumentCount={maxDocumentCount}
        display={isUploadVisible}
        setDisplay={setIsUploadVisible}
        onUploadSuccess={handleModalUploadClose}
        onClose={handleModalUploadClose}
      />
      <GenericModal
        variant="error"
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
        okButtonText="Yes"
        cancelButtonText="No"
      />
    </>
  );
};

export default DocumentListView;

const StyledRow = styled(Row)`
  justify-content: space-between;
  align-items: end;
  min-height: 4.5rem;
  .btn {
    margin: 0;
  }
`;

const ListHeaderActionsDiv = styled(InlineFlexDiv)`
  justify-content: space-between;
  gap: 0.5rem;
`;
