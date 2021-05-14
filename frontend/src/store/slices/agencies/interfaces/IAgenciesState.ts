import { TableSort } from 'components/Table/TableSort';
import { IPagedItems, IAgency, IAgencyFilter, IAgencyRecord, IAgencyDetail } from 'interfaces';

export interface IAgenciesState {
  pagedAgencies: IPagedItems<IAgency>;
  rowsPerPage: number;
  filter: IAgencyFilter;
  sort: TableSort<IAgencyRecord>;
  pageIndex: number;
  agencyDetail: IAgencyDetail;
}
