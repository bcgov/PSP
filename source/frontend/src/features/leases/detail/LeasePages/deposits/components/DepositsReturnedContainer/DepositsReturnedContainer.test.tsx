import { useKeycloak } from '@react-keycloak/web';

import { getMockDepositReturns, getMockDeposits } from '@/mocks/deposits.mock';
import { formatMoney, prettyFormatDate } from '@/utils';
import { getAllByRole as getAllByRoleBase, render, RenderOptions } from '@/utils/test-utils';

import DepositsReturnedContainer, {
  IDepositsReturnedContainerProps,
} from './DepositsReturnedContainer';

const mockCallback = (id: number): void => {};

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
      securityDeposits: [...getMockDeposits()],
      depositReturns: [...getMockDepositReturns()],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders one row for each security deposit return', () => {
    const { getAllByRole } = setup({
      securityDeposits: [...getMockDeposits()],
      depositReturns: [...getMockDepositReturns()],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });
    const rows = getAllByRole('row');
    expect(rows.length).toBe(getMockDepositReturns().length + 1);
  });

  it('renders security deposit returns information as expected', () => {
    const depositReturn = getMockDepositReturns()[0];
    const { findFirstRow, findCell } = setup({
      securityDeposits: [...getMockDeposits()],
      depositReturns: [depositReturn],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });
    const dataRow = findFirstRow() as HTMLElement;

    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 0)?.textContent).toBe(
      getMockDeposits()[0].depositType.description ?? '',
    );
    expect(findCell(dataRow, 1)?.textContent).toBe(prettyFormatDate(depositReturn.terminationDate));
    expect(findCell(dataRow, 2)?.textContent).toBe(formatMoney(getMockDeposits()[1].amountPaid));
    expect(findCell(dataRow, 3)?.textContent).toBe(formatMoney(depositReturn.claimsAgainst));
    expect(findCell(dataRow, 4)?.textContent).toBe(formatMoney(depositReturn.returnAmount));
    expect(findCell(dataRow, 5)?.textContent).toBe(formatMoney(depositReturn.interestPaid));
    expect(findCell(dataRow, 6)?.textContent).toBe(prettyFormatDate(depositReturn.returnDate));
    expect(findCell(dataRow, 7)?.textContent).toBe(
      depositReturn.contactHolder?.organization?.name ?? '',
    );
  });

  it('renders security deposit return holders as links', () => {
    const depositReturn = getMockDepositReturns()[0];
    const { getByText } = setup({
      securityDeposits: [...getMockDeposits()],
      depositReturns: [
        {
          ...depositReturn,
          contactHolder: { id: 'O1', organization: { name: 'test organization' } },
        },
        {
          ...depositReturn,
          contactHolder: { id: 'P1', person: { firstName: 'test', surname: 'person' } },
        },
      ],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });

    expect(getByText('test organization')).toHaveAttribute('href', '/contact/O1');
    expect(getByText('test person')).toHaveAttribute('href', '/contact/P1');
  });
});
