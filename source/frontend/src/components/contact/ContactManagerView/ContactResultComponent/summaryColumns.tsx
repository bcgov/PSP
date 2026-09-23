import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { PiSealCheckFill } from 'react-icons/pi';
import { CellProps } from 'react-table';

import { TooltipWrapper } from '@/components/common/TooltipWrapper';
import { ColumnWithProps } from '@/components/Table';
import { IContactSearchResult, isPersonSummary, isPIMSUserSummary } from '@/interfaces';

const summaryColumns: ColumnWithProps<IContactSearchResult>[] = [
  {
    Header: '',
    id: 'id',
    align: 'center',
    width: 20,
    maxWidth: 20,
    Cell: (props: CellProps<IContactSearchResult>) =>
      isPIMSUserSummary(props.row.original) ? (
        <>
          <TooltipWrapper tooltipId={`pims-user-${props.row.original.id}`} tooltip="PIMS User">
            <PiSealCheckFill size={20} />
          </TooltipWrapper>
        </>
      ) : isPersonSummary(props.row.original) ? (
        <>
          <TooltipWrapper tooltipId={`person-${props.row.original.id}`} tooltip="Individual">
            <FaRegUser size={20} />
          </TooltipWrapper>
        </>
      ) : (
        <>
          <TooltipWrapper
            tooltipId={`organization-${props.row.original.id}`}
            tooltip="Organization"
          >
            <FaRegBuilding size={20} />
          </TooltipWrapper>
        </>
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
      isPersonSummary(props.row.original) ? (
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
