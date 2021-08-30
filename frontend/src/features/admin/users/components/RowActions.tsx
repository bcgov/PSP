import { Menu } from 'components/menu/Menu';
import { IUser } from 'interfaces';
import { FiMoreHorizontal } from 'react-icons/fi';
import { useHistory } from 'react-router-dom';
import { CellProps } from 'react-table';
import { useAppSelector } from 'store/hooks';
import { useUsers } from 'store/slices/users';

import { IUserRecord } from '../interfaces/IUserRecord';

export const RowActions = (props: CellProps<IUserRecord>) => {
  const history = useHistory();
  const { updateUser } = useUsers();

  const users = useAppSelector(state => state.users.pagedUsers.items);
  const getUser = (): IUser | undefined =>
    users.find((user: IUser) => user.id === props.row.original.id);

  const changeAccountStatus = async (disabled: boolean) => {
    const user = getUser();
    if (user) {
      const tempUser = { ...user, isDisabled: disabled };
      await updateUser(tempUser);
    }
  };
  const enableUser = async () => {
    await changeAccountStatus(false);
  };

  const disableUser = async () => {
    await changeAccountStatus(true);
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
