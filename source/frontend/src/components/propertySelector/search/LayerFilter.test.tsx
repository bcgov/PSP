import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mockGeocoderOptions } from '@/mocks/index.mock';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import LayerFilter, { defaultLayerFilter } from './LayerFilter';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

const setFilter = vi.fn();
const onAddressChange = vi.fn();
const onAddressSelect = vi.fn();

describe('LayerFilter component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { searchBy?: string } = {}) => {
    const searchBy = renderOptions.searchBy || 'address';
    const utils = render(
      <LayerFilter
        onSearch={setFilter}
        addressResults={mockGeocoderOptions}
        filter={{ ...defaultLayerFilter, searchBy }}
        onAddressChange={onAddressChange}
        onAddressSelect={onAddressSelect}
        loading={false}
      />,
      {
        ...renderOptions,
        store: store,
      },
    );

    return {
      store,
      ...utils,
    };
  };

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('shall have an searchBy field', async () => {
    const { container } = setup({});

    await act(async () => {
      await fillInput(container, 'searchBy', 'address', 'select');
    });

    const searchByInput = container.querySelector('select[name="searchBy"]');
    expect(searchByInput).toBeInTheDocument();
  });

  it('shall have an address field', async () => {
    const { container } = setup({});

    await act(async () => {
      await fillInput(container, 'searchBy', 'address', 'select');
    });

    const addressInput = container.querySelector('input[name="address"]');
    expect(addressInput).toBeInTheDocument();
  });

  it('shall have an address suggestions', async () => {
    const { container } = setup({});

    await act(async () => {
      await fillInput(container, 'searchBy', 'address', 'select');
    });
    await act(async () => {
      await fillInput(container, 'address', '1234 Fake');
    });

    const addressInput = container.querySelector('.suggestionList');
    expect(addressInput).toBeInTheDocument();
  });

  it('shall have an address suggestion option', async () => {
    const { getByText, container } = setup({});

    await act(async () => {
      await fillInput(container, 'searchBy', 'address', 'select');
    });
    await act(async () => {
      await fillInput(container, 'address', '1234 Fake');
    });

    const option = getByText('1234 Fake St');
    expect(option).toBeInTheDocument();
  });

  it('shall have a coordinate searchBy field', async () => {
    const { container, getByText } = setup({});

    await act(async () => {
      await fillInput(container, 'searchBy', 'coordinates', 'select');
    });

    expect(getByText('Lat:')).toBeVisible();
  });

  it('searches by coordinates', async () => {
    const { container, getByTestId } = setup({});

    await act(async () => {
      await fillInput(container, 'searchBy', 'coordinates', 'select');
    });

    const searchButton = getByTestId('search');

    await act(async () => {
      userEvent.click(searchButton);
    });

    expect(setFilter).toHaveBeenCalledWith({
           "address": "",
           "coordinates": {
             "latitude": {
               "degrees": 0,
               "direction": "1",
               "minutes": 0,
               "seconds": 0,
             },
             "longitude": {
               "degrees": 0,
               "direction": "-1",
               "minutes": 0,
               "seconds": 0,
             },
           },
           "legalDescription": "",
           "pid": "",
           "pin": "",
           "planNumber": "",
           "searchBy": "coordinates",
         });
  });
});
