import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import useIsMounted from 'hooks/useIsMounted';
import { Api_User } from 'models/api/User';
import * as React from 'react';
import { useHistory } from 'react-router-dom';

import { useUsers } from '../users/hooks/useUsers';
import { FormUser } from '../users/models';
import EditUserForm from './EditUserForm';

export interface IEditUserContainerProps {
  userId?: string;
}

const EditUserContainer: React.FunctionComponent<
  React.PropsWithChildren<IEditUserContainerProps>
> = ({ userId }) => {
  const history = useHistory();
  const [user, setUser] = React.useState<Api_User>();
  const isMounted = useIsMounted();
  const {
    updateUser: { execute: updateUserDetail },
    fetchUserDetail: { execute: getUserDetail, loading },
  } = useUsers();

  React.useEffect(() => {
    const asyncFunc = async () => {
      if (userId) {
        const response = await getUserDetail(userId);
        if (isMounted()) {
          setUser(response);
        }
      }
    };
    asyncFunc();
  }, [userId, getUserDetail, isMounted]);

  const goBack = () => {
    history.goBack();
  };

  const initialValues: FormUser = {
    businessIdentifierValue: '',
    firstName: '',
    surname: '',
    email: '',
    isDisabled: false,
    rowVersion: 0,
    roles: [],
    regions: [],
    note: '',
    position: '',
    lastLogin: '',
    toApi: () => ({} as Api_User),
  };
  const formUser = user !== undefined ? new FormUser(user) : initialValues;
  return (
    <>
      <LoadingBackdrop parentScreen show={loading} />

      <EditUserForm
        updateUserDetail={async (updateUser: Api_User) => {
          const response = await updateUserDetail(updateUser);
          if (isMounted()) {
            setUser(response);
          }
        }}
        formUser={formUser}
        onCancel={goBack}
      />
    </>
  );
};

export default EditUserContainer;
