import { useKeycloak } from '@react-keycloak/web';

import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getMockDepositReturns, getMockDeposits } from '@/mocks/deposits.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { formatMoney, prettyFormatDate } from '@/utils';
import { getAllByRole as getAllByRoleBase, render, RenderOptions } from '@/utils/test-utils';

import DepositsReturnedContainer, {
  IDepositsReturnedContainerProps,
} from './DepositsReturnedContainer';
import { LeaseStatusUpdateSolver } from '@/features/leases/models/LeaseStatusUpdateSolver';
import { Claims } from '@/constants';
import { toTypeCode } from '@/utils/formUtils';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';

const mockCallback = (id: number): void => {};

// render component under test
const setup = (renderOptions: RenderOptions & IDepositsReturnedContainerProps) => {
  const result = render(
    <DepositsReturnedContainer
      securityDeposits={renderOptions.securityDeposits}
      depositReturns={renderOptions.depositReturns}
      onEdit={mockCallback}
      onDelete={mockCallback}
      statusSolver={new LeaseStatusUpdateSolver()}
    />,
    {
      ...renderOptions,
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
      getMockDeposits()[0].depositType?.description ?? '',
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
          contactHolder: {
            id: 'O1',
            organization: { ...getEmptyOrganization(), name: 'test organization' },
            person: null,
          },
        },
        {
          ...depositReturn,
          contactHolder: {
            id: 'P1',
            person: { ...getEmptyPerson(), firstName: 'test', surname: 'person' },
            organization: null,
          },
        },
      ],
      onEdit: mockCallback,
      onDelete: mockCallback,
    });

    expect(getByText('test organization')).toHaveAttribute('href', '/contact/O1');
    expect(getByText('test person')).toHaveAttribute('href', '/contact/P1');
  });

  it('renders a tooltip instead of a delete icon if file in final status', () => {
    const deposit = getMockDeposits()[0];
    const depositReturn = getMockDepositReturns()[0];
    deposit.id = 1;
    depositReturn.parentDepositId = 1;
    deposit.depositReturns = [
      {
        ...depositReturn,
        contactHolder: {
          id: 'O1',
          organization: { ...getEmptyOrganization(), name: 'test organization' },
          person: null,
        },
      },
    ];
    const { findFirstRow, queryByTitle, getByTestId } = setup({
      securityDeposits: [deposit],
      onEdit: mockCallback,
      onDelete: mockCallback,
      depositReturns: deposit.depositReturns,
      statusSolver: new LeaseStatusUpdateSolver(
        toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED),
      ),
      claims: [Claims.LEASE_EDIT, Claims.LEASE_VIEW],
    });
    const dataRow = findFirstRow() as HTMLElement;

    const tooltip = getByTestId(`tooltip-icon-deposit-returned-actions-cannot-edit-tooltip`);
    expect(dataRow).not.toBeNull();
    expect(queryByTitle('delete deposit return')).toBeNull();
    expect(tooltip).toBeVisible();
  });
});
