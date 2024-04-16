import * as React from 'react';
import { useHistory } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { UserTypes } from '@/constants/index';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';
import { toTypeCode } from '@/utils/formUtils';

import { useUsers } from '../users/hooks/useUsers';
import { FormUser } from '../users/models';
import EditUserForm from './EditUserForm';

export interface IEditUserContainerProps {
  userId?: string;
}

const EditUserContainer: React.FunctionComponent<IEditUserContainerProps> = ({ userId }) => {
  const history = useHistory();
  const [user, setUser] = React.useState<ApiGen_Concepts_User>();
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
    userTypeCode: toTypeCode(UserTypes.Contractor),
    lastLogin: '',
    toApi: () => ({} as ApiGen_Concepts_User),
  };
  const formUser = user !== undefined ? new FormUser(user) : initialValues;
  return (
    <>
      <LoadingBackdrop parentScreen show={loading} />

      <EditUserForm
        updateUserDetail={async (updateUser: ApiGen_Concepts_User) => {
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
