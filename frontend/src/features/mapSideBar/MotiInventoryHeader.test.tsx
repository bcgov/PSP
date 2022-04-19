import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { render, RenderOptions, RenderResult, userEvent } from 'utils/test-utils';

import { MotiInventoryHeader } from './MotiInventoryHeader';
import { mockLtsaResponse } from './tabs/ltsa/LtsaTabView.test';

const onZoom = jest.fn();
describe('MotiInventoryHeader component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      ltsaData?: LtsaOrders;
      property?: IPropertyApiModel;
    } = {},
  ): RenderResult => {
    // render component under test
    const result = render(
      <MotiInventoryHeader
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
    const { getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays PID', async () => {
    const result = setup({
      ltsaData: mockLtsaResponse,
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
      property: testProperty,
    });
    // PID is shown
    expect(result.getByText(testProperty?.propertyType?.description as string)).toBeVisible();
  });

  it('allows the active property to be zoomed in', async () => {
    const testProperty: IPropertyApiModel = {} as any;
    const { getByTitle } = setup({
      ltsaData: undefined,
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
    });

    const zoomButton = getByTitle('Zoom Map');
    userEvent.click(zoomButton);
    expect(onZoom).toHaveBeenCalled();
  });
});
