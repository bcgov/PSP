import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import Active from '@/assets/images/active.svg?react';
import Inactive from '@/assets/images/inactive.svg?react';
import { Select, SelectOption } from '@/components/common/form';
import { ColumnWithProps } from '@/components/Table';
import { getPrimaryContact } from '@/features/contacts/contactUtils';
import { isValidId } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { FormTenant } from './models';

const getColumns = (tenantTypes: SelectOption[]): ColumnWithProps<FormTenant>[] => {
  return [
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
      width: 16,
      maxWidth: 16,
      Cell: (props: CellProps<FormTenant>) =>
        isValidId(props.row.original.personId) ? (
          <FaRegUser size={16} />
        ) : (
          <FaRegBuilding size={16} />
        ),
    },
    {
      Header: 'Summary',
      accessor: 'summary',
      align: 'left',
      clickable: true,
      width: 120,
      maxWidth: 150,
      Cell: (props: CellProps<FormTenant>) => (
        <Link to={`/contact/${props.row.original.id}`} target="_blank" rel="noopener noreferrer">
          {props.row.original.summary}
        </Link>
      ),
    },
    {
      Header: 'Primary contact',
      accessor: 'primaryContactId',
      align: 'left',
      minWidth: 150,
      width: 120,
      Cell: (props: CellProps<FormTenant>) => {
        const original = props.row.original;
        const persons = original?.organizationPersons?.map(op => op.person);
        let primaryContact = original.initialPrimaryContact;
        if (original.primaryContactId !== original.initialPrimaryContact?.id) {
          primaryContact = original.primaryContactId
            ? getPrimaryContact(original.primaryContactId, original) ?? undefined
            : undefined;
        }
        const primaryContactOptions: SelectOption[] =
          persons?.map(person => ({
            label: formatApiPersonNames(person),
            value: person?.id ?? 0,
          })) ?? [];
        if (isValidId(props?.row?.original?.personId)) {
          return <p>Not applicable</p>;
        } else if (persons?.length && persons?.length > 1) {
          return (
            <Select
              key={`tenants.primaryContact.${persons[0]?.id ?? props?.row?.index}`}
              field={`tenants.${props.row.index}.primaryContactId`}
              type="number"
              options={primaryContactOptions}
              placeholder="Select a contact"
            ></Select>
          );
        } else if (persons?.length === 1) {
          return (
            <p key={`tenants.primaryContact.${persons[0]?.id ?? props?.row?.index}`}>
              {formatApiPersonNames(primaryContact ?? persons[0])}
            </p>
          );
        } else {
          return <p>No contacts available</p>;
        }
      },
    },
    {
      Header: 'Contact info',
      accessor: 'mailingAddress',
      align: 'left',
      minWidth: 80,
      width: 100,
      Cell: (props: CellProps<FormTenant>) => {
        return (
          <div>
            <p>{props.row.original.email}</p>
            <p>{props.row.original.mailingAddress?.streetAddress1}</p>
            <p>
              {props.row.original.municipalityName} {props.row.original.provinceState}
            </p>
          </div>
        );
      },
    },
    {
      Header: 'Contact type',
      accessor: 'email',
      align: 'left',
      minWidth: 80,
      width: 100,
      Cell: (props: CellProps<FormTenant>) => {
        return (
          <Select
            field={`tenants.${props.row.index}.tenantType`}
            type="number"
            options={tenantTypes}
          ></Select>
        );
      },
    },
  ];
};
export default getColumns;
