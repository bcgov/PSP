import { ILeaseSecurityDepositReturn } from 'interfaces';
import { getAllByRole as getAllByRoleBase, render, RenderOptions } from 'utils/test-utils';

import DepositsReturnedTable, { IDepositsReturnedTableProps } from './DepositsReturnedTable';

const mockDepositReturns: ILeaseSecurityDepositReturn[] = [
  {
    id: 1,
    securityDepositTypeId: 'PET',
    securityDepositType: 'Pet deposit',
    terminationDate: '2020-11-15T00:00:00',
    depositTotal: 2084.0,
    claimsAgainst: 200.0,
    returnAmount: 200.0,
    returnDate: '2021-03-25T00:00:00',
    chequeNumber: '20-12780',
    payeeName: 'John Smith',
    payeeAddress: '1020 Skid Row',
    terminationNote: '',
  },
];

// render component under test
const setup = (renderOptions: RenderOptions & IDepositsReturnedTableProps = {}) => {
  const result = render(<DepositsReturnedTable dataSource={renderOptions.dataSource} />, {
    ...renderOptions,
  });

  return {
    ...result,
    findFirstRow: () => {
      const rows = result.getAllByRole('row');
      return rows && rows.length > 1 ? rows[1] : null;
    },
    findCell: (row: HTMLElement, index: number) => {
      const columns = getAllByRoleBase(row, 'cell');
      return columns && columns.length > index ? columns[index] : null;
    },
  };
};

describe('DepositsReturnedTable component', () => {
  it('renders as expected', () => {
    const { asFragment } = setup({ dataSource: [...mockDepositReturns] });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders one row for each security deposit', () => {
    const { getAllByRole } = setup({ dataSource: [...mockDepositReturns] });
    const rows = getAllByRole('row');
    expect(rows.length).toBe(mockDepositReturns.length + 1);
  });

  it('renders security deposit information as expected', () => {
    const depositReturn = mockDepositReturns[0];
    const { findFirstRow, findCell } = setup({ dataSource: [depositReturn] });
    const dataRow = findFirstRow() as HTMLElement;

    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 0)?.textContent).toBe(depositReturn.securityDepositType);
    expect(findCell(dataRow, 1)?.textContent).toBe('Nov 15, 2020');
    expect(findCell(dataRow, 2)?.textContent).toBe('$2,084');
    expect(findCell(dataRow, 3)?.textContent).toBe('$200');
    expect(findCell(dataRow, 4)?.textContent).toBe('$200');
    expect(findCell(dataRow, 5)?.textContent).toBe('Mar 25, 2021');
    expect(findCell(dataRow, 6)?.textContent).toBe(depositReturn.chequeNumber);
    expect(findCell(dataRow, 7)?.textContent).toBe(depositReturn.payeeName);
  });
});
