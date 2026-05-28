import { configureStore } from '@reduxjs/toolkit';
import { loadingBarMiddleware } from 'react-redux-loading-bar';
import logger from 'redux-logger';

import { reducer } from './rootReducer';
import { telemetryMiddleware } from './telemetryMiddleware';

export const store = configureStore({
  reducer: reducer,
  middleware: (getDefaultMiddleware: () => any[]) =>
    getDefaultMiddleware()
      .concat(telemetryMiddleware)
      .concat(logger)
      .concat(loadingBarMiddleware()),
  devTools: import.meta.env.PROD === false,
});
export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
