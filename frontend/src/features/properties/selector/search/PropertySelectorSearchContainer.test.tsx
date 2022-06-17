import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { noop } from 'lodash';
import {
  mockDistrictLayerResponse,
  mockGeocoderOptions,
  mockGeocoderPidsResponse,
  mockMotiRegionLayerResponse,
  mockPropertyLayerSearchResponse,
} from 'mocks';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import {
  IPropertySelectorSearchContainerProps,
  PropertySelectorSearchContainer,
} from './PropertySelectorSearchContainer';

const mockAxios = new MockAdapter(axios);

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

const onSelectedProperty = jest.fn();

describe('PropertySelectorSearchContainer component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IPropertySelectorSearchContainerProps>) => {
    // render component under test
    const utils = render(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <PropertySelectorSearchContainer
          setSelectedProperties={onSelectedProperty}
          selectedProperties={[]}
        />
      </Formik>,
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

  beforeEach(() => {
    mockAxios
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs',
        ),
      )
      .reply(200, mockPropertyLayerSearchResponse)
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY/wfs',
        ),
      )
      .reply(200, mockMotiRegionLayerResponse)
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY/wfs',
        ),
      )
      .reply(200, mockDistrictLayerResponse)
      .onGet(new RegExp('tools/geocoder/parcels/pids/*'))
      .reply(200, mockGeocoderPidsResponse)
      .onGet(new RegExp('tools/geocoder/addresses'))
      .reply(200, mockGeocoderOptions);
  });

  afterEach(() => {
    jest.resetAllMocks();
    mockAxios.resetHistory();
  });

  it('searches by pid', async () => {
    const { getByTitle, getByText, container } = setup({});

    expect(getByText('No results found for your search criteria.')).toBeInTheDocument();
    await fillInput(container, 'searchBy', 'pid', 'select');
    await fillInput(container, 'pid', '123-456-789');

    const searchButton = getByTitle('search');
    userEvent.click(searchButton);

    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(15);
      // call parcel map layer
      expect(mockAxios.history.get[0].url).toBe(
        "https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW&srsName=EPSG:4326&cql_filter=PIN ilike '%25123456789%25' OR PID ilike '%25123456789%25'",
      );
      // calls the region and district layers
      expect(mockAxios.history.get[1].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
      expect(mockAxios.history.get[2].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
    });
  });
  it('searches by pin', async () => {
    const { getByTitle, getByText, container } = setup({});

    expect(getByText('No results found for your search criteria.')).toBeInTheDocument();
    await fillInput(container, 'searchBy', 'pin', 'select');
    await fillInput(container, 'pin', '54321');

    const searchButton = getByTitle('search');
    userEvent.click(searchButton);

    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(15);
      expect(mockAxios.history.get[0].url).toBe(
        "https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW&srsName=EPSG:4326&cql_filter=PIN ilike '%2554321%25'",
      );
      // calls the region and district layers
      expect(mockAxios.history.get[1].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
      expect(mockAxios.history.get[2].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
    });
  });
  it('searches by planNumber', async () => {
    const { getByTitle, getByText, container } = setup({});

    expect(getByText('No results found for your search criteria.')).toBeInTheDocument();
    await fillInput(container, 'searchBy', 'planNumber', 'select');
    await fillInput(container, 'planNumber', 'PRP4520');

    const searchButton = getByTitle('search');
    userEvent.click(searchButton);

    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(15);
      expect(mockAxios.history.get[0].url).toBe(
        "https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW&srsName=EPSG:4326&cql_filter=PLAN_NUMBER ilike '%25PRP4520%25'",
      );
      // calls the region and district layers
      expect(mockAxios.history.get[1].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
      expect(mockAxios.history.get[2].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
    });
  });

  it('searches by address', async () => {
    const { container } = setup({});

    await fillInput(container, 'searchBy', 'address', 'select');
    await fillInput(container, 'address', '1234 Fake');

    // typing on address search field should bring up address suggestions
    let addressSuggestions: HTMLElement;
    await waitFor(() => {
      addressSuggestions = container.querySelector('.suggestionList') as HTMLElement;
      expect(addressSuggestions).toBeInTheDocument();
      // clicking on a suggestion should initiate a search by address
      const firstAddress = addressSuggestions?.firstElementChild as HTMLElement;
      expect(firstAddress).toBeInTheDocument();
      userEvent.click(firstAddress);
    });

    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(17);
      expect(mockAxios.history.get[0].url).toBe(
        '/tools/geocoder/addresses?address=1234 Fake&matchPrecisionNot=OCCUPANT,INTERSECTION,BLOCK,STREET,LOCALITY,PROVINCE,OCCUPANT',
      );
      expect(mockAxios.history.get[1].url).toBe('/tools/geocoder/parcels/pids/1');
      // calls parcel layer - search by PID
      expect(mockAxios.history.get[2].url).toBe(
        "https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW&srsName=EPSG:4326&cql_filter=PIN ilike '%25312312%25' OR PID ilike '%25312312%25'",
      );
      // calls the region and district layers
      expect(mockAxios.history.get[3].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
      expect(mockAxios.history.get[4].url).toBe(
        'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -122.6414763 49.104223239999996))',
      );
    });
  });
});
