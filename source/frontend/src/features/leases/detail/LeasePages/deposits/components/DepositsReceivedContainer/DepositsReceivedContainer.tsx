import { FaPlus } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { Table } from '@/components/Table';
import Claims from '@/constants/claims';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';

import { DepositListEntry, getColumns } from './columns';

export interface IDepositsReceivedContainerProps {
  securityDeposits: ApiGen_Concepts_SecurityDeposit[];
  onAdd: () => void;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onReturn: (id: number) => void;
}

const DepositsReceivedContainer: React.FC<
  React.PropsWithChildren<IDepositsReceivedContainerProps>
> = ({ securityDeposits, onAdd, onEdit, onDelete, onReturn }) => {
  const columns = getColumns({ onEdit, onDelete, onReturn });
  const dataSource = securityDeposits.map<DepositListEntry>(d => {
    return new DepositListEntry(d);
  });
  return (
    <Section
      header={
        <SectionListHeader
          claims={[Claims.LEASE_ADD]}
          title="Deposits Received"
          addButtonText="Add a deposit"
          addButtonIcon={<FaPlus size={'2rem'} />}
          onAdd={() => {
            onAdd();
          }}
        />
      }
    >
      <Table<DepositListEntry>
        name="securityDepositsTable"
        columns={columns}
        data={dataSource}
        manualPagination
        hideToolbar
        noRowsMessage="There is no corresponding data"
      />
    </Section>
  );
};

export default DepositsReceivedContainer;
