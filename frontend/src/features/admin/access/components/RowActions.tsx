import { Menu } from 'components/menu/Menu';
import { AccessRequestStatus } from 'constants/accessStatus';
import React from 'react';
import { FiMoreHorizontal } from 'react-icons/fi';
import { CellProps } from 'react-table';
import { useAppSelector } from 'store/hooks';
import { useAccessRequests } from 'store/slices/accessRequests/useAccessRequests';

import { IAccessRequestModel } from '../interfaces';

export const RowActions = (props: CellProps<IAccessRequestModel>) => {
  const accessRequest = props.row.original;
  const { updateAccessRequest, removeAccessRequest } = useAccessRequests();

  const isStatusMatch = (value: AccessRequestStatus) => accessRequest.status === value;

  const accessRequests = useAppSelector(state => state.accessRequests.pagedAccessRequests.items);
  const storedAccessRequest = accessRequests.find(ar => ar.id === accessRequest.id);
  const originalAccessRequest = storedAccessRequest ? { ...storedAccessRequest } : undefined;

  const approve = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.Approved;
      updateAccessRequest(originalAccessRequest);
    }
  };
  const decline = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.Denied;
      updateAccessRequest(originalAccessRequest);
    }
  };

  const hold = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.Received;
      updateAccessRequest(originalAccessRequest);
    }
  };

  const deleteRequest = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.Denied;
      removeAccessRequest(originalAccessRequest.id ?? 0, originalAccessRequest);
    }
  };

  const isLastRow = accessRequest.id === props.data[props.data.length - 1].id;

  return (
    <Menu
      alignTop={isLastRow && props.data.length >= 20}
      disableScroll={true}
      options={[
        {
          label: 'Approve',
          disabled: isStatusMatch(AccessRequestStatus.Approved),
          onClick: approve,
        },
        {
          label: 'Hold',
          disabled: isStatusMatch(AccessRequestStatus.Received),
          onClick: hold,
        },
        {
          label: 'Decline',
          disabled: isStatusMatch(AccessRequestStatus.Denied),
          onClick: decline,
        },
        { label: 'Delete', onClick: deleteRequest },
      ]}
    >
      <FiMoreHorizontal />
    </Menu>
  );
};
