import { CellProps } from 'react-table';
import styled from 'styled-components';

import { RemoveIconButton } from '@/components/common/buttons';
import { EditButton } from '@/components/common/buttons/EditButton';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps, DateCell } from '@/components/Table';
import { Claims } from '@/constants';
import ReminderContainer from '@/features/notifications/ReminderContainer';
import ReminderView from '@/features/notifications/ReminderView';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_ExpropiationOwnerHistoryType } from '@/models/api/generated/ApiGen_CodeTypes_ExpropiationOwnerHistoryType';
import { ApiGen_CodeTypes_NotificationTypes } from '@/models/api/generated/ApiGen_CodeTypes_NotificationTypes';

import { ExpropriationEventRow } from '../models';

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
      width: 20,
      maxWidth: 20,
      Cell: DateCell,
    },
    {
      Header: 'Owner',
      accessor: 'ownerOrInterestHolder',
      align: 'left',
      sortable: true,
      width: 35,
      maxWidth: 35,
    },
    {
      Header: 'Event',
      accessor: 'eventDescription',
      align: 'left',
      sortable: true,
      width: 20,
      maxWidth: 20,
    },
    {
      Header: 'Actions',
      width: 15,
      maxWidth: 15,
      sortable: false,
      Cell: (props: CellProps<ExpropriationEventRow>) => {
        const { hasClaim } = useKeycloakWrapper();
        const event = props.row.original;

        if (hasClaim(Claims.ACQUISITION_EDIT)) {
          return (
            <ExpropriationActionsDiv>
              {event.eventType === ApiGen_CodeTypes_ExpropiationOwnerHistoryType.APPEFFCTVDT &&
                event.eventDate && (
                  <ReminderContainer
                    keyDate={event.eventDate}
                    keyDateLabel="Appraisal effective date"
                    notificationType={ApiGen_CodeTypes_NotificationTypes.EXPROPH_APPEFFDT}
                    notificationSource={{
                      acquisitionFileId: event.acquisitionFileId,
                      expropOwnerHistoryId: event.id,
                    }}
                    View={ReminderView}
                  />
                )}

              <EditButton
                title="edit expropriation event"
                data-testId={`edit-expropriation-event-${props.row.index}`}
                onClick={() => onUpdate(event.id)}
              />
              <RemoveIconButton
                title="delete expropriation event"
                data-testId={`delete-expropriation-event-${props.row.index}`}
                onRemove={() => onDelete(event.id)}
              />
            </ExpropriationActionsDiv>
          );
        } else if (
          event.eventType === ApiGen_CodeTypes_ExpropiationOwnerHistoryType.APPEFFCTVDT &&
          event.eventDate
        ) {
          <ExpropriationActionsDiv>
            <ReminderContainer
              keyDate={event.eventDate}
              keyDateLabel="Appraisal effective date"
              notificationType={ApiGen_CodeTypes_NotificationTypes.EXPROPH_APPEFFDT}
              notificationSource={{
                acquisitionFileId: event.acquisitionFileId,
                expropOwnerHistoryId: event.id,
              }}
              View={ReminderView}
            />
          </ExpropriationActionsDiv>;
        } else {
          return null;
        }
      },
    },
  ];
};

const ExpropriationActionsDiv = styled(InlineFlexDiv)`
  justify-content: center;
  align-items: center;
  flex-grow: 1;
  align-content: space-between;

  button {
    padding: 0.5rem !important;
  }
`;
