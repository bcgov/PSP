import { Formik } from 'formik';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockPropertyManagementActivity } from '@/mocks/PropertyManagementActivity.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { getMockManagementActivityInvoice } from '@/mocks/managementActivityInvoice.mock';
import { InvoiceTotalsForm } from './InvoiceTotalsForm';
import { PropertyActivityFormModel } from './models';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onAdd = vi.fn();

describe('EditPropertyActivity - InvoiceTotalsForm', () => {
  const setup = (renderOptions?: RenderOptions & { initialValues: PropertyActivityFormModel }) => {
    const utils = render(
      <Formik
        onSubmit={vi.fn()}
        initialValues={renderOptions?.initialValues ?? new PropertyActivityFormModel()}
      >
        {formikProps => <InvoiceTotalsForm formikProps={formikProps} onAdd={onAdd} />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return { ...utils };
  };

  let initialValues: PropertyActivityFormModel;

  beforeEach(() => {
    initialValues = PropertyActivityFormModel.fromApi({
      ...getMockPropertyManagementActivity(),
      invoices: [
        {
          ...getMockManagementActivityInvoice(),
          pretaxAmount: 1000,
          gstAmount: 50,
          pstAmount: 0,
          totalAmount: 1050,
        },
        {
          ...getMockManagementActivityInvoice(),
          pretaxAmount: 2000,
          gstAmount: 100,
          pstAmount: 0,
          totalAmount: 2100,
        },
      ],
    });
  });

  afterEach(() => vi.clearAllMocks());

  it('renders as expected', async () => {
    const { asFragment } = setup({ initialValues });
    await act(() => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('calculates total amounts as expected', async () => {
    setup({ initialValues });
    await act(() => {});
    expect(await screen.findByDisplayValue('$3,150.00')).toBeVisible();
  });

  it(`calls "onAdd" when Add Invoice button is clicked`, async () => {
    setup({ initialValues });
    await act(() => {});

    const addButton = await screen.findByText(/Add an Invoice/i);
    await act(() => userEvent.click(addButton));

    expect(onAdd).toHaveBeenCalled();
  });
});
