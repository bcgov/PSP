import { useKeycloak } from '@react-keycloak/web';
import Claims from 'constants/claims';
import { Api_SecurityDeposit } from 'models/api/SecurityDeposit';
import { formatMoney, prettyFormatDate } from 'utils';
import {
  getAllByRole as getAllByRoleBase,
  getAllByTitle,
  prettyDOM,
  render,
  RenderOptions,
} from 'utils/test-utils';

import DepositsReceivedContainer, {
  IDepositsReceivedContainerProps,
} from './DepositsReceivedContainer';

const mockVoidCallback = (): void => {};
const mockCallback = (id: number): void => {};

const mockDeposits: Api_SecurityDeposit[] = [
  {
    id: 1,
    description: 'Test deposit 1',
    amountPaid: 1234.0,
    depositDate: '2022-02-09',
    depositType: {
      id: 'PET',
      description: 'Pet deposit',
      isDisabled: false,
    },
    depositReturns: [],
    rowVersion: 1,
    contactHolder: { id: 'O3', organization: { name: 'test organization' } },
  },
  {
    id: 2,
    description: 'Test deposit 2',
    amountPaid: 444.0,
    depositDate: '2022-02-09',
    depositType: {
      id: 'OTHER',
      description: 'Other deposit',
      isDisabled: false,
    },
    otherTypeDescription: 'TestCustomDeposit',
    depositReturns: [],
    rowVersion: 1,
    contactHolder: { id: 'P1', person: { firstName: 'test', surname: 'person' } },
  },
];

jest.mock('@react-keycloak/web');

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
    Date.now = jest.fn().mockReturnValue(new Date('2020-11-30T18:33:37.000Z'));
  });
  afterAll(() => {
    jest.restoreAllMocks();
  });
  it('renders as expected', () => {
    const { asFragment } = setup({
      securityDeposits: [...mockDeposits],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders one row for each security deposit', () => {
    const { getAllByRole } = setup({
      securityDeposits: [...mockDeposits],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });
    const rows = getAllByRole('row');
    expect(rows.length).toBe(mockDeposits.length + 1);
  });

  it('renders security deposit information as expected', () => {
    const deposit = mockDeposits[0];
    const { findFirstRow, findCell } = setup({
      securityDeposits: [deposit],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });
    const dataRow = findFirstRow() as HTMLElement;

    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 0)?.textContent).toBe(deposit.depositType.description);
    expect(findCell(dataRow, 1)?.textContent).toBe(deposit.description);
    expect(findCell(dataRow, 2)?.textContent).toBe(formatMoney(deposit.amountPaid));
    expect(findCell(dataRow, 3)?.textContent).toBe(prettyFormatDate(deposit.depositDate));
    expect(findCell(dataRow, 4)?.textContent).toBe(deposit.contactHolder?.organization?.name);
  });

  it('renders a delete icon when deposit has no returns', () => {
    const deposit = mockDeposits[0];
    deposit.depositReturns = [];
    const { findFirstRow, getAllByTitle, queryByTestId } = setup({
      securityDeposits: [deposit],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
      claims: [Claims.LEASE_DELETE, Claims.LEASE_VIEW],
    });
    const dataRow = findFirstRow() as HTMLElement;

    const tooltip = queryByTestId('tooltip-icon');
    expect(dataRow).not.toBeNull();
    expect(getAllByTitle('delete deposit')[0]).toBeVisible();
    expect(tooltip).toBeNull();
  });

  it('renders a tooltip instead of a delete icon if a deposit has a return', () => {
    const deposit = mockDeposits[0];
    deposit.depositReturns = [{} as any];
    const { findFirstRow, queryByTitle, getByTestId } = setup({
      securityDeposits: [deposit],
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
      claims: [Claims.LEASE_DELETE, Claims.LEASE_VIEW],
    });
    const dataRow = findFirstRow() as HTMLElement;

    const tooltip = getByTestId('tooltip-icon');
    expect(dataRow).not.toBeNull();
    expect(queryByTitle('delete deposit')).toBeNull();
    expect(tooltip).toBeVisible();
    expect(tooltip.id).toBe('no-delete-tooltip-1');
  });

  it('renders security deposit return holders as links', () => {
    const { getByText } = setup({
      securityDeposits: mockDeposits,
      onAdd: mockVoidCallback,
      onEdit: mockCallback,
      onDelete: mockCallback,
      onReturn: mockCallback,
    });

    expect(getByText('test organization')).toHaveAttribute('href', '/contact/O3');
    expect(getByText('test person')).toHaveAttribute('href', '/contact/P1');
  });
});
