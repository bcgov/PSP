import userEvent from '@testing-library/user-event';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { getDefaultFormLease } from '../models';
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
      <Formik onSubmit={noop} initialValues={getDefaultFormLease()}>
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
    await act(async () => {
      fillInput(container, 'properties.0.property.pid', '1');
    });
    expect(getByLabelText('Lease Area:')).not.toBeDisabled();
  });

  it('pin is disabled when pid is valued', async () => {
    const {
      component: { getByLabelText, container },
    } = await setup({});
    await act(async () => {
      fillInput(container, 'properties.0.property.pid', '1');
    });
    expect(getByLabelText('PIN:')).toBeDisabled();
  });

  it('pid is disabled when pin is valued', async () => {
    const {
      component: { getByLabelText, container },
    } = await setup({});
    await act(async () => {
      fillInput(container, 'properties.0.property.pin', '1');
    });
    expect(getByLabelText('PID:')).toBeDisabled();
  });

  it('can add another row', async () => {
    const {
      component: { getByText, getAllByText },
    } = await setup({});

    await act(async () => {
      userEvent.click(getByText('+ Add another property'));
    });

    expect(getAllByText('PID:')).toHaveLength(2);
  });

  it('can remove a row', async () => {
    const {
      component: { getByText, queryByText },
    } = await setup({});

    await act(async () => {
      userEvent.click(getByText('Remove'));
    });

    expect(queryByText('PID:')).toBeNull();
  });
});
