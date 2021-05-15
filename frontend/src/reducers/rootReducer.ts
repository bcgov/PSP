import { propertiesSlice } from './../store/slices/properties/propertiesSlice';
import { usersSlice } from './../store/slices/users/usersSlice';
import { lookupCodesSlice } from './../store/slices/lookupCodes/lookupCodesSlice';
import { combineReducers } from 'redux';
import * as reducerTypes from 'constants/reducerTypes';
import networkReducer from './networkReducer';
import { loadingBarReducer } from 'react-redux-loading-bar';
import leafletMouseSlice from './LeafletMouseSlice';
import jwtSlice from './JwtSlice';
import keycloakReadySlice from './keycloakReadySlice';
import mapViewZoomSlice from './mapViewZoomSlice';
import projectWorkflowSlice from 'features/projects/common/slices/projectWorkflowSlice';
import { ProjectReducers } from 'constants/reducerTypes';
import projectSlice from 'features/projects/common/slices/projectSlice';
import projectTasksSlice from 'features/projects/common/slices/projectTasksSlice';
import ProjectWorkflowTasksSlice from 'features/projects/common/slices/projectWorkflowTasksSlice';
import erpTabSlice from 'features/projects/erp/slices/erpTabSlice';
import splTabSlice from 'features/projects/spl/slices/splTabSlice';
import projectStatusesSlice from 'features/projects/common/slices/projectStatusesSlice';
import propertyNameSlice from 'features/properties/common/slices/propertyNameSlice';
import filterSlice from './filterSlice';
import parcelLayerDataSlice from './parcelLayerDataSlice';
import { accessRequestsSlice } from 'store/slices/accessRequests';
import { agenciesSlice } from 'store/slices/agencies/agenciesSlice';

export const reducerObject = {
  loadingBar: loadingBarReducer,
  [reducerTypes.PROPERTIES]: propertiesSlice.reducer,
  [reducerTypes.USERS]: usersSlice.reducer,
  [reducerTypes.ACCESS_REQUEST]: accessRequestsSlice.reducer,
  [reducerTypes.LOOKUP_CODE]: lookupCodesSlice.reducer,
  [reducerTypes.AGENCIES]: agenciesSlice.reducer,
  [reducerTypes.NETWORK]: networkReducer,
  [reducerTypes.LEAFLET_CLICK_EVENT]: leafletMouseSlice.reducer,
  [reducerTypes.PARCEL_LAYER_DATA]: parcelLayerDataSlice.reducer,
  [reducerTypes.JWT]: jwtSlice.reducer,
  [reducerTypes.FILTER]: filterSlice.reducer,
  [ProjectReducers.WORKFLOW]: projectWorkflowSlice.reducer,
  [ProjectReducers.WORKFLOW_TASKS]: ProjectWorkflowTasksSlice.reducer,
  [ProjectReducers.TASKS]: projectTasksSlice.reducer,
  [ProjectReducers.STATUSES]: projectStatusesSlice.reducer,
  [ProjectReducers.PROJECT]: projectSlice.reducer,
  [ProjectReducers.ERP_TAB]: erpTabSlice.reducer,
  [ProjectReducers.SPL_TAB]: splTabSlice.reducer,
  [reducerTypes.KEYCLOAK_READY]: keycloakReadySlice.reducer,
  [reducerTypes.MAP_VIEW_ZOOM]: mapViewZoomSlice.reducer,
  [reducerTypes.PROPERTY_NAMES]: propertyNameSlice.reducer,
};

export const rootReducer = combineReducers(reducerObject);

export type RootState = ReturnType<typeof rootReducer>;
