import { useDispatch, TypedUseSelectorHook, useSelector } from 'react-redux';
import { AppDispatch } from './store';
import { RootState } from 'reducers/rootReducer';

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
