import GenericModal from 'components/common/GenericModal';
import { TableSort } from 'components/Table/TableSort';
import Claims from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Section } from 'features/mapSideBar/tabs/Section';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { defaultDocumentFilter, IDocumentFilter } from 'interfaces/IDocumentResults';
import { orderBy } from 'lodash';
import { Api_Document, Api_DocumentRelationship } from 'models/api/Document';
import React, { useState } from 'react';
import { Button, Col, Row } from 'react-bootstrap';
import { FaUpload } from 'react-icons/fa';
import styled from 'styled-components';

import { DocumentDetailModal } from '../documentDetail/DocumentDetailModal';
import { DocumentUploadModal } from '../documentUpload/DocumentUploadModal';
import { DocumentFilterForm } from './DocumentFilter/DocumentFilterForm';
import { DocumentResults } from './DocumentResults/DocumentResults';

export interface IDocumentListViewProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
  isLoading: boolean;
  documentResults: Api_DocumentRelationship[];
  hideFilters?: boolean;
  defaultFilters?: IDocumentFilter;
  onDelete: (relationship: Api_DocumentRelationship) => Promise<boolean | undefined>;
  refreshDocumentList: () => void;
}
/**
 * Page that displays document information as a list.
 */
export const DocumentListView: React.FunctionComponent<IDocumentListViewProps> = (
  props: IDocumentListViewProps,
) => {
  const { documentResults, isLoading, defaultFilters, hideFilters } = props;

  const [showDeleteConfirmModal, setShowDeleteConfirmModal] = useState<boolean>(false);

  const { hasClaim } = useKeycloakWrapper();

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
  const [isUploadVisible, setIsUploadVisible] = useState(false);
  const [selectedDocument, setSelectedDocument] = useState<Api_Document | undefined>(undefined);

  const handleModalUploadClose = () => {
    setIsUploadVisible(false);
  };

  const handleViewDetails = (document: Api_Document) => {
    setIsDetailsVisible(true);
    setSelectedDocument(document);
  };

  const handleModalDetailsClose = () => {
    setIsDetailsVisible(false);
    setSelectedDocument(undefined);
  };

  const handleDeleteClick = (document: Api_Document) => {
    setShowDeleteConfirmModal(true);
    setSelectedDocument(document);
  };

  const onDeleteConfirm = async () => {
    if (selectedDocument) {
      const documentRelationship = documentResults.find(
        x => x.document?.id === selectedDocument.id,
      );

      if (documentRelationship !== undefined) {
        let result = await props.onDelete(documentRelationship);
        if (result) {
          setShowDeleteConfirmModal(false);
          setSelectedDocument(undefined);
        }
      }
    }
  };

  const onUploadSuccess = () => {
    handleModalUploadClose();
    props.refreshDocumentList();
  };

  return (
    <div>
      <Section
        header={
          <Row>
            <Col xs="auto">Documents</Col>
            {hasClaim(Claims.RESEARCH_ADD) && (
              <Col>
                <StyledAddButton onClick={() => setIsUploadVisible(true)}>
                  <FaUpload />
                  &nbsp;Add a Document
                </StyledAddButton>
              </Col>
            )}
          </Row>
        }
        isCollapsable
      >
        {!hideFilters && <DocumentFilterForm onSetFilter={setFilters} documentFilter={filters} />}
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
        setDisplay={setIsDetailsVisible}
        pimsDocument={selectedDocument}
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
    </div>
  );
};

export default DocumentListView;

const StyledAddButton = styled(Button)`
  font-weight: bold;
  font-size: 1.3rem;
  background-color: ${props => props.theme.css.completedColor};
  margin-bottom: 0.2rem;
`;
