import { Table } from 'components/Table';
import { ILeaseSecurityDeposit } from 'interfaces';

import { columns } from './columns';

export interface IDepositsReceivedTableProps {
  dataSource?: ILeaseSecurityDeposit[];
}

const DepositsReceivedTable: React.FC<IDepositsReceivedTableProps> = ({ dataSource }) => {
  return (
    <Table<ILeaseSecurityDeposit>
      name="securityDepositsTable"
      columns={columns}
      data={dataSource ?? []}
      manualPagination={false}
      hideToolbar={true}
      noRowsMessage="There is no corresponding data"
    />
  );
};

export default DepositsReceivedTable;
