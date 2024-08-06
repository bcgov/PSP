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
import { featureToLocationFeatureDataset } from './PropertySelectorSearchContainer';

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

    it('can search for a legal description', async () => {
      const { getByTitle, getByText, container } = setup({
        search: { ...defaultLayerFilter, searchBy: 'legalDescription' },
      });

      expect(getByText('No results found for your search criteria.')).toBeInTheDocument();
      await act(async () => {
        await fillInput(container, 'searchBy', 'legalDescription', 'select');
        await fillInput(
          container,
          'legalDescription',
          'SECTION 13, RANGE 1, SOUTH SALT SPRING ISLAND',
          'textarea',
        );
      });

      const searchButton = getByTitle('search');
      act(() => userEvent.click(searchButton));

      await waitFor(() => {
        expect(onSearch).toHaveBeenCalledWith({
          ...defaultLayerFilter,
          searchBy: 'legalDescription',
          legalDescription: 'SECTION 13, RANGE 1, SOUTH SALT SPRING ISLAND',
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
        searchResults: mockPropertyLayerSearchResponse.features.map(f =>
          featureToLocationFeatureDataset(f as any),
        ),
      });
      expect(getByText(`1 - 5 of 7`)).toBeVisible();
      expect(getAllByRole('row')).toHaveLength(6);
    });

    it('the search results are paged and paging works as expected', async () => {
      const { getByText, getByLabelText, getAllByRole } = setup({
        searchResults: mockPropertyLayerSearchResponse.features.map(f =>
          featureToLocationFeatureDataset(f as any),
        ),
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
        searchResults: [
          ...mockPropertyLayerSearchResponse.features,
          ...mockPropertyLayerSearchResponse.features,
          ...mockPropertyLayerSearchResponse.features,
        ].map(f => featureToLocationFeatureDataset(f as any)),
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
        searchResults: mockPropertyLayerSearchResponse.features.map(f =>
          featureToLocationFeatureDataset(f as any),
        ),
      });
      const checkbox = await findByTestId(
        'selectrow-PID-006-772-331-55.706230240625004--121.60834946062499',
      );
      await act(async () => userEvent.click(checkbox));
      expect(checkbox).toBeChecked();
      expect(onSelectedProperties).toHaveBeenCalledWith([
        {
          districtFeature: null,
          id: 'PID-006-772-331-55.706230240625004--121.60834946062499',
          location: {
            lat: 55.706230240625004,
            lng: -121.60834946062499,
          },
          municipalityFeature: null,
          highwayFeature: null,
          parcelFeature: {
            geometry: {
              coordinates: [
                [
                  [-121.60861991, 55.70650025],
                  [-121.60861925, 55.70588252],
                  [-121.60728684, 55.7061924],
                  [-121.60718833, 55.70627546],
                  [+-121.60718846, 55.70643785],
                  [-121.60729988, 55.70650069],
                  [-121.60861991, 55.70650025],
                ],
              ],
              type: 'Polygon',
            },
            geometry_name: 'SHAPE',
            id: 'WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW.fid-674bf6f8_180d8c9b18e_7c12',
            properties: {
              FEATURE_AREA_SQM: 4478.6462,
              FEATURE_LENGTH_M: 281.3187,
              MUNICIPALITY: 'Chetwynd, District of',
              OBJECTID: 601612446,
              OWNER_TYPE: 'Private',
              PARCEL_CLASS: 'Subdivision',
              PARCEL_FABRIC_POLY_ID: 1994518,
              PARCEL_NAME: '006772331',
              PARCEL_START_DATE: null,
              PARCEL_STATUS: 'Active',
              PID: '006772331',
              PID_NUMBER: 6772331,
              PIN: 10514131,
              PLAN_NUMBER: 'PGP27005',
              REGIONAL_DISTRICT: 'Peace River Regional District',
              SE_ANNO_CAD_DATA: null,
              WHEN_UPDATED: '2019-01-09Z',
            },
            type: 'Feature',
          },
          pimsFeature: null,
          regionFeature: null,
          selectingComponentId: null,
        },
      ]);
    });
  });
});
