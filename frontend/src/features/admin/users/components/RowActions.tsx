import { IUserRecord } from '../interfaces/IUserRecord';
import { IUser } from 'interfaces';
import { Menu } from 'components/menu/Menu';
import { FiMoreHorizontal } from 'react-icons/fi';
import { CellProps } from 'react-table';
import { useHistory } from 'react-router-dom';
import { useUsers } from 'store/slices/users';
import { useAppSelector } from 'store/hooks';

export const RowActions = (props: CellProps<IUserRecord>) => {
  const history = useHistory();
  const { updateUser } = useUsers();

  const users = useAppSelector(state => state.users.pagedUsers.items);
  const getUser = (): IUser | undefined =>
    users.find((user: IUser) => user.id === props.row.original.id);

  const changeAccountStatus = async (disabled: boolean) => {
    const user = { ...getUser() };
    if (user) {
      user.isDisabled = disabled;
      await updateUser({ id: props.row.original.id }, user);
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
