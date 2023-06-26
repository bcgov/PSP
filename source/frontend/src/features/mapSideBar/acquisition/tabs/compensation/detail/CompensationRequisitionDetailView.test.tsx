import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { Roles } from '@/constants/index';
import {
  getMockApiCompensation,
  getMockApiCompensationPayee,
  getMockApiDefaultCompensation,
  getMockApiFinalCompensation,
} from '@/mocks/compensations.mock';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import CompensationRequisitionDetailView, {
  CompensationRequisitionDetailViewProps,
} from './CompensationRequisitionDetailView';

const setEditMode = jest.fn();

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

describe('Compensation Detail View Component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionDetailViewProps> },
  ) => {
    // render component under test
    const component = render(
      <CompensationRequisitionDetailView
        compensation={renderOptions?.props?.compensation ?? getMockApiCompensation()}
        compensationPayee={renderOptions?.props?.compensationPayee ?? getMockApiCompensationPayee()}
        loading={renderOptions.props?.loading ?? false}
        setEditMode={setEditMode}
        clientConstant={renderOptions.props?.clientConstant ?? '034'}
        onGenerate={jest.fn()}
      />,
      {
        ...renderOptions,
        history: history,
        claims: renderOptions?.claims ?? [Claims.COMPENSATION_REQUISITION_VIEW],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.restoreAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('Displays the Comensation Requisition Header Information with Draft Status', async () => {
    const mockCompensation = getMockApiDefaultCompensation();
    const { queryByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: { compensation: mockCompensation },
    });

    const editButton = queryByTestId('compensation-client');
    expect(editButton).toHaveTextContent('034');

    const compensationNumber = queryByTestId('compensation-number');
    expect(compensationNumber).toHaveTextContent('Draft');

    const headerPreTaxAmount = queryByTestId('header-pretax-amount');
    expect(headerPreTaxAmount).toHaveTextContent('$0.00');

    const headerTaxAmount = queryByTestId('header-tax-amount');
    expect(headerTaxAmount).toHaveTextContent('$0.00');

    const headerTotalAmount = queryByTestId('header-total-amount');
    expect(headerTotalAmount).toHaveTextContent('$0.00');
  });

  it('Displays the Comensation Requisition Header Information with Final Status', async () => {
    const mockCompensation = getMockApiFinalCompensation();
    const { queryByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: { compensation: mockCompensation },
    });

    const editButton = queryByTestId('compensation-client');
    expect(editButton).toHaveTextContent('034');

    const compensationNumber = queryByTestId('compensation-number');
    expect(compensationNumber).toHaveTextContent('1');

    const headerPreTaxAmount = queryByTestId('header-pretax-amount');
    expect(headerPreTaxAmount).toHaveTextContent('$30,000.00');

    const headerTaxAmount = queryByTestId('header-tax-amount');
    expect(headerTaxAmount).toHaveTextContent('$1,500.00');

    const headerTotalAmount = queryByTestId('header-total-amount');
    expect(headerTotalAmount).toHaveTextContent('$31,500.00');
  });

  it('Edit Compensation Button not displayed without claims when is in "Draft" status', async () => {
    const { queryByTitle } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).not.toBeInTheDocument();
  });

  it('Can click on the Edit Compensation Button when is in "Draft" status', async () => {
    const { getByTitle } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
    });

    const editButton = getByTitle('Edit compensation requisition');
    expect(editButton).toBeVisible();

    await act(async () => userEvent.click(editButton));

    expect(setEditMode).toHaveBeenCalled();
  });

  it('User does not have the option to Edit Compensation when is in "FINAL" status', async () => {
    const mockFinalCompensation = getMockApiFinalCompensation();
    const { queryByTitle } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: { compensation: mockFinalCompensation },
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).not.toBeInTheDocument();
  });

  it('Admin user should be able to Edit Compensation when is in "FINAL" status', async () => {
    const mockFinalCompensation = getMockApiFinalCompensation();
    const { queryByTitle } = await setup({
      roles: [Roles.SYSTEM_ADMINISTRATOR],
      props: { compensation: mockFinalCompensation },
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).toBeInTheDocument();
  });
});
