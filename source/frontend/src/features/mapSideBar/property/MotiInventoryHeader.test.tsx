import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { act, render, RenderOptions, RenderResult, userEvent } from '@/utils/test-utils';

import { ComposedProperty } from './ComposedProperty';
import { IMotiInventoryHeaderProps, MotiInventoryHeader } from './MotiInventoryHeader';
import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';

const defaultComposedProperty: ComposedProperty = {
  pid: undefined,
  pin: undefined,
  id: undefined,
  ltsaOrders: undefined,
  pimsProperty: undefined,
  propertyAssociations: undefined,
  parcelMapFeatureCollection: undefined,
  geoserverFeatureCollection: undefined,
  bcAssessmentSummary: undefined,
  crownTenureFeature: undefined,
};

vi.mock('@/hooks/repositories/useHistoricalNumberRepository');
vi.mocked(useHistoricalNumberRepository).mockReturnValue({
  getPropertyHistoricalNumbers: {
    error: null,
    response: [],
    execute: vi.fn().mockResolvedValue([]),
    loading: false,
    status: 200,
  },
  updatePropertyHistoricalNumbers: {
    error: null,
    response: [],
    execute: vi.fn().mockResolvedValue([]),
    loading: false,
    status: 200,
  },
});

const onZoom = vi.fn();
describe('MotiInventoryHeader component', () => {
  const setup = async (
    renderOptions: RenderOptions & IMotiInventoryHeaderProps = {
      composedProperty: defaultComposedProperty,
      isLoading: false,
    },
  ): Promise<RenderResult> => {
    // render component under test
    const result = render(
      <MotiInventoryHeader
        composedProperty={renderOptions.composedProperty}
        onZoom={onZoom}
        isLoading={renderOptions.isLoading}
      />,
    );
    await act(async () => {});
    return result;
  };

  afterEach(() => {
    onZoom.mockClear();
  });

  it('renders as expected', async () => {
    const result = await setup();
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('renders a spinner when the data is loading', async () => {
    const { getByTestId } = await setup({
      composedProperty: { ...defaultComposedProperty },
      isLoading: true,
    });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays PID', async () => {
    const testPid = '009-212-434';
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pid: testPid,
      },
      isLoading: false,
    });
    // PID is shown
    expect(result.getByText(testPid)).toBeVisible();
  });

  it('displays land parcel type', async () => {
    const testProperty: ApiGen_Concepts_Property = {
      ...getEmptyProperty(),
      propertyType: {
        description: 'A land type description',
        displayOrder: null,
        isDisabled: false,
        id: null,
      },
    };
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: testProperty,
      },
      isLoading: false,
    });
    // land parcel type is shown
    expect(result.getByText(testProperty?.propertyType?.description as string)).toBeVisible();
  });

  it(`shows "retired" indicator for retired properties`, async () => {
    const testProperty: ApiGen_Concepts_Property = {
      ...getEmptyProperty(),
      isRetired: true,
    };
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: testProperty,
      },
      isLoading: false,
    });
    // "retired" indicator is shown
    expect(result.getByText(/retired/i)).toBeVisible();
  });

  it('allows the active property to be zoomed in', async () => {
    const testProperty: ApiGen_Concepts_Property = { latitude: 1, longitude: 1 } as any;

    const { getByTitle } = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: testProperty,
      },
      isLoading: false,
    });
    const zoomButton = getByTitle('Zoom Map');
    await act(async () => userEvent.click(zoomButton));
    expect(onZoom).toHaveBeenCalled();
  });

  it('does not allow property zooming if no property is visible', async () => {
    const { queryByText } = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: undefined,
      },
      isLoading: false,
    });

    expect(queryByText('Zoom Map')).not.toBeInTheDocument();
    expect(onZoom).not.toHaveBeenCalled();
  });

  it('displays PIN', async () => {
    const testPin = '9212434';
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pin: testPin,
      },
      isLoading: false,
    });
    // PID is shown
    expect(result.getByText(testPin)).toBeVisible();
  });
});
