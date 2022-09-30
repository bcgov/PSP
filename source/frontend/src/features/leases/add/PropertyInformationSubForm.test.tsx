import userEvent from '@testing-library/user-event';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultAddFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions, waitFor } from 'utils/test-utils';

import PropertyInformationSubForm, {
  IPropertyInformationSubFormProps,
} from './PropertyInformationSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('PropertyInformationSubForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & Partial<IPropertyInformationSubFormProps> = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={defaultAddFormLease}>
        {formikProps => <PropertyInformationSubForm />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('unit type and area are disabled by default', async () => {
    const {
      component: { getByLabelText },
    } = await setup({});
    expect(getByLabelText('Lease Area:')).toBeDisabled();
  });

  it('unit type and area are enabled when pid entered', async () => {
    const {
      component: { getByLabelText, container },
    } = await setup({});
    await fillInput(container, 'properties.0.pid', '1');
    expect(getByLabelText('Lease Area:')).not.toBeDisabled();
  });

  it('pin is disabled when pid is valued', async () => {
    const {
      component: { getByLabelText, container },
    } = await setup({});
    await fillInput(container, 'properties.0.pid', '1');
    expect(getByLabelText('PIN:')).toBeDisabled();
  });

  it('pid is disabled when pin is valued', async () => {
    const {
      component: { getByLabelText, container },
    } = await setup({});
    await fillInput(container, 'properties.0.pin', '1');
    expect(getByLabelText('PID:')).toBeDisabled();
  });

  it('can add another row', async () => {
    const {
      component: { getByText, findAllByText },
    } = await setup({});
    userEvent.click(getByText('+ Add another property'));
    expect(await (await findAllByText('PID:')).length).toBe(2);
  });

  it('can remove a row', async () => {
    const {
      component: { getByText, queryByText },
    } = await setup({});
    userEvent.click(getByText('Remove'));
    await waitFor(() => {
      expect(queryByText('PID:')).toBeNull();
    });
  });
});
