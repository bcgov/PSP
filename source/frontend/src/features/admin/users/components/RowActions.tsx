import { FiMoreHorizontal } from 'react-icons/fi';
import { useHistory } from 'react-router-dom';
import { CellProps } from 'react-table';

import { Menu } from '@/components/menu/Menu';

import { useUsers } from '../hooks/useUsers';
import { FormUser } from '../models';

export const RowActions = (props: CellProps<FormUser> & { refresh: () => void }) => {
  const history = useHistory();

  const {
    updateUser: { execute: updateUser },
  } = useUsers();

  const changeAccountStatus = async (disabled: boolean) => {
    const user = props.row.original;
    if (user) {
      const tempUser = { ...user.toApi(), isDisabled: disabled };
      await updateUser(tempUser);
      props.refresh();
    }
  };
  const enableUser = async () => {
    await changeAccountStatus(false);
    props.refresh();
  };

  const disableUser = async () => {
    await changeAccountStatus(true);
    props.refresh();
  };

  const openUserDetails = () => {
    history.push(`/admin/user/${props.row.original.id}`);
  };

  const isLastRow = props.row.original.id === props.data[props.data.length - 1].id;

  return (
    <Menu
      alignTop={isLastRow && props.data.length >= 20}
      disableScroll={true}
      options={[
        {
          label: 'Enable',
          disabled: !props.row.original.isDisabled,
          onClick: enableUser,
        },
        {
          label: 'Disable',
          disabled: props.row.original.isDisabled,
          onClick: disableUser,
        },
        {
          label: 'Open',
          onClick: openUserDetails,
        },
      ]}
    >
      <FiMoreHorizontal />
    </Menu>
  );
};
