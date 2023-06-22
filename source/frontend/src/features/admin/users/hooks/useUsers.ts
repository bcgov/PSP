import * as actionTypes from '@/constants/actionTypes';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';

/**
 * hook that wraps calls to the users api.
 */
export const useUsers = () => {
  const { getUsersPaged, activateUser, putUser, getUser, exportUsers } = useApiUsers();

  /**
   * fetch all of the users from the server based on a filter.
   * @return the filtered, paged list of users.
   */
  const activate = useApiRequestWrapper({
    requestFunction: activateUser,
    requestName: actionTypes.ADD_ACTIVATE_USER,
    throwError: true,
  });

  /**
   * fetch all of the users from the server based on a filter.
   * @return the filtered, paged list of users.
   */
  const fetch = useApiRequestWrapper({
    requestFunction: getUsersPaged,
    requestName: actionTypes.GET_USERS,
  });

  /**
   * fetch the detailed user based on the user id.
   * @return the detailed user.
   */
  const fetchDetail = useApiRequestWrapper({
    requestFunction: getUser,
    requestName: actionTypes.GET_USER_DETAIL,
  });

  /**
   * Update an existing user based on its id.
   * @return the updated user.
   */
  const update = useApiRequestWrapper({
    requestFunction: putUser,
    requestName: actionTypes.PUT_USER_DETAIL,
  });

  /**
   * export a filtered list of users to a file.
   * @return the updated user.
   */
  const exportList = useApiRequestWrapper({
    requestFunction: exportUsers,
    requestName: actionTypes.EXPORT_USER,
  });

  return {
    fetchUsers: fetch,
    fetchUserDetail: fetchDetail,
    updateUser: update,
    activateUser: activate,
    exportUsers: exportList,
  };
};

export const NEW_PIMS_USER = 201;
