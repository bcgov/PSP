import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mockPropertyLayerSearchResponse } from '@/mocks/filterData.mock';
import { mapFeatureToProperty } from '@/utils/mapPropertyUtils';
import { act, fillInput, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { IMapProperty } from '../models';
import { defaultLayerFilter } from './LayerFilter';
import {
  IPropertySearchSelectorFormViewProps,
  PropertySearchSelectorFormView,
} from './PropertySearchSelectorFormView';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

const onSelectedProperties = vi.fn();
const onSearch = vi.fn();
const onAddressChange = vi.fn();
const onAddressSelect = vi.fn();

describe('PropertySearchSelectorFormView component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IPropertySearchSelectorFormViewProps>) => {
    // render component under test
    const component = render(
      <PropertySearchSelectorFormView
        onSearch={onSearch}
        search={renderOptions.search}
        onSelectedProperties={onSelectedProperties}
        searchResults={renderOptions.searchResults ?? []}
        loading={renderOptions.loading ?? false}
        selectedProperties={renderOptions.selectedProperties ?? []}
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
      ...component,
    };
  };

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected when provided no properties', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  describe('search functionality', () => {
    it('displays the loading spinner when making a request', async () => {
      const { getByTitle } = setup({
        loading: true,
      });
      expect(getByTitle('table-loading')).toBeVisible();
    });
    it('can search for a pid', async () => {
      const { getByTitle, getByText, container } = setup({});
      expect(getByText('No results found for your search criteria.')).toBeInTheDocument();
      await act(async () => {
        await fillInput(container, 'searchBy', 'pid', 'select');
        await fillInput(container, 'pid', '123-456-789');
      });
      const searchButton = getByTitle('search');

      await act(async () => userEvent.click(searchButton));
      await waitFor(() => {
        expect(onSearch).toHaveBeenCalledWith({ ...defaultLayerFilter, pid: '123-456-789' });
      });
    });

    it('can search for a pin', async () => {
      const { getByTitle, getByText, container } = setup({});
      expect(getByText('No results found for your search criteria.')).toBeInTheDocument();
      await act(async () => {
        await fillInput(container, 'searchBy', 'pin', 'select');
        await fillInput(container, 'pin', '54321');
      });
      const searchButton = getByTitle('search');

      await act(async () => userEvent.click(searchButton));
      await waitFor(() => {
        expect(onSearch).toHaveBeenCalledWith({
          ...defaultLayerFilter,
          searchBy: 'pin',
          pin: '54321',
        });
      });
    });

    it('can search for a plan number', async () => {
      const { getByTitle, getByText, container } = setup({});
      expect(getByText('No results found for your search criteria.')).toBeInTheDocument();
      await act(async () => {
        await fillInput(container, 'searchBy', 'planNumber', 'select');
        await fillInput(container, 'planNumber', '123456');
      });
      const searchButton = getByTitle('search');

      await act(async () => userEvent.click(searchButton));
      await waitFor(() => {
        expect(onSearch).toHaveBeenCalledWith({
          ...defaultLayerFilter,
          searchBy: 'planNumber',
          planNumber: '123456',
        });
      });
    });

    it('can reset the search criteria', async () => {
      const { getByTitle, findByText, queryByDisplayValue, container } = setup({});
      expect(await findByText('No results found for your search criteria.')).toBeInTheDocument();
      await act(async () => {
        await fillInput(container, 'searchby', 'pid', 'select');
        await fillInput(container, 'pid', '123-456-789');
      });
      expect(queryByDisplayValue('123-456-789')).toBeVisible(); //ensure that expected input value is present.

      const resetButton = getByTitle('reset-button');
      await act(async () => userEvent.click(resetButton));
      expect(onSearch).toHaveBeenCalledWith(defaultLayerFilter);
      expect(queryByDisplayValue('123-456-789')).toBeNull(); //input value should now be cleared.
    });
  });
  describe('search results display', () => {
    it('displays 5 search results at a time', async () => {
      const { getByText, getAllByRole } = setup({
        searchResults: toMapProperty(mockPropertyLayerSearchResponse.features),
      });
      expect(getByText(`1 - 5 of 7`)).toBeVisible();
      expect(getAllByRole('row')).toHaveLength(6);
    });

    it('the search results are paged and paging works as expected', async () => {
      const { getByText, getByLabelText, getAllByRole } = setup({
        searchResults: toMapProperty(mockPropertyLayerSearchResponse.features),
      });
      const nextPage = getByLabelText('Page 2');
      await act(async () => userEvent.click(nextPage));
      await waitFor(async () => {
        expect(getByText(`6 - 7 of 7`)).toBeVisible();
        expect(getAllByRole('row')).toHaveLength(3);
      });
    });

    it('does not display results but displays a warning when more then 15 results are returned', async () => {
      const { getByText } = setup({
        searchResults: toMapProperty([
          ...mockPropertyLayerSearchResponse.features,
          ...mockPropertyLayerSearchResponse.features,
          ...mockPropertyLayerSearchResponse.features,
        ]),
      });
      expect(
        getByText(
          `Too many results (more than 15) match this criteria. Please refine your search.`,
        ),
      ).toBeVisible();
    });
  });

  describe('selecting results', () => {
    it('search results can be selected', async () => {
      const { findByTestId } = setup({
        searchResults: toMapProperty(mockPropertyLayerSearchResponse.features),
      });
      const checkbox = await findByTestId('selectrow-PID-006-772-331');
      await act(async () => userEvent.click(checkbox));
      expect(checkbox).toBeChecked();
      expect(onSelectedProperties).toHaveBeenCalledWith([
        {
          address: 'unknown',
          areaUnit: 'M2',
          district: undefined,
          districtName: undefined,
          id: 'PID-006-772-331',
          landArea: 4478.6462,
          latitude: 55.706191605,
          longitude: -121.60790412,
          name: undefined,
          pid: '006772331',
          pin: '10514131',
          planNumber: 'PGP27005',
          propertyId: undefined,
          region: undefined,
          regionName: undefined,
        },
      ]);
    });
  });
});

function toMapProperty(propertyFeatures: any[]) {
  return propertyFeatures.map<IMapProperty>(x => {
    return mapFeatureToProperty(x);
  });
}
