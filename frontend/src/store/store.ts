import { reducer } from './rootReducer';
import { loadingBarMiddleware } from 'react-redux-loading-bar';
import logger from 'redux-logger';
import { configureStore } from '@reduxjs/toolkit';
import { useDispatch, TypedUseSelectorHook, useSelector } from 'react-redux';

export const store = configureStore({
  reducer: reducer,
  middleware: (getDefaultMiddleware: () => any[]) =>
    getDefaultMiddleware()
      .concat(logger)
      .concat(loadingBarMiddleware()),
  devTools: process.env.NODE_ENV !== 'production',
});
export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
