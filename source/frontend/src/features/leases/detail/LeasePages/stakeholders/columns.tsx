import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { FaCircle } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { Select, SelectOption } from '@/components/common/form';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps } from '@/components/Table';
import { getPrimaryContact } from '@/features/contacts/contactUtils';
import { isValidId } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { FormStakeholder } from './models';

const getColumns = (
  stakeholderTypes: SelectOption[],
  isPayableLease: boolean,
): ColumnWithProps<FormStakeholder>[] => {
  const stakeholderType = isPayableLease ? 'Payee type' : 'Contact type';
  return [
    {
      Header: (
        <TooltipIcon
          toolTipId="stakeholder-status"
          toolTip="Green dot indicates contact is active"
        ></TooltipIcon>
      ),
      accessor: 'isDisabled',
      align: 'right',
      width: 16,
      maxWidth: 16,
      minWidth: 16,
      Cell: (props: CellProps<FormStakeholder>) => {
        const stakeholderStatus = props.row.original;
        const status =
          stakeholderStatus.original !== undefined
            ? stakeholderStatus.original.id.startsWith('O') === true
              ? stakeholderStatus.original.organization.isDisabled
              : stakeholderStatus.isDisabled
            : stakeholderStatus.isDisabled;
        return (
          <StatusIndicators className={status ? 'inactive' : 'active'}>
            <FaCircle size={10} className="mr-2" />
          </StatusIndicators>
        );
      },
    },
    {
      Header: '',
      accessor: 'leaseStakeholderId',
      align: 'right',
      width: 16,
      maxWidth: 16,
      Cell: (props: CellProps<FormStakeholder>) => {
        const stakeholderId = props.row.original;
        const status =
          stakeholderId.original !== undefined
            ? stakeholderId.original.id.startsWith('O') === true
              ? stakeholderId.original.organization.isDisabled
              : stakeholderId.isDisabled
            : stakeholderId.isDisabled;
        return isValidId(props.row.original.personId) ? (
          <StatusIndicators className={status ? 'inactive' : 'active'}>
            <FaRegUser size={16} />
          </StatusIndicators>
        ) : (
          <StatusIndicators className={status ? 'inactive' : 'active'}>
            <FaRegBuilding size={16} />
          </StatusIndicators>
        );
      },
    },
    {
      Header: 'Summary',
      accessor: 'summary',
      align: 'left',
      clickable: true,
      width: 120,
      maxWidth: 150,
      Cell: (props: CellProps<FormStakeholder>) => (
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
      Cell: (props: CellProps<FormStakeholder>) => {
        const stakeholder = props.row.original;
        const persons =
          stakeholder.original !== undefined && isValidId(stakeholder.organizationId)
            ? stakeholder?.original.organization.organizationPersons?.map(op => op.person)
            : stakeholder?.organizationPersons?.map(op => op.person);
        let initialPrimaryContact = stakeholder.initialPrimaryContact;
        if (Number(stakeholder.primaryContactId) !== initialPrimaryContact?.id) {
          initialPrimaryContact = stakeholder.primaryContactId
            ? getPrimaryContact(Number(stakeholder.primaryContactId), stakeholder) ?? undefined
            : undefined;
        }
        const primaryContactOptions: SelectOption[] =
          persons?.map(person => ({
            label: formatApiPersonNames(person),
            value: person?.id ?? 0,
          })) ?? [];
        if (isValidId(stakeholder?.personId)) {
          return <p>Not applicable</p>;
        } else if (persons?.length && persons?.length > 1) {
          return (
            <Select
              key={`stakeholders.primaryContact.${persons[0]?.id ?? props?.row?.index}`}
              field={`stakeholders.${props.row.index}.primaryContactId`}
              type="number"
              options={primaryContactOptions}
              placeholder="Select a contact"
            ></Select>
          );
        } else if (persons?.length === 1) {
          return (
            <p key={`stakeholders.primaryContact.${persons[0]?.id ?? props?.row?.index}`}>
              {formatApiPersonNames(initialPrimaryContact ?? persons[0])}
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
      Cell: (props: CellProps<FormStakeholder>) => {
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
      Header: stakeholderType,
      accessor: 'stakeholderType',
      align: 'left',
      minWidth: 80,
      width: 100,
      Cell: (props: CellProps<FormStakeholder>) => {
        return (
          <Select
            field={`stakeholders.${props.row.index}.stakeholderType`}
            type="number"
            options={stakeholderTypes}
          ></Select>
        );
      },
    },
  ];
};

export const StatusIndicators = styled.div`
  color: ${props => props.theme.css.borderOutlineColor};
  &.active {
    color: ${props => props.theme.bcTokens.iconsColorSuccess};
  }
`;

export default getColumns;
