import { ILeaseSecurityDeposit } from 'interfaces';
import { getAllByRole as getAllByRoleBase, render, RenderOptions } from 'utils/test-utils';

import DepositsReceivedTable, { IDepositsReceivedTableProps } from './DepositsReceivedTable';

const mockDeposits: ILeaseSecurityDeposit[] = [
  {
    id: 1,
    securityDepositHolderTypeId: 'MINISTRY',
    securityDepositHolderType: 'Ministry',
    securityDepositTypeId: 'PET',
    securityDepositType: 'Pet deposit',
    description: 'Pet deposit collected for one cat and one medium size dog.',
    amountPaid: 500.0,
    totalAmount: 521.0,
    depositDate: '2021-09-15T00:00:00',
    annualInterestRate: 2.1,
  },
  {
    id: 2,
    securityDepositHolderTypeId: 'MINISTRY',
    securityDepositHolderType: 'Ministry',
    securityDepositTypeId: 'SECURITY',
    securityDepositType: 'Security deposit',
    description:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit. \r\n\r\nInteger nec odio.',
    amountPaid: 2000.0,
    totalAmount: 2084.0,
    depositDate: '2019-03-01T00:00:00',
    annualInterestRate: 2.1,
  },
];

// render component under test
const setup = (renderOptions: RenderOptions & IDepositsReceivedTableProps = {}) => {
  const result = render(<DepositsReceivedTable dataSource={renderOptions.dataSource} />, {
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

describe('DepositsReceivedTable component', () => {
  beforeEach(() => {
    Date.now = jest.fn().mockReturnValue(new Date('2020-11-30T18:33:37.000Z'));
  });
  afterAll(() => {
    jest.restoreAllMocks();
  });
  it('renders as expected', () => {
    const { asFragment } = setup({ dataSource: [...mockDeposits] });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders one row for each security deposit', () => {
    const { getAllByRole } = setup({ dataSource: [...mockDeposits] });
    const rows = getAllByRole('row');
    expect(rows.length).toBe(mockDeposits.length + 1);
  });

  it('renders security deposit information as expected', () => {
    const deposit = mockDeposits[0];
    const { findFirstRow, findCell } = setup({ dataSource: [deposit] });
    const dataRow = findFirstRow() as HTMLElement;

    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 0)?.textContent).toBe(deposit.securityDepositType);
    expect(findCell(dataRow, 1)?.textContent).toBe(deposit.description);
    expect(findCell(dataRow, 2)?.textContent).toBe('$500');
    expect(findCell(dataRow, 3)?.textContent).toBe('Sep 15, 2021');
    expect(findCell(dataRow, 4)?.textContent).toBe(deposit.securityDepositHolderType);
    expect(findCell(dataRow, 5)?.textContent).toBe('2.1%');
  });
});
