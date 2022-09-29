import { ReactComponent as Active } from 'assets/images/active.svg';
import { ReactComponent as Inactive } from 'assets/images/inactive.svg';
import { Select, SelectOption } from 'components/common/form';
import { NotesModal } from 'components/common/form/NotesModal';
import { ColumnWithProps } from 'components/Table';
import { getPrimaryContact } from 'features/contacts/contactUtils';
import noop from 'lodash/noop';
import React from 'react';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import { formatApiPersonNames } from 'utils/personUtils';

import { FormTenant } from './Tenant';

const columns = [
  {
    Header: '',
    accessor: 'isDisabled',
    align: 'right',
    width: 10,
    maxWidth: 10,
    minWidth: 10,
    Cell: (props: CellProps<FormTenant>) =>
      props.row.original.isDisabled ? <Inactive /> : <Active />,
  },
  {
    Header: '',
    accessor: 'leaseTenantId',
    align: 'right',
    width: 20,
    maxWidth: 20,
    Cell: (props: CellProps<FormTenant>) =>
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
    Cell: (props: CellProps<FormTenant>) => (
      <Link to={`/contact/${props.row.original.id}`}>{props.row.original.summary}</Link>
    ),
  },
  {
    Header: 'Primary contact',
    accessor: 'primaryContactId',
    align: 'left',
    minWidth: 200,
    width: 300,
    Cell: (props: CellProps<FormTenant>) => {
      const original = props.row.original;
      const persons = original?.organizationPersons?.map(op => op.person);
      let primaryContact = original.initialPrimaryContact;
      if (original.primaryContactId !== original.initialPrimaryContact?.id) {
        primaryContact = original.primaryContactId
          ? getPrimaryContact(original.primaryContactId, original)
          : undefined;
      }
      const primaryContactOptions: SelectOption[] =
        persons?.map(person => ({
          label: formatApiPersonNames(person),
          value: person?.id ?? 0,
        })) ?? [];
      if (!!props?.row?.original?.personId) {
        return <p>Not applicable</p>;
      } else if (persons?.length && persons?.length > 1) {
        return (
          <Select
            field={`tenants.${props.row.index}.primaryContactId`}
            type="number"
            options={primaryContactOptions}
            placeholder="Select a contact"
          ></Select>
        );
      } else if (persons?.length === 1) {
        return <p>{formatApiPersonNames(primaryContact ?? persons[0])}</p>;
      } else {
        return <p>No contacts available</p>;
      }
    },
  },
  {
    Header: 'E-mail',
    accessor: 'email',
    align: 'left',
    minWidth: 80,
    width: 100,
  },
  {
    Header: 'Mailing address',
    accessor: 'mailingAddress',
    align: 'left',
    minWidth: 80,
    width: 100,
    Cell: (props: CellProps<FormTenant>) => {
      return <p>{props.row.original.mailingAddress?.streetAddress1}</p>;
    },
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
    Cell: (props: CellProps<FormTenant>) => (
      <NotesModal
        nameSpace={`tenants.${props.row.index}`}
        notesLabel={
          <p>
            Notes pertaining to <b>{props.row.original.summary}</b>
          </p>
        }
        title="Tenant Notes"
        onSave={noop}
      />
    ),
  },
] as ColumnWithProps<FormTenant>[];

export default columns;
