import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { IoMdPersonAdd } from 'react-icons/io';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { Claims } from '@/constants/claims';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { useSearch } from '@/hooks/useSearch';
import { IContactSearchResult } from '@/interfaces/IContactSearchResult';

import {
  ContactFilterComponent,
  defaultFilter,
  RestrictContactType,
} from './ContactFilterComponent/ContactFilterComponent';
import { ContactResultComponent } from './ContactResultComponent/ContactResultComponent';
import { IContactFilter } from './IContactFilter';

interface IContactManagerViewProps {
  setSelectedRows?: (selectedContacts: IContactSearchResult[]) => void;
  selectedRows?: IContactSearchResult[];
  showSelectedRowCount?: boolean;
  isSummary?: boolean;
  noInitialSearch?: boolean;
  className?: string;
  showAddButton?: boolean;
  showActiveSelector?: boolean;
  isSingleSelect?: boolean;
  restrictContactType?: RestrictContactType;
}

/**
 * Component that provides functionality manage contacts. Can be embedded as a widget.
 */
const ContactManagerView = ({
  setSelectedRows,
  selectedRows,
  className,
  isSummary,
  noInitialSearch,
  showSelectedRowCount,
  showAddButton,
  showActiveSelector,
  isSingleSelect,
  restrictContactType,
}: IContactManagerViewProps) => {
  const history = useHistory();
  const { hasClaim } = useKeycloakWrapper();
  const { getContacts } = useApiContacts();

  const initialFilter: IContactFilter = (
    noInitialSearch ? undefined : defaultFilter
  ) as IContactFilter;

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
    setSort,
    setCurrentPage,
    setPageSize,
    loading,
  } = useSearch<IContactSearchResult, IContactFilter>(
    initialFilter,
    getContacts,
    'Search returned no results',
    {},
    0,
    isSummary ? 5 : 10,
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return (
    <div className={className}>
      <Row>
        <Col>
          <ContactFilterComponent
            filter={filter}
            setFilter={setFilter}
            showActiveSelector={showActiveSelector}
            restrictContactType={restrictContactType}
          />
        </Col>
        {showAddButton && hasClaim(Claims.CONTACT_ADD) && (
          <Col xs="auto" xl="3" className="pl-0">
            <StyledPrimaryButton onClick={() => history.push('/contact/new')}>
              <IoMdPersonAdd color="white" className="mr-3" />
              <span>Add new contact</span>
            </StyledPrimaryButton>
          </Col>
        )}
      </Row>
      <div>
        <ContactResultComponent
          loading={loading}
          results={results}
          sort={sort}
          pageSize={pageSize}
          pageIndex={currentPage}
          totalItems={totalItems}
          setSort={setSort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
          pageCount={totalPages}
          selectedRows={selectedRows}
          setSelectedRows={setSelectedRows}
          showSelectedRowCount={showSelectedRowCount}
          isSummary={isSummary}
          isSingleSelect={isSingleSelect}
        />
      </div>
    </div>
  );
};

export default ContactManagerView;

const StyledPrimaryButton = styled(Button)`
  margin: 0.4rem 0.6rem;
  white-space: nowrap;
  display: inline-block;
  gap: 1rem;
`;
