import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { noop } from 'lodash';
import { mockPropertyLayerSearchResponse } from 'mocks/filterDataMock';
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
    const component = render(
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
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
    mockAxios.resetHistory();
  });
  it('searches by pid', async () => {
    mockAxios.onGet().reply(200, mockPropertyLayerSearchResponse);
    const {
      component: { getByTitle, getByText, container },
    } = setup({});

    getByText('No results found for your search criteria.');
    await fillInput(container, 'searchBy', 'pid', 'select');
    await fillInput(container, 'pid', '123-456-789');
    const searchButton = getByTitle('search');

    userEvent.click(searchButton);
    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toBe(
        "https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW&srsName=EPSG:4326&cql_filter=PID ilike '%25123456789%25'",
      );
    });
  });
  it('searches by pin', async () => {
    mockAxios.onGet().reply(200, mockPropertyLayerSearchResponse);
    const {
      component: { getByTitle, getByText, container },
    } = setup({});

    getByText('No results found for your search criteria.');
    await fillInput(container, 'searchBy', 'pin', 'select');
    await fillInput(container, 'pin', '54321');
    const searchButton = getByTitle('search');

    userEvent.click(searchButton);
    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toBe(
        "https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW&srsName=EPSG:4326&cql_filter=PIN ilike '%2554321%25'",
      );
    });
  });
  it('searches by planNumber', async () => {
    mockAxios.onGet().reply(200, mockPropertyLayerSearchResponse);
    const {
      component: { getByTitle, getByText, container },
    } = setup({});

    getByText('No results found for your search criteria.');
    await fillInput(container, 'searchBy', 'planNumber', 'select');
    await fillInput(container, 'planNumber', 'PRP4520');
    const searchButton = getByTitle('search');

    userEvent.click(searchButton);
    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toBe(
        "https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW&srsName=EPSG:4326&cql_filter=PLAN_NUMBER ilike '%25PRP4520%25'",
      );
    });
  });
});
