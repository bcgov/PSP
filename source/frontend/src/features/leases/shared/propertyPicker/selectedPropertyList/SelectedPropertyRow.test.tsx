import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { IMapProperty } from '@/components/propertySelector/models';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { renderAsync, RenderOptions, userEvent } from '@/utils/test-utils';

import SelectedPropertyRow, { ISelectedPropertyRowProps } from './SelectedPropertyRow';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onRemove = vi.fn();

describe('SelectedPropertyRow component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<ISelectedPropertyRowProps> & { values?: { properties: IMapProperty[] } } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={renderOptions.values ?? {}}>
        {() => (
          <SelectedPropertyRow
            property={
              renderOptions.values?.properties
                ? PropertyForm.fromMapProperty(renderOptions.values?.properties[0])
                : PropertyForm.fromMapProperty({})
            }
            index={renderOptions.index ?? 0}
            onRemove={onRemove}
          />
        )}
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
    const mapProperties: IMapProperty[] = [
      { pid: '111111111', pin: '1234', planNumber: 'plan', latitude: 4, longitude: 5 },
    ];

    const {
      component: { getByText },
    } = await setup({
      values: {
        properties: mapProperties,
      },
    } as any);
    expect(getByText('PID: 111-111-111')).toBeVisible();
  });
  it('falls back to pin', async () => {
    const mapProperties: IMapProperty[] = [
      { pin: '1234', planNumber: 'plan', latitude: 4, longitude: 5 },
    ];

    const {
      component: { getByText },
    } = await setup({
      values: { properties: mapProperties.map(x => x) },
    });
    expect(getByText('PIN: 1234')).toBeVisible();
  });

  it('falls back to plan number', async () => {
    const mapProperties: IMapProperty[] = [{ planNumber: 'plan', latitude: 4, longitude: 5 }];
    const {
      component: { getByText },
    } = await setup({
      values: { properties: mapProperties },
    } as any);
    expect(getByText('Plan #: plan')).toBeVisible();
  });

  it('falls back to lat/lng', async () => {
    const mapProperties: IMapProperty[] = [{ latitude: 4, longitude: 5 }];
    const {
      component: { getByText },
    } = await setup({
      values: { properties: mapProperties },
    } as any);
    expect(getByText('5.000000, 4.000000')).toBeVisible();
  });

  it('falls back to address', async () => {
    const mapProperties: IMapProperty[] = [{ address: 'a test address' }];
    const {
      component: { getByText },
    } = await setup({
      values: { properties: mapProperties },
    } as any);
    expect(getByText('Address: a test address')).toBeVisible();
  });
});
