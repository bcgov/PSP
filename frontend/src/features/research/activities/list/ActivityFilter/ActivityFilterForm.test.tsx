import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultActivityFilter } from 'interfaces/IActivityResults';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { ActivityFilterForm, IActivityFilterFormProps } from './ActivityFilterForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSetFilter = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ActivityFilterForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IActivityFilterFormProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <ActivityFilterForm onSetFilter={onSetFilter} activityFilter={defaultActivityFilter} />
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

  beforeEach(() => {
    mockAxios.reset();
  });

  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: { ...defaultActivityFilter },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders activityType with data as expected', async () => {
    const {
      component: { getByTestId, container },
    } = await setup({
      initialValues: { ...defaultActivityFilter },
    });

    await fillInput(container, 'activityType', 'Survey', 'select');

    expect(getByTestId('activity-type')).not.toBeNull();
  });

  it('renders activityStatus with data as expected', async () => {
    const {
      component: { getByTestId, container },
    } = await setup({
      initialValues: { ...defaultActivityFilter },
    });

    await fillInput(container, 'activityStatus', 'Draft', 'select');

    expect(getByTestId('activity-status')).not.toBeNull();
  });
});
