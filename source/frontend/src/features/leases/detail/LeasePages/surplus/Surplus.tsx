import { useContext, useEffect } from 'react';

import { FormSection } from '@/components/common/form/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { ColumnWithProps, Table } from '@/components/Table';
import { PidCell } from '@/components/Table/PidCell';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { prettyFormatDate, stringToFragment } from '@/utils';

interface IDeclaration {
  id?: number;
  identifier: string;
  declarationType: string;
  date: string;
  comments: string;
}

const columns: ColumnWithProps<IDeclaration>[] = [
  {
    Header: 'PID / Identifier',
    accessor: 'identifier',
    maxWidth: 40,
    Cell: PidCell,
  },
  {
    Header: 'Surplus Declaration',
    accessor: 'declarationType',
    maxWidth: 40,
    Cell: ({ row: { original } }) => {
      const declarationType = original.declarationType;
      const isYesDeclarationType = declarationType.toUpperCase() === 'YES';
      return (
        <>
          {isYesDeclarationType ? (
            <strong className="text-success">{declarationType}</strong>
          ) : (
            <span>{declarationType}</span>
          )}
        </>
      );
    },
  },
  {
    Header: 'Declaration Date',
    accessor: 'date',
    Cell: ({ cell: { value } }) => stringToFragment(prettyFormatDate(value)),
    maxWidth: 50,
  },
  {
    Header: 'Declaration Comments',
    accessor: 'comments',
    minWidth: 150,
  },
];

const Surplus: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const { lease } = useContext(LeaseStateContext);
  const {
    getPropertyLeases: { execute: getPropertyLeases, response: properties, loading },
  } = usePropertyLeaseRepository();
  useEffect(() => {
    lease?.id && getPropertyLeases(lease.id);
  }, [lease, getPropertyLeases]);

  let declarations: IDeclaration[] = (properties ?? []).map<IDeclaration>(x => {
    return {
      id: x.id,
      identifier: x?.property?.pid?.toString() ?? '',
      comments: x?.property?.surplusDeclarationComment || '',
      declarationType: x?.property?.surplusDeclarationType?.description || 'Unknown',
      date: x?.property?.surplusDeclarationDate || '',
    };
  });

  return (
    <FormSection>
      <LoadingBackdrop show={loading} parentScreen />
      <p>
        Data shown is from the Surplus Declaration workflow on the property screen and is not
        directly editable on this screen.
      </p>
      <br />
      <Table<IDeclaration>
        name="leasesTable"
        columns={columns}
        data={declarations ?? []}
        manualPagination
        hideToolbar
        noRowsMessage="Lease / Surplus Declaration details do not exist in PIMS inventory"
      ></Table>
    </FormSection>
  );
};

export default Surplus;
