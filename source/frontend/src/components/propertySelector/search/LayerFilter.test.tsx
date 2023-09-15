import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mockGeocoderOptions } from '@/mocks/index.mock';
import { act, fillInput, render, RenderOptions } from '@/utils/test-utils';

import LayerFilter, { defaultLayerFilter } from './LayerFilter';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

const setFilter = jest.fn();
const onAddressChange = jest.fn();
const onAddressSelect = jest.fn();

describe('LayerFilter component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { searchBy?: string } = {}) => {
    const searchBy = renderOptions.searchBy || 'address';
    const utils = render(
      <LayerFilter
        setFilter={setFilter}
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
    jest.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected - search by legal description', () => {
    const { asFragment } = setup({ searchBy: 'legalDescription' });
    expect(asFragment()).toMatchSnapshot();
  });

  it('shall have an searchBy field', async () => {
    const { container } = setup({});

    await act(async () => await fillInput(container, 'searchBy', 'address', 'select'));

    const searchByInput = container.querySelector('select[name="searchBy"]');
    expect(searchByInput).toBeInTheDocument();
  });

  it('shall have an address field', async () => {
    const { container } = setup({});

    await act(async () => await fillInput(container, 'searchBy', 'address', 'select'));

    const addressInput = container.querySelector('input[name="address"]');
    expect(addressInput).toBeInTheDocument();
  });

  it('shall have an address suggestions', async () => {
    const { container } = setup({});

    await act(async () => await fillInput(container, 'searchBy', 'address', 'select'));
    await act(async () => await fillInput(container, 'address', '1234 Fake'));

    const addressInput = container.querySelector('.suggestionList');
    expect(addressInput).toBeInTheDocument();
  });

  it('shall have an address suggestion option', async () => {
    const { getByText, container } = setup({});

    await act(async () => await fillInput(container, 'searchBy', 'address', 'select'));
    await act(async () => await fillInput(container, 'address', '1234 Fake'));

    const option = getByText('1234 Fake St');
    expect(option).toBeInTheDocument();
  });

  it('shall have a legal description TEXTAREA field', async () => {
    const { container, findByText } = setup({ searchBy: 'legalDescription' });

    await act(async () => await fillInput(container, 'searchBy', 'legalDescription', 'select'));

    expect(
      await findByText(/Searching by Legal Description may result in a slower search/i),
    ).toBeInTheDocument();

    const textarea = container.querySelector('textarea[name="legalDescription"]');
    expect(textarea).toBeInTheDocument();
  });
});
