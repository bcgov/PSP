import { mockGeocoderOptions } from 'hooks/pims-api/useApiGeocoder';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fillInput, render, RenderOptions } from 'utils/test-utils';

import LayerFilter, { defaultLayerFilter, ILayerFilterProps } from './LayerFilter';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

const setFilter = jest.fn();
const onAddressChange = jest.fn();
const onAddressSelect = jest.fn();

describe('LayerFilter component', () => {
  const setup = (renderOptions: RenderOptions & Partial<ILayerFilterProps>) => {
    // render component under test
    const component = render(
      <LayerFilter
        setFilter={setFilter}
        addressResults={mockGeocoderOptions}
        filter={{ ...defaultLayerFilter, searchBy: 'address' }}
        onAddressChange={onAddressChange}
        onAddressSelect={onAddressSelect}
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

  it('renders as expected', () => {
    const { component } = setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('shall have an searchBy field', async () => {
    const {
      component: { container },
    } = setup({
      addressResults: mockGeocoderOptions,
    });

    await fillInput(container, 'searchBy', 'address', 'select');

    const searchByInput = container.querySelector('select[name="searchBy"]');
    expect(searchByInput).toBeInTheDocument();
  });
  it('shall have an address field', async () => {
    const {
      component: { container },
    } = setup({
      addressResults: mockGeocoderOptions,
    });

    await fillInput(container, 'searchBy', 'address', 'select');

    const addressInput = container.querySelector('input[name="address"]');
    expect(addressInput).toBeInTheDocument();
  });
  it('shall have an address suggestions', async () => {
    const {
      component: { container },
    } = setup({
      addressResults: mockGeocoderOptions,
    });

    await fillInput(container, 'searchBy', 'address', 'select');
    await fillInput(container, 'address', '1234 Fake');

    const addressInput = container.querySelector('.suggestionList');
    expect(addressInput).toBeInTheDocument();
  });
  it('shall have an address suggestion option', async () => {
    const {
      component: { getByText, container },
    } = setup({
      addressResults: mockGeocoderOptions,
    });

    await fillInput(container, 'searchBy', 'address', 'select');
    await fillInput(container, 'address', '1234 Fake');

    const option = getByText('1234 Fake St');
    expect(option).toBeInTheDocument();
  });
});
