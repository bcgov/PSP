import * as actionTypes from '@/constants/actionTypes';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';

import { useApiAccessRequests } from './useApiAccessRequests';

export const useAccessRequests = () => {
  const {
    getAccessRequest,
    getAccessRequestById,
    getAccessRequestsPaged,
    postAccessRequest,
    putAccessRequest,
    deleteAccessRequest,
  } = useApiAccessRequests();
  /**
   * Get the fetchCurrent function with returns the current access request for the current user.
   * @returns The dispatchable action which will return the access request if one exists, or 204 if one doesn't
   */
  const fetchCurrent = useApiRequestWrapper({
    requestFunction: getAccessRequest,
    requestName: actionTypes.GET_REQUEST_ACCESS,
  });

  /**
   * Get the fetchById function which returns the access request corresponding to the passed id for the current user.
   * @returns The dispatchable action which will return the access request if one exists, or 204 if one doesn't
   */
  const fetchById = useApiRequestWrapper({
    requestFunction: getAccessRequestById,
    requestName: actionTypes.GET_REQUEST_ACCESS,
  });

  /**
   * Get the storeAccessRequest action.
   * If the 'accessRequest' is new return the POST action.
   * If the 'accessRequest' exists, return the PUT action.
   * @param accessRequest - The access request to add
   * @returns The action function to submit the access request
   */
  const add = useApiRequestWrapper({
    requestFunction: postAccessRequest,
    requestName: actionTypes.ADD_REQUEST_ACCESS,
  });

  /**
   * Get the update action which updates the passed access request (therefore it must have an id)
   * @returns The dispatchable action which will update the access request.
   */
  const update = useApiRequestWrapper({
    requestFunction: putAccessRequest,
    requestName: actionTypes.UPDATE_REQUEST_ACCESS_ADMIN,
  });

  const fetch = useApiRequestWrapper({
    requestFunction: getAccessRequestsPaged,
    requestName: actionTypes.GET_REQUEST_ACCESS,
  });

  /**
   * Get the remove action which will delete the passed access request with matching id.
   * @returns The dispatchable action which will delete the matching access request.
   */
  const remove = useApiRequestWrapper({
    requestFunction: deleteAccessRequest,
    requestName: actionTypes.DELETE_REQUEST_ACCESS_ADMIN,
  });

  return {
    addAccessRequest: add,
    updateAccessRequest: update,
    fetchAccessRequests: fetch,
    fetchCurrentAccessRequest: fetchCurrent,
    fetchAccessRequestById: fetchById,
    removeAccessRequest: remove,
  };
};
