import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { IContactSearchResult, isPersonResult } from '@/interfaces';
import { isValidId } from '@/utils';

const summaryColumns: ColumnWithProps<IContactSearchResult>[] = [
  {
    Header: '',
    id: 'id',
    align: 'center',
    width: 20,
    maxWidth: 20,
    Cell: (props: CellProps<IContactSearchResult>) =>
      isValidId(props.row.original.personId) ? (
        <FaRegUser size={20} />
      ) : (
        <FaRegBuilding size={20} />
      ),
  },
  {
    Header: 'Name',
    id: 'summary',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 80,
    maxWidth: 120,
    Cell: (props: CellProps<IContactSearchResult>) =>
      isPersonResult(props.row.original) ? (
        <strong>{props.row.original.firstName + ' ' + props.row.original.surname}</strong>
      ) : (
        <span></span>
      ),
  },
  {
    Header: 'Organization',
    id: 'organizationName',
    sortable: true,
    align: 'left',
    width: 80,
    maxWidth: 100,
    Cell: (props: CellProps<IContactSearchResult>) => (
      <span>{props.row.original.organizationName}</span>
    ),
  },
  {
    Header: 'Mailing address',
    accessor: 'mailingAddress',
    align: 'left',
    minWidth: 80,
    width: 100,
  },
  {
    Header: 'City',
    accessor: 'municipalityName',
    sortable: true,
    align: 'left',
    minWidth: 50,
    width: 70,
  },
  {
    Header: 'Prov',
    accessor: 'provinceState',
    align: 'left',
    width: 30,
    maxWidth: 50,
  },
];

export default summaryColumns;
