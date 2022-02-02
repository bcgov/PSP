import { useKeycloak } from '@react-keycloak/web';
import { ILeaseSecurityDeposit } from 'interfaces';
import { formatMoney, prettyFormatDate } from 'utils';
import { getAllByRole as getAllByRoleBase, render, RenderOptions } from 'utils/test-utils';

import DepositsReceivedContainer, {
  IDepositsReceivedContainerProps,
} from './DepositsReceivedContainer';

const mockVoidCallback = (): void => {};
const mockCallback = (id: number): void => {};

const mockDeposits: ILeaseSecurityDeposit[] = [
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
    rowVersion: 1,
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
    rowVersion: 1,
  },
];

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
const setup = (renderOptions: RenderOptions & IDepositsReceivedContainerProps) => {
  const result = render(
    <DepositsReceivedContainer
      securityDeposits={renderOptions.securityDeposits}
      depositReturns={renderOptions.depositReturns}
      onAdd={mockVoidCallback}
      onEdit={mockCallback}
      onDelete={mockCallback}
      onReturn={mockCallback}
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
      depositReturns: [],
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
      depositReturns: [],
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
      depositReturns: [],
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
  });
});
