import { useCallback, useContext, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import DocumentIcon from '@/assets/images/document-icon.svg?react';
import { SelectOption } from '@/components/common/form/Select';
import { PaddedScrollable } from '@/components/common/styles';
import * as CommonStyled from '@/components/common/styles';
import { useApiDocuments } from '@/hooks/pims-api/useApiDocuments';
import { useSearch } from '@/hooks/useSearch';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentSearchFilter } from '@/models/api/generated/ApiGen_Concepts_DocumentSearchFilter';
import { ApiGen_Concepts_DocumentSearchResult } from '@/models/api/generated/ApiGen_Concepts_DocumentSearchResult';
import { exists } from '@/utils/utils';

import { DocumentViewerContext } from '../../context/DocumentViewerContext';
import { DocumentDetailModal } from '../../documentDetail/DocumentDetailModal';
import { useDocumentProvider } from '../../hooks/useDocumentProvider';
import { DocumentSearchFilterModel } from '../../models/DocumentFilterModel';
import { DocumentSearchResults } from '../DocumentSearchResults/DocumentSearchResults';
import DocumentSearchFilter from './DocumentSearchFilter/DocumentSearchFilter';

/**
 * Page that displays Documents information.
 */
export const DocumentSearchListView: React.FC<unknown> = () => {
  const { getDocumentsPagedApi } = useApiDocuments();
  const { getDocumentTypes } = useDocumentProvider();
  const { setPreviewDocumentId, setShowDocumentPreview } = useContext(DocumentViewerContext);

  const [documentTypesOptions, setDocumentTypesOptions] = useState<SelectOption[]>(null);
  const [isDetailsVisible, setIsDetailsVisible] = useState(false);
  const [selectedDocument, setSelectedDocument] =
    useState<ApiGen_Concepts_DocumentRelationship | null>(null);

  const {
    results,
    filter,
    sort,
    error,
    totalItems,
    currentPage,
    totalPages,
    pageSize,
    setFilter,
    setCurrentPage,
    setPageSize,
    loading,
  } = useSearch<ApiGen_Concepts_DocumentSearchResult, ApiGen_Concepts_DocumentSearchFilter>(
    new DocumentSearchFilterModel().toApi(),
    getDocumentsPagedApi,
    'No matching results can be found. Try widening your search criteria.',
  );

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: ApiGen_Concepts_DocumentSearchFilter) => {
      setFilter(filter);
    },
    [setFilter],
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  useEffect(() => {
    const fetch = async () => {
      if (documentTypesOptions === null) {
        const axiosResponse = await getDocumentTypes();
        if (exists(axiosResponse)) {
          const typeOptions: SelectOption[] = axiosResponse.map(dt => {
            return {
              label: dt.documentTypeDescription || '',
              value: dt.id ? dt.id.toString() : '',
            };
          });
          setDocumentTypesOptions(typeOptions);
        }
      }
    };

    fetch();
  }, [getDocumentTypes, documentTypesOptions]);

  return (
    <CommonStyled.ListPage>
      <PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <DocumentIcon
                title="Document search Icon"
                width={24}
                height={24}
                fill="currentColor"
                stroke="currentColor"
              />
              <span className="ml-2">Documents</span>
            </div>
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <DocumentSearchFilter
                documentTypeOptions={documentTypesOptions ?? []}
                filter={filter}
                setFilter={changeFilter}
              />
            </Col>
          </Row>
        </CommonStyled.PageToolbar>

        <DocumentSearchResults
          loading={loading}
          results={results}
          totalItems={totalItems}
          pageIndex={currentPage}
          pageSize={pageSize}
          pageCount={totalPages}
          sort={sort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
          onPreview={mayanDocumentId => {
            setPreviewDocumentId(mayanDocumentId);
            setShowDocumentPreview(true);
          }}
          onViewDetails={document => {
            setSelectedDocument(document);
            setIsDetailsVisible(true);
          }}
        />

        {exists(selectedDocument) && (
          <DocumentDetailModal
            display={isDetailsVisible}
            relationshipType={selectedDocument.relationshipType}
            setDisplay={setIsDetailsVisible}
            pimsDocumentRelationship={selectedDocument}
            onClose={() => {
              setIsDetailsVisible(false);
              setSelectedDocument(null);
            }}
            canEdit={false}
          />
        )}
      </PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

export default DocumentSearchListView;

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;

  svg {
    vertical-align: baseline;
  }
`;
