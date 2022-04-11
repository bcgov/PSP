import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { IProperty } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { renderAsync, RenderOptions, userEvent } from 'utils/test-utils';

import SelectedPropertyRow, { ISelectedPropertyRowProps } from './SelectedPropertyRow';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onRemove = jest.fn();

describe('SelectedPropertyRow component', () => {
  const setup = async (
    renderOptions: RenderOptions & Partial<ISelectedPropertyRowProps> & { values?: IProperty } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={renderOptions.values ?? {}}>
        {() => <SelectedPropertyRow index={renderOptions.index ?? 0} onRemove={onRemove} />}
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

  it('fires onRemove when remove button clicked', async () => {
    const {
      component: { getByTitle },
    } = await setup({});
    const removeButton = getByTitle('remove');
    userEvent.click(removeButton);
    expect(onRemove).toHaveBeenCalled();
  });

  it('displays pid', async () => {
    const {
      component: { getByText },
    } = await setup({
      values: { pid: '111111111', pin: 1234, planNumber: 'plan', latitude: 4, longitude: 5 },
    } as any);
    expect(getByText('PID: 111-111-111')).toBeVisible();
  });
  it('falls back to pin', async () => {
    const {
      component: { getByText },
    } = await setup({
      values: { pin: 1234, planNumber: 'plan', latitude: 4, longitude: 5 },
    } as any);
    expect(getByText('PIN: 1234')).toBeVisible();
  });

  it('falls back to plan number', async () => {
    const {
      component: { getByText },
    } = await setup({
      values: { planNumber: 'plan', latitude: 4, longitude: 5 },
    } as any);
    expect(getByText('Plan #: plan')).toBeVisible();
  });

  it('falls back to lat/lng', async () => {
    const {
      component: { getByText },
    } = await setup({
      values: { latitude: 4, longitude: 5 },
    } as any);
    expect(getByText('4.00000, 5.00000')).toBeVisible();
  });
});
