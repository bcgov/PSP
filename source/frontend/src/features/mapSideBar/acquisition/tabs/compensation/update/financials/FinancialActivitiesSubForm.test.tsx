import { Formik } from 'formik';
import { noop } from 'lodash';

import { SelectOption } from '@/components/common/form';
import { getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import {
  act,
  fillInput,
  fireEvent,
  renderAsync,
  RenderOptions,
  userEvent,
} from '@/utils/test-utils';

import { CompensationRequisitionYupSchema } from '../CompensationRequisitionYupSchema';
import { CompensationRequisitionFormModel } from '../models';
import FinancialActivitiesSubForm, {
  IFinancialActivitiesSubFormProps,
} from './FinancialActivitiesSubForm';

const activitiesUpdated = jest.fn();
const defaultApiCompensation = CompensationRequisitionFormModel.fromApi(
  getMockApiDefaultCompensation(),
);
const mockFinancialActivitiesOptions: SelectOption[] = [
  { label: 'Land', value: '1', code: '4000' },
];

describe('FinancialActivitiesSubForm  component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IFinancialActivitiesSubFormProps> },
  ) => {
    // render component under test
    const utils = await renderAsync(
      <Formik
        onSubmit={noop}
        initialValues={defaultApiCompensation}
        validationSchema={CompensationRequisitionYupSchema}
      >
        {formikProps => (
          <FinancialActivitiesSubForm
            formikProps={formikProps}
            compensationRequisitionId={defaultApiCompensation.id!}
            financialActivityOptions={mockFinancialActivitiesOptions}
            gstConstantPercentage={0.05}
            activitiesUpdated={activitiesUpdated}
          />
        )}
      </Formik>,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
      // Finding elements
      getPreTaxAmountTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="financials[${index}].pretaxAmount"]`,
        ) as HTMLInputElement,
      getTaxAmountTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="financials[${index}].taxAmount"]`,
        ) as HTMLInputElement,
      getTotalAmountTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="financials[${index}].totalAmount"]`,
        ) as HTMLInputElement,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it(`renders 'Add Activity' link`, async () => {
    const { getByTestId } = await setup({});
    expect(getByTestId('add-financial-activity')).toBeVisible();
  });

  it(`renders 'Remove Activity' link`, async () => {
    const { getByTestId } = await setup({});
    const addRow = getByTestId('add-financial-activity');
    await act(async () => {
      userEvent.click(addRow);
    });
    expect(getByTestId('activity[0].delete-button')).toBeVisible();
  });

  it(`renders 'Financial Activity Default Values`, async () => {
    const { getByTestId, getPreTaxAmountTextbox, getTaxAmountTextbox, getTotalAmountTextbox } =
      await setup({});
    const addRow = getByTestId('add-financial-activity');
    await act(async () => {
      userEvent.click(addRow);
    });

    expect(getByTestId('activity[0].delete-button')).toBeVisible();
    expect(getPreTaxAmountTextbox(0)).toBeVisible();
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$0.00');

    expect(getTaxAmountTextbox(0)).toBeNull();

    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');
  });

  it(`removes the financial activity upon user confirmation`, async () => {
    const { getByTestId, getByText, getByTitle, queryByTestId } = await setup({});

    await act(async () => userEvent.click(getByTestId('add-financial-activity')));
    await act(async () => userEvent.click(getByTestId('activity[0].delete-button')));

    expect(getByText(/Are you sure you want to remove this financial activity/i)).toBeVisible();
    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(activitiesUpdated).toHaveBeenCalled();
    expect(queryByTestId(`finacialActivity[0]`)).not.toBeInTheDocument();
  });

  it(`keeps the financial activity upon user cancel`, async () => {
    const { getByTestId, getByText, getByTitle, queryByTestId } = await setup({});

    await act(async () => userEvent.click(getByTestId('add-financial-activity')));
    await act(async () => userEvent.click(getByTestId('activity[0].delete-button')));

    expect(getByText(/Are you sure you want to remove this financial activity/i)).toBeVisible();
    await act(async () => userEvent.click(getByTitle('cancel-modal')));

    expect(activitiesUpdated).toHaveBeenCalledTimes(0);
    expect(queryByTestId(`finacialActivity[0]`)).toBeInTheDocument();
  });

  it(`calculates the total amount when adding pretax amount`, async () => {
    const { getByTestId, getPreTaxAmountTextbox, getTaxAmountTextbox, getTotalAmountTextbox } =
      await setup({});
    const addRow = getByTestId('add-financial-activity');
    await act(async () => {
      userEvent.click(addRow);
    });

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).toBeNull();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    expect(activitiesUpdated).toHaveBeenCalled();

    // Total
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$100.00');
  });

  it(`calculates the GST amount when adding pretax and tax amount`, async () => {
    const {
      container,
      getByTestId,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});
    const addRow = getByTestId('add-financial-activity');
    await act(async () => {
      userEvent.click(addRow);
    });

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).toBeNull();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    await act(() => fillInput(container, 'financials[0].isGstRequired', 'true', 'select'));
    expect(getTaxAmountTextbox(0)).toBeVisible();

    expect(activitiesUpdated).toHaveBeenCalled();

    // Total
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTaxAmountTextbox(0)).toHaveValue('$5.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$105.00');
  });

  it(`re-calculates the GST amount when removing tax amount`, async () => {
    const {
      container,
      getByTestId,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});
    const addRow = getByTestId('add-financial-activity');
    await act(async () => {
      userEvent.click(addRow);
    });

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).toBeNull();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    await act(() => fillInput(container, 'financials[0].isGstRequired', 'true', 'select'));
    expect(getTaxAmountTextbox(0)).toBeVisible();

    expect(activitiesUpdated).toHaveBeenCalled();

    // Total
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTaxAmountTextbox(0)).toHaveValue('$5.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$105.00');

    await act(() => fillInput(container, 'financials[0].isGstRequired', 'false', 'select'));
    expect(getTaxAmountTextbox(0)).toBeNull();

    // Total
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$100.00');
    expect(activitiesUpdated).toHaveBeenCalled();
  });

  it(`overrides the GST amount`, async () => {
    const {
      container,
      getByTestId,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});
    const addRow = getByTestId('add-financial-activity');
    await act(async () => {
      userEvent.click(addRow);
    });

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).toBeNull();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    await act(() => fillInput(container, 'financials[0].isGstRequired', 'true', 'select'));
    expect(getTaxAmountTextbox(0)).toBeVisible();

    expect(activitiesUpdated).toHaveBeenCalled();

    // Total
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTaxAmountTextbox(0)).toHaveValue('$5.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$105.00');

    await act(async () => {
      fireEvent.change(getTaxAmountTextbox(0), { target: { value: '$10.00' } });
    });

    expect(getTaxAmountTextbox(0)).toHaveValue('$10.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$110.00');

    expect(activitiesUpdated).toHaveBeenCalled();
  });
});
