import { FormSection } from 'components/common/form/styles';
import { ColumnWithProps, Table } from 'components/Table';
import { getIn, useFormikContext } from 'formik';
import { ILease, IProperty } from 'interfaces';
import { prettyFormatDate } from 'utils';
import { withNameSpace } from 'utils/formUtils';

interface IDeclaration {
  id: number;
  identifier: string;
  declarationType: string;
  date: string;
  comments: string;
}

const columns: ColumnWithProps<IDeclaration>[] = [
  {
    Header: 'PID / Identifier',
    accessor: 'identifier',
    align: 'left',
    maxWidth: 40,
  },
  {
    Header: 'Surplus Declaration',
    accessor: 'declarationType',
    align: 'left',
    maxWidth: 40,
  },
  {
    Header: 'Declaration Date',
    accessor: 'date',
    align: 'left',
    Cell: ({ cell: { value } }) => prettyFormatDate(value),
    maxWidth: 50,
  },
  {
    Header: 'Declaration Comments',
    accessor: 'comments',
    align: 'left',
    minWidth: 150,
  },
];

const Surplus: React.FunctionComponent<{}> = () => {
  const { values } = useFormikContext<ILease>();
  const organizations: IProperty[] = getIn(values, withNameSpace(undefined, 'properties')) ?? [];

  let declarations: IDeclaration[] = organizations.map<IDeclaration>(x => {
    return {
      id: x.id || 1,
      identifier: x.pid,
      comments: x.surplusDeclaration?.comment || '',
      declarationType: x.surplusDeclaration?.typeDescription || 'No',
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
