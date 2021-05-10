import configureStore from 'configureStore';

export const store = configureStore();
export type AppDispatch = typeof store.dispatch;
