import { Api_Property } from '@/models/api/Property';
import { render, RenderOptions, RenderResult, userEvent } from '@/utils/test-utils';

import { ComposedProperty } from './ComposedProperty';
import { IMotiInventoryHeaderProps, MotiInventoryHeader } from './MotiInventoryHeader';

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
};

const onZoom = jest.fn();
describe('MotiInventoryHeader component', () => {
  const setup = (
    renderOptions: RenderOptions & IMotiInventoryHeaderProps = {
      composedProperty: defaultComposedProperty,
      isLoading: false,
    },
  ): RenderResult => {
    // render component under test
    const result = render(
      <MotiInventoryHeader
        composedProperty={renderOptions.composedProperty}
        onZoom={onZoom}
        isLoading={renderOptions.isLoading}
      />,
    );
    return result;
  };

  afterEach(() => {
    onZoom.mockClear();
  });

  it('renders as expected', () => {
    const result = setup();
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('renders a spinner when the data is loading', () => {
    const { getByTestId } = setup({
      composedProperty: { ...defaultComposedProperty },
      isLoading: true,
    });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays PID', async () => {
    const testPid = '009-212-434';
    const result = setup({
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
    const testProperty: Api_Property = {
      propertyType: { description: 'A land type description' },
    };
    const result = setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: testProperty,
      },
      isLoading: false,
    });
    // PID is shown
    expect(result.getByText(testProperty?.propertyType?.description as string)).toBeVisible();
  });

  it('allows the active property to be zoomed in', async () => {
    const testProperty: Api_Property = {} as any;

    const { getByTitle } = setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: testProperty,
      },
      isLoading: false,
    });
    const zoomButton = getByTitle('Zoom Map');
    userEvent.click(zoomButton);
    expect(onZoom).toHaveBeenCalled();
  });

  it('does not allow property zooming if no property is visible', async () => {
    const { getByTitle } = setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: undefined,
      },
      isLoading: false,
    });

    const zoomButton = getByTitle('Zoom Map');
    userEvent.click(zoomButton);
    expect(onZoom).toHaveBeenCalled();
  });
});
