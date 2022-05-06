import { useKeycloak } from '@react-keycloak/web';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from 'models/api/SecurityDeposit';
import { formatMoney, prettyFormatDate } from 'utils';
import { getAllByRole as getAllByRoleBase, render, RenderOptions } from 'utils/test-utils';

import DepositsReturnedContainer, {
  IDepositsReturnedContainerProps,
} from './DepositsReturnedContainer';

const mockCallback = (id: number): void => {};

const mockDepositReturns: Api_SecurityDepositReturn[] = [
  {
    id: 1,
    parentDepositId: 7,
    terminationDate: '2022-02-01',
    claimsAgainst: 1234.0,
    returnAmount: 123.0,
    interestPaid: 1.0,
    returnDate: '2022-02-16',
    rowVersion: 1,
  },
];

const mockDeposit: Api_SecurityDeposit = {
  id: 7,
  description: 'Test deposit 1',
  amountPaid: 1234.0,
  depositDate: '2022-02-09',
  depositType: {
    id: 'PET',
    description: 'Pet deposit',
    isDisabled: false,
  },
  depositReturns: mockDepositReturns,
  rowVersion: 1,
};

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      organizations: [1],
      roles: [],
    },
    subject: 'test',
  },
});

// render component under test
const setup = (renderOptions: RenderOptions & IDepositsReturnedContainerProps) => {
  const result = render(
    <DepositsReturnedContainer
      securityDeposits={renderOptions.securityDeposits}
      depositReturns={renderOptions.depositReturns}
      onEdit={mockCallback}
      onDelete={mockCallback}
    />,
    {
      ...renderOptions,
    },
  );

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

describe('DepositsReturnedContainer component', () => {
  it('renders as expected', () => {
    const { asFragment } = setup({
      securityDeposits: [mockDeposit],
      depositReturns: [...mockDepositReturns],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders one row for each security deposit return', () => {
    const { getAllByRole } = setup({
      securityDeposits: [mockDeposit],
      depositReturns: [...mockDepositReturns],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });
    const rows = getAllByRole('row');
    expect(rows.length).toBe(mockDepositReturns.length + 1);
  });

  it('renders security deposit returns information as expected', () => {
    const depositReturn = mockDepositReturns[0];
    const { findFirstRow, findCell } = setup({
      securityDeposits: [mockDeposit],
      depositReturns: [depositReturn],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });
    const dataRow = findFirstRow() as HTMLElement;

    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 0)?.textContent).toBe(mockDeposit.depositType.description);
    expect(findCell(dataRow, 1)?.textContent).toBe(prettyFormatDate(depositReturn.terminationDate));
    expect(findCell(dataRow, 2)?.textContent).toBe(formatMoney(mockDeposit.amountPaid));
    expect(findCell(dataRow, 3)?.textContent).toBe(formatMoney(depositReturn.claimsAgainst));
    expect(findCell(dataRow, 4)?.textContent).toBe(formatMoney(depositReturn.returnAmount));
    expect(findCell(dataRow, 5)?.textContent).toBe(formatMoney(depositReturn.interestPaid));
    expect(findCell(dataRow, 6)?.textContent).toBe(prettyFormatDate(depositReturn.returnDate));
  });
});
