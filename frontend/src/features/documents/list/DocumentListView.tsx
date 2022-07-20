import { TableSort } from 'components/Table/TableSort';
import { DocumentTypes } from 'constants/documentTypes';
import { defaultDocumentFilter, IDocumentFilter } from 'interfaces/IDocumentResults';
import { orderBy } from 'lodash';
import { Api_Document } from 'models/api/Document';
import React from 'react';

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
              document.documentTypeId === Number(filters.documentTypeId)) &&
            (!filters.status || document.statusTypeCode?.id === filters.status)
          );
        });
      }
      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = (sort as any)[sortFields[0]];
          return orderBy(documentItems, sortFields[0], keyName);
        }
      }
      return documentItems;
    }
    return [];
  }, [documentResults, sort, filters]);

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
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default DocumentListView;
