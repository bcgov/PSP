import { toast } from 'react-toastify';

import { MAP_UNAVAILABLE_STR, QUERY_MAP } from './strings';

/**
 * The purpose of this file is to centralize the toasts in use in the application in one location. This should allow us to minimize duplication of toast messages using toastIds. https://fkhadra.github.io/react-toastify/prevent-duplicate
 * In the future we may need to split this file out (at least into /features).
 */

/** These toasts are used by the update user api */
const USER_UPDATING_TOAST_ID = 'UPDATING_USER';
const USER_UPDATING = () => toast.dark('Updating User...', { toastId: USER_UPDATING_TOAST_ID });
const USER_UPDATED_TOAST_ID = 'USER_UPDATED';
const USER_UPDATED = () => toast.dark('User updated', { toastId: USER_UPDATED_TOAST_ID });
const USER_ERROR_TOAST_ID = 'USER_ERROR';
const USER_ERROR = () => toast.error('Failed to update User', { toastId: USER_ERROR_TOAST_ID });
export const user = {
  USER_UPDATING_TOAST_ID,
  USER_UPDATING,
  USER_UPDATED_TOAST_ID,
  USER_UPDATED,
  USER_ERROR_TOAST_ID,
  USER_ERROR,
};

/** These toasts are used to display bc data warehouse loading */
const LAYER_DATA_LOADING_ID = 'LOADING_LAYER_DATA';
const LAYER_DATA_LOADING = () => toast.dark(QUERY_MAP, { toastId: LAYER_DATA_LOADING_ID });
const LAYER_DATA_ERROR_ID = 'LAYER_DATA_ERROR_ID';
const LAYER_DATA_ERROR = () => toast.error(MAP_UNAVAILABLE_STR, { toastId: LAYER_DATA_ERROR_ID });
export const layerData = {
  LAYER_DATA_LOADING_ID,
  LAYER_DATA_LOADING,
  LAYER_DATA_ERROR_ID,
  LAYER_DATA_ERROR,
};
