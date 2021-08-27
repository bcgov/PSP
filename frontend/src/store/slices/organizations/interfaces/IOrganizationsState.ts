import { TableSort } from 'components/Table/TableSort';
import { IOrganization, IOrganizationFilter, IOrganizationRecord, IPagedItems } from 'interfaces';

export interface IOrganizationsState {
  pagedOrganizations: IPagedItems<IOrganization>;
  rowsPerPage: number;
  filter: IOrganizationFilter;
  sort: TableSort<IOrganizationRecord>;
  pageIndex: number;
  organizationDetail: IOrganization;
}
