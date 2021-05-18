import { networkSlice } from 'store/slices/network/networkSlice';
import { propertiesSlice } from './slices/properties/propertiesSlice';
import { usersSlice } from './slices/users/usersSlice';
import { lookupCodesSlice } from './slices/lookupCodes/lookupCodesSlice';
import { loadingBarReducer } from 'react-redux-loading-bar';
import projectWorkflowSlice from 'features/projects/common/slices/projectWorkflowSlice';
import projectSlice from 'features/projects/common/slices/projectSlice';
import projectTasksSlice from 'features/projects/common/slices/projectTasksSlice';
import ProjectWorkflowTasksSlice from 'features/projects/common/slices/projectWorkflowTasksSlice';
import erpTabSlice from 'features/projects/erp/slices/erpTabSlice';
import splTabSlice from 'features/projects/spl/slices/splTabSlice';
import projectStatusesSlice from 'features/projects/common/slices/projectStatusesSlice';
import propertyNameSlice from 'features/properties/common/slices/propertyNameSlice';
import { accessRequestsSlice } from 'store/slices/accessRequests';
import { agenciesSlice } from 'store/slices/agencies/agenciesSlice';
import filterSlice from 'store/slices/filter/filterSlice';
import jwtSlice from 'store/slices/jwt/JwtSlice';
import keycloakReadySlice from 'store/slices/keycloakReady/keycloakReadySlice';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import mapViewZoomSlice from 'store/slices/mapViewZoom/mapViewZoomSlice';
import parcelLayerDataSlice from 'store/slices/parcelLayerData/parcelLayerDataSlice';
import { tenantsSlice } from './slices/tenants';

export const reducer = {
  loadingBar: loadingBarReducer,
  [propertiesSlice.name]: propertiesSlice.reducer,
  [usersSlice.name]: usersSlice.reducer,
  [accessRequestsSlice.name]: accessRequestsSlice.reducer,
  [lookupCodesSlice.name]: lookupCodesSlice.reducer,
  [agenciesSlice.name]: agenciesSlice.reducer,
  [networkSlice.name]: networkSlice.reducer,
  [leafletMouseSlice.name]: leafletMouseSlice.reducer,
  [parcelLayerDataSlice.name]: parcelLayerDataSlice.reducer,
  [jwtSlice.name]: jwtSlice.reducer,
  [filterSlice.name]: filterSlice.reducer,
  [projectWorkflowSlice.name]: projectWorkflowSlice.reducer,
  [ProjectWorkflowTasksSlice.name]: ProjectWorkflowTasksSlice.reducer,
  [projectTasksSlice.name]: projectTasksSlice.reducer,
  [projectStatusesSlice.name]: projectStatusesSlice.reducer,
  [projectSlice.name]: projectSlice.reducer,
  [erpTabSlice.name]: erpTabSlice.reducer,
  [splTabSlice.name]: splTabSlice.reducer,
  [keycloakReadySlice.name]: keycloakReadySlice.reducer,
  [mapViewZoomSlice.name]: mapViewZoomSlice.reducer,
  [propertyNameSlice.name]: propertyNameSlice.reducer,
  [tenantsSlice.name]: tenantsSlice.reducer,
};
