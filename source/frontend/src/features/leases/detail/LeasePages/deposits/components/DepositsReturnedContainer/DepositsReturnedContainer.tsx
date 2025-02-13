import { Section } from '@/components/common/Section/Section';
import { Table } from '@/components/Table';
import { LeaseStatusUpdateSolver } from '@/features/leases/models/LeaseStatusUpdateSolver';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { ApiGen_Concepts_SecurityDepositReturn } from '@/models/api/generated/ApiGen_Concepts_SecurityDepositReturn';

import { getColumns, ReturnListEntry } from './columns';

export interface IDepositsReturnedContainerProps {
  securityDeposits: ApiGen_Concepts_SecurityDeposit[];
  depositReturns: ApiGen_Concepts_SecurityDepositReturn[];
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  statusSolver?: LeaseStatusUpdateSolver;
}

const DepositsReturnedContainer: React.FC<
  React.PropsWithChildren<IDepositsReturnedContainerProps>
> = ({ securityDeposits, depositReturns, statusSolver, onEdit, onDelete }) => {
  const columns = getColumns({
    onEdit,
    onDelete,
    isFileFinalStatus: !statusSolver.canEditDeposits(),
  });
  const dataSource = depositReturns.reduce(
    (accumulator: ReturnListEntry[], returnDeposit: ApiGen_Concepts_SecurityDepositReturn) => {
      const parentDeposit = securityDeposits.find(r => r?.id === returnDeposit?.parentDepositId);
      if (parentDeposit) {
        accumulator.push(new ReturnListEntry(returnDeposit, parentDeposit));
      }
      return accumulator;
    },
    [],
  );
  return (
    <Section header="Deposits Returned">
      <Table<ReturnListEntry>
        name="securityDepositReturnsTable"
        columns={columns}
        data={dataSource}
        manualPagination
        hideToolbar
        noRowsMessage="There is no corresponding data"
      />
    </Section>
  );
};

export default DepositsReturnedContainer;
