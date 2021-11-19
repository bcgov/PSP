import { Table } from 'components/Table';
import { ILeaseSecurityDepositReturn } from 'interfaces';

import { columns } from './columns';

export interface IDepositsReturnedTableProps {
  dataSource?: ILeaseSecurityDepositReturn[];
}

const DepositsReturnedTable: React.FC<IDepositsReturnedTableProps> = ({ dataSource }) => {
  return (
    <Table<ILeaseSecurityDepositReturn>
      name="securityDepositReturnsTable"
      columns={columns}
      data={dataSource ?? []}
      manualPagination={false}
      hideToolbar={true}
      noRowsMessage="There is no corresponding data"
    />
  );
};

export default DepositsReturnedTable;
