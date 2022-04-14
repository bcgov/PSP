import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { render, RenderOptions, RenderResult } from 'utils/test-utils';

import { MotiInventoryHeader } from './MotiInventoryHeader';
import { mockLtsaResponse } from './tabs/ltsa/LtsaTabView.test';

describe('MotiInventoryHeader component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      ltsaData?: LtsaOrders;
      property?: IPropertyApiModel;
    } = {},
  ): RenderResult => {
    // render component under test
    const result = render(
      <MotiInventoryHeader ltsaData={renderOptions.ltsaData} property={renderOptions.property} />,
    );
    return result;
  };

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
});
