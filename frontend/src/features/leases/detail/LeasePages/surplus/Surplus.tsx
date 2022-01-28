import { FormSection } from 'components/common/form/styles';
import { ColumnWithProps, Table } from 'components/Table';
import { getIn, useFormikContext } from 'formik';
import { ILease, IProperty } from 'interfaces';
import { prettyFormatDate } from 'utils';

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
    Cell: ({ cell: { value } }) => prettyFormatDate(value),
    maxWidth: 50,
  },
  {
    Header: 'Declaration Comments',
    accessor: 'comments',
    minWidth: 150,
  },
];

const Surplus: React.FunctionComponent = () => {
  const { values } = useFormikContext<ILease>();
  const properties: IProperty[] = getIn(values, 'properties') ?? [];

  let declarations: IDeclaration[] = properties.map<IDeclaration>(x => {
    return {
      id: x.id,
      identifier: x.pid,
      comments: x.surplusDeclaration?.comment || '',
      declarationType: x.surplusDeclaration?.type?.description || 'Unknown',
      date: x.surplusDeclaration?.date || '',
    };
  });

  return (
    <FormSection>
      <p>
        Data shown is from the Surplus Declaration workflow on the property screen and is not
        directly editable on this screen.
      </p>
      <br />
      <Table<IDeclaration>
        name="leasesTable"
        columns={columns}
        data={declarations ?? []}
        manualPagination={false}
        hideToolbar={true}
        noRowsMessage="Lease / Surplus Declaration details do not exist in PIMS inventory"
      ></Table>
    </FormSection>
  );
};

export default Surplus;
