import { Button } from 'components/common/buttons/Button';
import { Table } from 'components/Table';
import Claims from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_SecurityDeposit } from 'models/api/SecurityDeposit';

import { DepositListEntry, getColumns } from './columns';

export interface IDepositsReceivedContainerProps {
  securityDeposits: Api_SecurityDeposit[];
  onAdd: () => void;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onReturn: (id: number) => void;
}

const DepositsReceivedContainer: React.FC<
  React.PropsWithChildren<IDepositsReceivedContainerProps>
> = ({ securityDeposits, onAdd, onEdit, onDelete, onReturn }) => {
  const { hasClaim } = useKeycloakWrapper();
  const columns = getColumns({ onEdit, onDelete, onReturn });
  const dataSource = securityDeposits.map<DepositListEntry>(d => {
    return new DepositListEntry(d);
  });
  return (
    <Section header="Deposits Received">
      <Button
        variant="secondary"
        onClick={() => onAdd()}
        className="mb-4 px-5"
        disabled={!hasClaim(Claims.LEASE_ADD)}
      >
        Add a deposit
      </Button>
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
