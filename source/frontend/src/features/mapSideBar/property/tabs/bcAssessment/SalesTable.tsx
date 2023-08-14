import { CellProps } from 'react-table';

import { ColumnWithProps, Table } from '@/components/Table';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { formatMoney, prettyFormatDate, stringToFragment } from '@/utils';

interface ISalesTableProps {
  salesData?: IBcAssessmentSummary['SALES'];
}

const SalesTable: React.FunctionComponent<ISalesTableProps> = props => {
  return (
    <Table
      columns={columns}
      data={props.salesData ?? []}
      name="Property Sales"
      noRowsMessage="No Sale data found"
      hideToolbar
    ></Table>
  );
};

type BcAssessmentSalesModelType = IBcAssessmentSummary['SALES'][0] & { id?: number };

const columns: ColumnWithProps<BcAssessmentSalesModelType>[] = [
  {
    Header: 'Description',
    accessor: 'CONVEYANCE_DATE',
    align: 'left',
    width: 40,
    Cell: ({ cell }: CellProps<BcAssessmentSalesModelType, string | undefined>) =>
      stringToFragment(
        `A ${cell.row.original?.CONVEYANCE_TYPE_DESCRIPTION} occurred on ${prettyFormatDate(
          cell.row.original?.CONVEYANCE_DATE,
        )}. The sale price was ${formatMoney(
          cell.row.original?.CONVEYANCE_PRICE,
        )}. The document # was ${cell.row.original?.DOCUMENT_NUMBER}`,
      ),
  },
];

export default SalesTable;
