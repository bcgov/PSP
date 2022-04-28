import { loadingBarReducer } from 'react-redux-loading-bar';
import { accessRequestsSlice } from 'store/slices/accessRequests';
import filterSlice from 'store/slices/filter/filterSlice';
import jwtSlice from 'store/slices/jwt/JwtSlice';
import keycloakReadySlice from 'store/slices/keycloakReady/keycloakReadySlice';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import mapViewZoomSlice from 'store/slices/mapViewZoom/mapViewZoomSlice';
import { networkSlice } from 'store/slices/network/networkSlice';

import { lookupCodesSlice } from './slices/lookupCodes/lookupCodesSlice';
import { systemConstantsSlice } from './slices/systemConstants/systemConstantsSlice';
import { tenantsSlice } from './slices/tenants';
import { usersSlice } from './slices/users/usersSlice';

export const reducer = {
  loadingBar: loadingBarReducer,
  [usersSlice.name]: usersSlice.reducer,
  [accessRequestsSlice.name]: accessRequestsSlice.reducer,
  [lookupCodesSlice.name]: lookupCodesSlice.reducer,
  [systemConstantsSlice.name]: systemConstantsSlice.reducer,
  [networkSlice.name]: networkSlice.reducer,
  [leafletMouseSlice.name]: leafletMouseSlice.reducer,
  [jwtSlice.name]: jwtSlice.reducer,
  [filterSlice.name]: filterSlice.reducer,
  [keycloakReadySlice.name]: keycloakReadySlice.reducer,
  [mapViewZoomSlice.name]: mapViewZoomSlice.reducer,
  [tenantsSlice.name]: tenantsSlice.reducer,
};
