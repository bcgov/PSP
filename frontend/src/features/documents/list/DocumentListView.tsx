import { TableSort } from 'components/Table/TableSort';
import { DocumentTypes } from 'constants/documentTypes';
import { defaultDocumentFilter, IDocumentFilter } from 'interfaces/IDocumentResults';
import { orderBy } from 'lodash';
import { Api_Document } from 'models/api/Document';
import React, { useState } from 'react';

import { DocumentDetailModal } from '../documentDetail/DocumentDetailModal';
import { DocumentFilterForm } from './DocumentFilter/DocumentFilterForm';
import { DocumentResults } from './DocumentResults/DocumentResults';
import * as Styled from './styles';

export interface IDocumentListViewProps {
  entityId: number;
  documentType: DocumentTypes;
  isLoading: boolean;
  documentResults: Api_Document[];
  hideFilters?: boolean;
  defaultFilters?: IDocumentFilter;
}
/**
 * Page that displays documents information.
 */
export const DocumentListView: React.FunctionComponent<IDocumentListViewProps> = (
  props: IDocumentListViewProps,
) => {
  const { documentResults, isLoading, defaultFilters, hideFilters } = props;
  const [sort, setSort] = React.useState<TableSort<Api_Document>>({});

  const [filters, setFilters] = React.useState<IDocumentFilter>(
    defaultFilters ?? defaultDocumentFilter,
  );

  const sortedFilteredDocuments = React.useMemo(() => {
    if (documentResults?.length > 0) {
      let documentItems = [...documentResults];

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

  const handleViewDetails = (values: Api_Document) => {
    setIsDetailsVisible(true);
    setSelectedDocument(values);
  };
  const handleDelete = (values: Api_Document) => {
    // Todo: need to have the parent entity in context
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
          onDelete={handleDelete}
        />
      </Styled.Scrollable>
      <DocumentDetailModal
        display={isDetailsVisible}
        setDisplay={setIsDetailsVisible}
        pimsDocument={selectedDocument}
      />
    </Styled.ListPage>
  );
};

export default DocumentListView;
