import { ColumnWithProps, MoneyCell, Table } from 'components/Table';
import { IBcAssessmentSummary } from 'hooks/useBcAssessmentLayer';
import { CellProps } from 'react-table';
import { formatMoney } from 'utils';

interface IAssessedValuesTableProps {
  valuesData?: IBcAssessmentSummary['SALES'];
}

const AssessedValuesTable: React.FunctionComponent<IAssessedValuesTableProps> = props => {
  return (
    <Table
      columns={columns}
      data={props.valuesData ?? []}
      name="Assessed Values Sales"
      noRowsMessage="No Value data found"
      hideToolbar
    ></Table>
  );
};

const columns: ColumnWithProps<IBcAssessmentSummary['VALUES'][0] & { id?: number }>[] = [
  {
    Header: 'Year',
    accessor: 'BCA_FGPV_SYSID',
    align: 'left',
    width: 20,
    Cell: ({ cell }: CellProps<IBcAssessmentSummary['VALUES'][0], string>) => 'Current',
  },
  {
    Header: 'Property Class',
    accessor: 'GEN_PROPERTY_CLASS_DESC',
    align: 'left',
    width: 40,
    Cell: ({ cell }: CellProps<IBcAssessmentSummary['VALUES'][0], string>) =>
      [
        cell.row.original.GEN_PROPERTY_CLASS_CODE,
        cell.row.original.GEN_PROPERTY_CLASS_DESC,
        cell.row.original.GEN_PROPERTY_SUBCLASS_CODE,
        cell.row.original.GEN_PROPERTY_SUBCLASS_DESC,
      ]
        .filter(a => !!a)
        .join(' - '),
  },
  {
    Header: 'Land',
    accessor: 'GEN_GROSS_LAND_VALUE',
    align: 'left',
    width: 40,
    Cell: MoneyCell,
  },
  {
    Header: 'Improvements',
    accessor: 'GEN_GROSS_IMPROVEMENT_VALUE',
    align: 'left',
    width: 40,
    Cell: MoneyCell,
  },
  {
    Header: 'Total Assessed Value',
    accessor: 'GEN_NET_IMPROVEMENT_VALUE',
    align: 'left',
    width: 60,
    Cell: ({ cell }: CellProps<IBcAssessmentSummary['VALUES'][0], string>) =>
      formatMoney(
        +(cell.row.original.GEN_GROSS_LAND_VALUE ?? 0) +
          +(cell.row.original.GEN_GROSS_IMPROVEMENT_VALUE ?? 0),
      ),
  },
];

export default AssessedValuesTable;
