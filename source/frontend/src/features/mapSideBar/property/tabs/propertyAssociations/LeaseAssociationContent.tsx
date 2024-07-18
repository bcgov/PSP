import { sortBy } from 'lodash';
import find from 'lodash/find';
import orderBy from 'lodash/orderBy';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { ColumnWithProps, renderDate, Table } from '@/components/Table';
import { getCalculatedExpiry } from '@/features/leases/leaseUtils';
import { ApiGen_CodeTypes_LeaseTenantTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseTenantTypes';
import { ApiGen_Concepts_Association } from '@/models/api/generated/ApiGen_Concepts_Association';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';
import { formatApiPersonNames } from '@/utils/personUtils';

interface IAssociationInfo {
  id: string;
  linkUrl: string;
  fileIdentifier: string;
  fileName: string;
  createdBy: string;
  createdByGuid: string;
  createdDate: string;
  status: string;
  tenants: string;
  expiryDate: string;
}

export interface ILeaseAssociationContentProps {
  associationName: string;
  tenants: ApiGen_Concepts_LeaseTenant[];
  renewals: ApiGen_Concepts_LeaseRenewal[];
  leases: ApiGen_Concepts_Lease[];
  associations?: ApiGen_Concepts_Association[];
  linkUrlMask: string;
}

const getFormattedTenants = (tenants: ApiGen_Concepts_LeaseTenant[]) => {
  if (tenants.length === 0) {
    return '';
  }
  const sortOrder = [
    { type: ApiGen_CodeTypes_LeaseTenantTypes.ASGN, order: 1 },
    { type: ApiGen_CodeTypes_LeaseTenantTypes.TEN, order: 2 },
    { type: ApiGen_CodeTypes_LeaseTenantTypes.UNK, order: 3 },
  ];
  const filteredTenants: ApiGen_Concepts_LeaseTenant[] = tenants.filter(t =>
    sortOrder.map(t => t.type.toString()).includes(t.tenantTypeCode.id),
  );
  const sortedTenants: ApiGen_Concepts_LeaseTenant[] = sortBy(
    filteredTenants,
    tenant => sortOrder.find(s => s.type === tenant.tenantTypeCode.id)?.order,
  );
  const tenantTypeCode = sortedTenants[0]?.tenantTypeCode?.id;

  return sortedTenants
    .filter(t => t.tenantTypeCode.id === tenantTypeCode)
    .map(t => (t.lessorType?.id === 'PER' ? formatApiPersonNames(t.person) : t.organization?.name))
    .join(', ');
};

export const LeaseAssociationContent: React.FunctionComponent<
  React.PropsWithChildren<ILeaseAssociationContentProps>
> = props => {
  const noDataMessage = `There are no ${props.associationName} files available`;
  if (props.associations === undefined) {
    return <>{noDataMessage}</>;
  }
  const tableData = orderBy(
    props.associations.map<IAssociationInfo>(x => {
      const lease = find(props.leases, lease => x.id === lease.id);
      const leaseRenewals = props.renewals?.filter(renewal => x.id === renewal.leaseId);
      const calculatedExpiry = getCalculatedExpiry(lease, leaseRenewals ?? []);
      return {
        id: x.id?.toString() || '',
        linkUrl: props.linkUrlMask.replace('|id|', x.id?.toString() || ''),
        fileIdentifier: x.fileNumber || '',
        fileName: x.fileName || '',
        createdBy: x.createdBy || '',
        createdByGuid: x.createdByGuid || '',
        createdDate: x.createdDateTime || '',
        status: x.status || '',
        tenants: getFormattedTenants(props.tenants?.filter(tenant => x.id === tenant.leaseId)),
        expiryDate: calculatedExpiry,
      };
    }),
    (association: IAssociationInfo) => {
      return association.createdDate;
    },
    'desc',
  );

  return (
    <Table<IAssociationInfo>
      name="associationFiles"
      columns={associationColumns}
      data={tableData ?? []}
      manualSortBy={true}
      noRowsMessage={noDataMessage}
      hideToolbar
    ></Table>
  );
};

const associationColumns: ColumnWithProps<IAssociationInfo>[] = [
  {
    Header: 'File #',
    accessor: 'fileIdentifier',
    align: 'left',
    Cell: (props: CellProps<IAssociationInfo>) => {
      return <Link to={props.row.original.linkUrl}>{props.row.original.fileIdentifier}</Link>;
    },
    width: 50,
  },
  {
    Header: 'Tenant name',
    accessor: 'tenants',
    align: 'left',
    Cell: (props: CellProps<IAssociationInfo>) => {
      return <p>{props.row.original.tenants}</p>;
    },
  },
  {
    Header: 'Created by',
    accessor: 'createdBy',
    align: 'left',
    width: 50,
    Cell: (props: CellProps<IAssociationInfo>) => {
      return (
        <UserNameTooltip
          userName={props.row.original?.createdBy}
          userGuid={props.row.original?.createdByGuid}
        />
      );
    },
  },
  {
    Header: 'Expiry date',
    accessor: 'expiryDate',
    align: 'left',
    Cell: renderDate,
    width: 80,
  },
  {
    Header: 'Status',
    accessor: 'status',
    align: 'left',
    width: 60,
  },
];
