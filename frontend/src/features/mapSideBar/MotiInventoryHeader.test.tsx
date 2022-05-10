import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { mockLtsaResponse } from 'mocks/filterDataMock';
import { render, RenderOptions, RenderResult, userEvent } from 'utils/test-utils';

import { MotiInventoryHeader } from './MotiInventoryHeader';

const onZoom = jest.fn();
describe('MotiInventoryHeader component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      ltsaData?: LtsaOrders;
      property?: IPropertyApiModel;
      ltsaLoading?: boolean;
      propertyLoading?: boolean;
    } = {},
  ): RenderResult => {
    // render component under test
    const result = render(
      <MotiInventoryHeader
        ltsaLoading={!!renderOptions.ltsaLoading}
        propertyLoading={!!renderOptions.propertyLoading}
        ltsaData={renderOptions.ltsaData}
        property={renderOptions.property}
        onZoom={onZoom}
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
    const { getByTestId } = setup({ ltsaLoading: true });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays PID', async () => {
    const result = setup({
      ltsaData: mockLtsaResponse,
      ltsaLoading: false,
      propertyLoading: false,
      property: undefined,
    });
    // PID is shown
    expect(
      result.getByText(mockLtsaResponse.parcelInfo.orderedProduct.fieldedData.parcelIdentifier),
    ).toBeVisible();
  });

  it('displays land parcel type', async () => {
    const testProperty: IPropertyApiModel = {
      propertyType: { description: 'A land type description' },
    };
    const result = setup({
      ltsaData: undefined,
      ltsaLoading: false,
      propertyLoading: false,
      property: testProperty,
    });
    // PID is shown
    expect(result.getByText(testProperty?.propertyType?.description as string)).toBeVisible();
  });

  it('allows the active property to be zoomed in', async () => {
    const testProperty: IPropertyApiModel = {} as any;
    const { getByTitle } = setup({
      ltsaData: undefined,
      ltsaLoading: false,
      propertyLoading: false,
      property: testProperty,
    });
    const zoomButton = getByTitle('Zoom Map');
    userEvent.click(zoomButton);
    expect(onZoom).toHaveBeenCalled();
  });

  it('does not allow property zooming if no property is visible', async () => {
    const { getByTitle } = setup({
      ltsaData: undefined,
      property: undefined,
      ltsaLoading: false,
      propertyLoading: false,
    });

    const zoomButton = getByTitle('Zoom Map');
    userEvent.click(zoomButton);
    expect(onZoom).toHaveBeenCalled();
  });
});
