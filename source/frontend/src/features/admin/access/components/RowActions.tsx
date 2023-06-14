import { FiMoreHorizontal } from 'react-icons/fi';
import { CellProps } from 'react-table';

import { Menu } from '@/components/menu/Menu';
import { AccessRequestStatus } from '@/constants/accessStatus';
import { FormAccessRequest } from '@/features/admin/access-request/models';
import { useAccessRequests } from '@/hooks/pims-api/useAccessRequests';

export const RowActions = (props: CellProps<FormAccessRequest> & { refresh: () => void }) => {
  const accessRequest = props.row.original;
  const {
    updateAccessRequest: { execute: update },
    removeAccessRequest: { execute: remove },
  } = useAccessRequests();

  const isStatusMatch = (value: AccessRequestStatus) =>
    accessRequest.accessRequestStatusTypeCodeId === value;

  const approve = async () => {
    if (accessRequest) {
      await update({
        ...accessRequest.toApi(),
        accessRequestStatusTypeCode: { id: AccessRequestStatus.Approved },
      });
      props.refresh();
    }
  };
  const decline = async () => {
    if (accessRequest) {
      await update({
        ...accessRequest.toApi(),
        accessRequestStatusTypeCode: { id: AccessRequestStatus.Denied },
      });
      props.refresh();
    }
  };

  const hold = async () => {
    if (accessRequest) {
      await update({
        ...accessRequest.toApi(),
        accessRequestStatusTypeCode: { id: AccessRequestStatus.Received },
      });
      props.refresh();
    }
  };

  const deleteRequest = async () => {
    if (accessRequest) {
      await remove({
        ...accessRequest.toApi(),
        accessRequestStatusTypeCode: { id: AccessRequestStatus.Denied },
      });
      props.refresh();
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
