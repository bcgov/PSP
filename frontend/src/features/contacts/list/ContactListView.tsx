import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IContactSearchResult } from 'interfaces/IContactSearchResult';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { IoMdPersonAdd } from 'react-icons/io';
import { toast } from 'react-toastify';

import { useSearch } from '../../../hooks/useSearch';
import { IContactFilter } from '../interfaces';
import { ContactSearchResults } from './ContactSearchResults/ContactSearchResults';
import ContactFilter, { defaultFilter } from './filter/ContactFilter';
import * as Styled from './styles';

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const ContactListView = () => {
  const { getContacts } = useApiContacts();
  const {
    results,
    filter,
    sort,
    error,
    currentPage,
    totalPages,
    pageSize,
    setFilter,
    setSort,
    setCurrentPage,
    setPageSize,
    loading,
  } = useSearch<IContactSearchResult, IContactFilter>(
    defaultFilter,
    getContacts,
    'Search returned no results',
  );

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: IContactFilter) => {
      setFilter(filter as any);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return (
    <Styled.ListPage>
      <Styled.Scrollable>
        <Styled.PageHeader>Contacts</Styled.PageHeader>
        <Styled.PageToolbar>
          <ContactFilter filter={filter as any} setFilter={changeFilter} />
          <Styled.Spacer />
          <Styled.PrimaryButton>
            <IoMdPersonAdd color="white" />
            Add new contact
          </Styled.PrimaryButton>
          <Styled.Spacer />
        </Styled.PageToolbar>
        <ContactSearchResults
          loading={loading}
          results={results}
          sort={sort}
          pageSize={pageSize}
          pageIndex={currentPage}
          setSort={setSort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
          pageCount={totalPages}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default ContactListView;
