import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { render, RenderOptions } from 'utils/test-utils';

import { IMapProperty } from './models';
import PropertySelectorSubForm, { IPropertySelectorSubFormProps } from './PropertySelectorSubForm';

const onClickAway = jest.fn();
const onClickDraftMarker = jest.fn();

const testProperty: IMapProperty = {
  pid: '123-456-789',
  planNumber: '123546',
  address: 'Test address 123',
  legalDescription: 'Test Legal Description',
};

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

describe('PropertySelectorSubForm component', () => {
  const setup = (renderOptions: RenderOptions & IPropertySelectorSubFormProps) => {
    // render component under test
    const component = render(
      <PropertySelectorSubForm
        onClickDraftMarker={renderOptions.onClickDraftMarker}
        onClickAway={renderOptions.onClickAway}
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
    jest.resetAllMocks();
  });

  it('renders as expected when provided no properties', () => {
    const { component } = setup({
      selectedProperty: testProperty,
      onClickAway,
      onClickDraftMarker,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', async () => {
    const {
      component: { getByText },
    } = await setup({
      selectedProperty: testProperty,
      onClickAway,
      onClickDraftMarker,
    });
    expect(getByText(`${testProperty.pid}`)).toBeVisible();
    expect(getByText(`${testProperty.planNumber}`)).toBeVisible();
    expect(getByText(`${testProperty.address}`)).toBeVisible();
    expect(getByText(`${testProperty.legalDescription}`)).toBeVisible();
  });
});
