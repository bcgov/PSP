import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { render, RenderOptions } from '@/utils/test-utils';

import { IMapProperty } from '../models';
import PropertyMapSelectorSubForm, {
  IPropertyMapSelectorSubFormProps,
} from './PropertyMapSelectorSubForm';

const onClickDraftMarker = vi.fn();

const testProperty: IMapProperty = {
  pid: '123-456-789',
  planNumber: '123546',
  address: 'Test address 123',
  region: 2,
  regionName: 'South Coast',
  district: 2,
  districtName: 'Vancouver Island',
};

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

describe('PropertySelectorSubForm component', () => {
  const setup = (renderOptions: RenderOptions & IPropertyMapSelectorSubFormProps) => {
    // render component under test
    const component = render(
      <PropertyMapSelectorSubForm
        onClickDraftMarker={renderOptions.onClickDraftMarker}
        selectedProperty={renderOptions.selectedProperty}
      />,
      {
        ...renderOptions,
        store: store,
      },
    );

    return {
      store,
      component,
    };
  };

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected when provided no properties', () => {
    const { component } = setup({
      selectedProperty: testProperty,
      onClickDraftMarker,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', async () => {
    const {
      component: { getByText },
    } = await setup({
      selectedProperty: testProperty,
      onClickDraftMarker,
    });
    expect(getByText(`${testProperty.pid}`)).toBeVisible();
    expect(getByText(`${testProperty.planNumber}`)).toBeVisible();
    expect(getByText(`${testProperty.address}`)).toBeVisible();
    expect(getByText(`${testProperty.region} - ${testProperty.regionName}`)).toBeVisible();
    expect(getByText(`${testProperty.district} - ${testProperty.districtName}`)).toBeVisible();
  });
});
