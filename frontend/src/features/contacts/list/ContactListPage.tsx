import ContactManagerView from 'components/contact/ContactManagerView/ContactManagerView';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IContactSearchResult } from 'interfaces/IContactSearchResult';
import { noop } from 'lodash';
import { useCallback, useEffect } from 'react';
import { IoMdPersonAdd } from 'react-icons/io';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';

import { useSearch } from '../../../hooks/useSearch';
import { IContactFilter } from '../interfaces';
import { ContactSearchResults } from './ContactSearchResults/ContactSearchResults';
import ContactFilter, { defaultFilter } from './filter/ContactFilter';
import * as Styled from './styles';

interface IContactListViewProps {
  setSelectedRows: (selectedContacts: IContactSearchResult[]) => void;
  selectedRows: IContactSearchResult[];
  showSelectedRowCount?: boolean;
  className?: string;
  hideAddButton?: boolean;
}

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const ContactListPage = ({
  setSelectedRows,
  selectedRows,
  className,
  showSelectedRowCount,
  hideAddButton,
}: IContactListViewProps) => {
  const history = useHistory();
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
    <Styled.ListPage className={className}>
      <Styled.Scrollable>
        <Styled.PageHeader>Contacts</Styled.PageHeader>
        <ContactManagerView
          showActiveSelector
          showAddButton
          setSelectedRows={noop}
          selectedRows={[]}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};
