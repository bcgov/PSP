import { ReactComponent as Active } from 'assets/images/active.svg';
import { ReactComponent as Inactive } from 'assets/images/inactive.svg';
import { ColumnWithProps } from 'components/Table';
import { IContactSearchResult } from 'interfaces';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

const columns: ColumnWithProps<IContactSearchResult>[] = [
  {
    Header: '',
    accessor: 'isDisabled',
    align: 'right',
    width: 20,
    maxWidth: 20,
    Cell: (props: CellProps<IContactSearchResult>) =>
      props.row.original.isDisabled ? <Inactive /> : <Active />,
  },
  {
    Header: '',
    accessor: 'id',
    align: 'right',
    width: 20,
    maxWidth: 20,
    Cell: (props: CellProps<IContactSearchResult>) =>
      props.row.original.personId !== undefined ? (
        <FaRegUser size={20} />
      ) : (
        <FaRegBuilding size={20} />
      ),
  },
  {
    Header: 'Summary',
    accessor: 'summary',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 80,
    maxWidth: 120,
    Cell: (props: CellProps<IContactSearchResult>) => (
      <Link to={`/contact/${props.row.original.id}`}>{props.row.original.summary}</Link>
    ),
  },
  {
    Header: 'Last Name',
    accessor: 'surname',
    clickable: true,
    sortable: true,
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'First Name',
    accessor: 'firstName',
    clickable: true,
    sortable: true,
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'Organization',
    accessor: 'organizationName',
    clickable: true,
    sortable: true,
    align: 'left',
    width: 80,
    maxWidth: 120,
  },
  {
    Header: 'E-mail',
    accessor: 'email',
    align: 'left',
    minWidth: 80,
    width: 100,
  },
  {
    Header: 'Mailing Address',
    accessor: 'mailingAddress',
    align: 'left',
    minWidth: 100,
    width: 150,
  },
  {
    Header: 'City',
    accessor: 'municipality',
    clickable: true,
    sortable: true,
    align: 'left',
    minWidth: 50,
    width: 70,
  },
  {
    Header: 'Prov',
    accessor: 'provinceState',
    align: 'left',
    width: 50,
    maxWidth: 70,
  },
];

export default columns;
