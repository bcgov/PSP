import { Section } from '@/components/common/Section/Section';
import { Table } from '@/components/Table';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { ApiGen_Concepts_SecurityDepositReturn } from '@/models/api/generated/ApiGen_Concepts_SecurityDepositReturn';

import { getColumns, ReturnListEntry } from './columns';

export interface IDepositsReturnedContainerProps {
  securityDeposits: ApiGen_Concepts_SecurityDeposit[];
  depositReturns: ApiGen_Concepts_SecurityDepositReturn[];
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
}

const DepositsReturnedContainer: React.FC<
  React.PropsWithChildren<IDepositsReturnedContainerProps>
> = ({ securityDeposits, depositReturns, onEdit, onDelete }) => {
  const columns = getColumns({ onEdit, onDelete });
  const dataSource = depositReturns.reduce(
    (accumulator: ReturnListEntry[], returnDeposit: ApiGen_Concepts_SecurityDepositReturn) => {
      var parentDeposit = securityDeposits.find(r => r?.id === returnDeposit?.parentDepositId);
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
