import { IPagedItems, IAccessRequest } from 'interfaces';
import { IAccessRequestsFilterData, IAccessRequestsSort } from '.';

export interface IAccessRequestsState {
  pagedAccessRequests: IPagedItems<IAccessRequest>;
  filter: IAccessRequestsFilterData;
  selections: string[];
  sorting: IAccessRequestsSort;
  accessRequest: IAccessRequest | null;
  pageSize: number;
  pageIndex: number;
}
