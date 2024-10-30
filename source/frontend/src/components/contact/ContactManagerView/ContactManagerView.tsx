import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { Claims } from '@/constants/claims';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { useSearch } from '@/hooks/useSearch';
import { fromContactSummary, IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_ContactSummary } from '@/models/api/generated/ApiGen_Concepts_ContactSummary';

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
  } = useSearch<ApiGen_Concepts_ContactSummary, IContactFilter>(
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
      </Row>
      <Row>
        {showAddButton && hasClaim(Claims.CONTACT_ADD) && (
          <Col xs="auto" xl="3" className="pl-0">
            <StyledPrimaryButton onClick={() => history.push('/contact/new')}>
              <FaPlus className="mr-3" />
              &nbsp;Add new contact
            </StyledPrimaryButton>
          </Col>
        )}
      </Row>
      <div>
        <ContactResultComponent
          loading={loading}
          results={results.map(fromContactSummary)}
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
  margin: 2rem 1.5rem;
  &.btn.btn-primary {
    background-color: ${props => props.theme.bcTokens.iconsColorSuccess};
  }
`;
