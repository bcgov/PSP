import { CellProps } from 'react-table';
import styled from 'styled-components';

import { RemoveIconButton } from '@/components/common/buttons';
import { EditButton } from '@/components/common/buttons/EditButton';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps, DateCell } from '@/components/Table';
import { Claims } from '@/constants';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

import { ExpropriationEventRow } from '../../models';

export const getExpropriationEventColumns = (
  onUpdate: (expropriationEventId: number) => void,
  onDelete: (expropriationEventId: number) => void,
): ColumnWithProps<ExpropriationEventRow>[] => {
  return [
    {
      Header: 'Date',
      accessor: 'eventDate',
      align: 'left',
      sortable: true,
      width: 10,
      maxWidth: 20,
      Cell: DateCell,
    },
    {
      Header: 'Owner',
      accessor: 'ownerOrInterestHolder',
      align: 'left',
      sortable: true,
      width: 10,
      maxWidth: 20,
    },
    {
      Header: 'Event',
      accessor: 'eventDescription',
      align: 'left',
      sortable: true,
      width: 40,
      maxWidth: 40,
    },
    {
      Header: 'Actions',
      width: 20,
      maxWidth: 20,
      sortable: false,
      Cell: (cell: CellProps<ExpropriationEventRow>) => {
        const { hasClaim } = useKeycloakWrapper();

        if (hasClaim(Claims.ACQUISITION_EDIT)) {
          return (
            <ExpropriationActionsDiv>
              <EditButton
                title="edit expropriation event"
                data-testId={`edit-expropriation-event-${cell.row.index}`}
                onClick={() => onUpdate(cell.row.original.id)}
              />
              <RemoveIconButton
                title="delete expropriation event"
                data-testId={`delete-expropriation-event-${cell.row.index}`}
                onRemove={() => onDelete(cell.row.original.id)}
              />
            </ExpropriationActionsDiv>
          );
        } else {
          return null;
        }
      },
    },
  ];
};

const ExpropriationActionsDiv = styled(InlineFlexDiv)`
  justify-content: center;
  gap: 1rem;
  align-items: center;
  flex-grow: 1;
  align-content: space-between;
`;
