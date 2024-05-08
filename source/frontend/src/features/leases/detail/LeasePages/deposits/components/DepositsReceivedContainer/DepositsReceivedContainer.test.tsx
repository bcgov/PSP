import Claims from '@/constants/claims';
import { getMockDeposits } from '@/mocks/deposits.mock';
import { formatMoney, prettyFormatDate } from '@/utils';
import { getAllByRole as getAllByRoleBase, render, RenderOptions } from '@/utils/test-utils';

import DepositsReceivedContainer, {
  IDepositsReceivedContainerProps,
} from './DepositsReceivedContainer';

const mockVoidCallback = (): void => {};
const mockCallback = (id: number): void => {};

// render component under test
const setup = (renderOptions: RenderOptions & IDepositsReceivedContainerProps) => {
  const result = render(
    <DepositsReceivedContainer
      securityDeposits={renderOptions.securityDeposits}
      onAdd={mockVoidCallback}
      onEdit={mockCallback}
      onDelete={mockCallback}
      onReturn={mockCallback}
    />,
    {
      ...renderOptions,
      organizations: [1],
      useMockAuthentication: true,
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

describe('DepositsReceivedContainer component', () => {
  beforeEach(() => {
    Date.now = vi.fn().mockReturnValue(new Date('2020-11-30T18:33:37.000Z'));
  });
  afterAll(() => {
    vi.restoreAllMocks();
  });
  it('renders as expected', () => {
    const { asFragment } = setup({
      securityDeposits: [...getMockDeposits()],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders one row for each security deposit', () => {
    const { getAllByRole } = setup({
      securityDeposits: [...getMockDeposits()],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });
    const rows = getAllByRole('row');
    expect(rows.length).toBe(getMockDeposits().length + 1);
  });

  it('renders security deposit information as expected', () => {
    const deposit = getMockDeposits()[0];
    const { findFirstRow, findCell } = setup({
      securityDeposits: [deposit],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });
    const dataRow = findFirstRow() as HTMLElement;

    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 0)?.textContent).toBe(deposit.depositType?.description ?? '');
    expect(findCell(dataRow, 1)?.textContent).toBe(deposit.description);
    expect(findCell(dataRow, 2)?.textContent).toBe(formatMoney(deposit.amountPaid));
    expect(findCell(dataRow, 3)?.textContent).toBe(prettyFormatDate(deposit.depositDateOnly));
    expect(findCell(dataRow, 4)?.textContent).toBe('test person');
  });

  it('renders a delete icon when deposit has no returns', () => {
    const deposit = getMockDeposits()[0];
    deposit.depositReturns = [];
    const { findFirstRow, getAllByTitle, queryByTestId } = setup({
      securityDeposits: [deposit],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
      claims: [Claims.LEASE_EDIT, Claims.LEASE_VIEW],
    });
    const dataRow = findFirstRow() as HTMLElement;

    const tooltip = queryByTestId('tooltip-icon');
    expect(dataRow).not.toBeNull();
    expect(getAllByTitle('delete deposit')[0]).toBeVisible();
    expect(tooltip).toBeNull();
  });

  it('renders a tooltip instead of a delete icon if a deposit has a return', () => {
    const deposit = getMockDeposits()[0];
    deposit.depositReturns = [{} as any];
    const { findFirstRow, queryByTitle, getByTestId } = setup({
      securityDeposits: [deposit],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
      claims: [Claims.LEASE_EDIT, Claims.LEASE_VIEW],
    });
    const dataRow = findFirstRow() as HTMLElement;

    const tooltip = getByTestId('tooltip-icon-no-delete-tooltip-1');
    expect(dataRow).not.toBeNull();
    expect(queryByTitle('delete deposit')).toBeNull();
    expect(tooltip).toBeVisible();
    expect(tooltip.id).toBe('no-delete-tooltip-1');
  });

  it('renders security deposit return holders as links', () => {
    const { getByText } = setup({
      securityDeposits: getMockDeposits(),
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });

    expect(getByText('test organization')).toHaveAttribute('href', '/contact/O1');
    expect(getByText('test person')).toHaveAttribute('href', '/contact/P1');
  });
});
