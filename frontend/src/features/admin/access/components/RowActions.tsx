import { CellProps } from 'react-table';
import { AccessRequestStatus } from 'constants/accessStatus';
import { Menu } from 'components/menu/Menu';
import React from 'react';
import { FiMoreHorizontal } from 'react-icons/fi';
import { IAccessRequestModel } from '../interfaces';
import { useStore } from 'react-redux';
import { RootState } from 'reducers/rootReducer';
import { useAccessRequests } from 'store/slices/accessRequests/useAccessRequests';

export const RowActions = (props: CellProps<IAccessRequestModel>) => {
  const accessRequest = props.row.original;
  const store = useStore();
  const { updateAccessRequest, removeAccessRequest } = useAccessRequests();

  const isStatusMatch = (value: AccessRequestStatus) => accessRequest.status === value;

  const storedAccessRequest = (store.getState() as RootState).accessRequests.pagedAccessRequests.items.find(
    ar => ar.id === accessRequest.id,
  );
  const originalAccessRequest = storedAccessRequest ? { ...storedAccessRequest } : undefined;

  const approve = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.Approved;
      updateAccessRequest(originalAccessRequest);
    }
  };
  const decline = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.Declined;
      updateAccessRequest(originalAccessRequest);
    }
  };

  const hold = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.OnHold;
      updateAccessRequest(originalAccessRequest);
    }
  };

  const deleteRequest = () => {
    if (originalAccessRequest) {
      originalAccessRequest.status = AccessRequestStatus.OnHold;
      removeAccessRequest(originalAccessRequest.id, originalAccessRequest);
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
          disabled: isStatusMatch(AccessRequestStatus.OnHold),
          onClick: hold,
        },
        {
          label: 'Decline',
          disabled: isStatusMatch(AccessRequestStatus.Declined),
          onClick: decline,
        },
        { label: 'Delete', onClick: deleteRequest },
      ]}
    >
      <FiMoreHorizontal />
    </Menu>
  );
};
