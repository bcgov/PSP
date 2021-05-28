import { TableSort } from 'components/Table/TableSort';
import { IAgency, IAgencyDetail, IAgencyFilter, IAgencyRecord, IPagedItems } from 'interfaces';

export interface IAgenciesState {
  pagedAgencies: IPagedItems<IAgency>;
  rowsPerPage: number;
  filter: IAgencyFilter;
  sort: TableSort<IAgencyRecord>;
  pageIndex: number;
  agencyDetail: IAgencyDetail;
}
