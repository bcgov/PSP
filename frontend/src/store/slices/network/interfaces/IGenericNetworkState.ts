import { IGenericNetworkAction } from './IGenericNetworkAction';

export interface IGenericNetworkState {
  [key: string]: IGenericNetworkAction;
}
