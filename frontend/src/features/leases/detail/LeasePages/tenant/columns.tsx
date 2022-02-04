import { ReactComponent as Active } from 'assets/images/active.svg';
import { ReactComponent as Inactive } from 'assets/images/inactive.svg';
import { NotesModal } from 'components/common/form/NotesModal';
import { ColumnWithProps } from 'components/Table';
import { IContactSearchResult } from 'interfaces';
import React from 'react';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

const columns = [
  {
    Header: '',
    accessor: 'isDisabled',
    align: 'right',
    width: 10,
    maxWidth: 10,
    minWidth: 10,
    Cell: (props: CellProps<IContactSearchResult>) =>
      props.row.original.isDisabled ? <Inactive /> : <Active />,
  },
  {
    Header: '',
    accessor: 'leaseTenantId',
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
    width: 80,
    maxWidth: 120,
    Cell: (props: CellProps<IContactSearchResult>) => (
      <Link to={`/contact/${props.row.original.id}`}>{props.row.original.summary}</Link>
    ),
  },
  {
    Header: 'Last Name',
    accessor: 'surname',
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'First Name',
    accessor: 'firstName',
    align: 'left',
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'Organization',
    accessor: 'organizationName',
    align: 'left',
    width: 80,
    maxWidth: 100,
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
    minWidth: 80,
    width: 100,
  },
  {
    Header: 'City',
    accessor: 'municipalityName',
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
  {
    Header: 'Notes',
    accessor: 'note',
    align: 'left',
    width: 30,
    maxWidth: 50,
    Cell: (props: CellProps<IContactSearchResult>) => (
      <NotesModal
        nameSpace={`tenants.${props.row.index}`}
        notesLabel={
          <p>
            Notes pertaining to <b>{props.row.original.summary}</b>
          </p>
        }
        title="Tenant Notes"
      />
    ),
  },
] as ColumnWithProps<IContactSearchResult>[];

export default columns;
