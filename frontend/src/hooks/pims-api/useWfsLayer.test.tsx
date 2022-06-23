import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import merge from 'lodash/merge';
import { mockFAParcelLayerResponse } from 'mocks';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { IUseWfsLayerOptions, useWfsLayer } from '.';

const mockAxios = new MockAdapter(axios);
const mockStore = configureMockStore([thunk]);

const wfsUrl = 'ogs-internal/ows';
const wfsLayer = 'PMBC_PARCEL_POLYGON_FABRIC';

describe('useWfsLayer hook', () => {
  const setup = (url: string = wfsUrl, props: Partial<IUseWfsLayerOptions> = {}) => {
    const _options: IUseWfsLayerOptions = merge({ name: wfsLayer }, props);
    const { result } = renderHook(() => useWfsLayer(url, _options), {
      wrapper: props => <TestCommonWrapper store={mockStore}>{props.children}</TestCommonWrapper>,
    });

    return result.current;
  };

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onGet(new RegExp('ogs-internal/ows')).reply(200, mockFAParcelLayerResponse);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('fetches all features matching CQL filter', async () => {
    const { getAllFeatures, getAllFeaturesLoading } = setup();

    await act(async () => {
      const response = await getAllFeatures({ LEGAL_DESCRIPTION: 'some legal description' });
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(getAllFeaturesLoading).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].url).toBe(
      'http://localhost/ogs-internal/ows?service=WFS&version=2.0.0&outputFormat=json&typeNames=PMBC_PARCEL_POLYGON_FABRIC&srsName=EPSG%3A4326&request=GetFeature&cql_filter=LEGAL_DESCRIPTION+ilike+%27%25some+legal+description%25%27',
    );
  });

  it('supports absolute URLs', async () => {
    const url = 'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs';
    mockAxios.onGet(new RegExp(url)).reply(200, mockFAParcelLayerResponse);

    const { getAllFeatures, getAllFeaturesLoading } = setup(url);

    await act(async () => {
      const response = await getAllFeatures({ PID: '000111222' });
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(getAllFeaturesLoading).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].url).toBe(
      'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs?service=WFS&version=2.0.0&outputFormat=json&typeNames=PMBC_PARCEL_POLYGON_FABRIC&srsName=EPSG%3A4326&request=GetFeature&cql_filter=PIN+ilike+%27%25000111222%25%27+OR+PID+ilike+%27%25000111222%25%27++OR+PID_PADDED+ilike+%27%25000111222%25%27',
    );
  });

  it('supports relative URLs', async () => {
    const url = 'geoserver/psp/ows';
    mockAxios.onGet(new RegExp(url)).reply(200, mockFAParcelLayerResponse);

    const { getAllFeatures, getAllFeaturesLoading } = setup(url);

    await act(async () => {
      const response = await getAllFeatures({ PID: '000111222' });
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(getAllFeaturesLoading).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].url).toBe(
      'http://localhost/geoserver/psp/ows?service=WFS&version=2.0.0&outputFormat=json&typeNames=PMBC_PARCEL_POLYGON_FABRIC&srsName=EPSG%3A4326&request=GetFeature&cql_filter=PIN+ilike+%27%25000111222%25%27+OR+PID+ilike+%27%25000111222%25%27++OR+PID_PADDED+ilike+%27%25000111222%25%27',
    );
  });

  it('supports WFS parameters', async () => {
    const props: IUseWfsLayerOptions = {
      name: 'geo:FOOBAR',
      version: '1.3.0',
      outputFormat: 'GML3',
      outputSrsName: 'EPSG:3005',
    };
    const { getAllFeatures, getAllFeaturesLoading } = setup(wfsUrl, props);

    await act(async () => {
      const response = await getAllFeatures();
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(getAllFeaturesLoading).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].url).toBe(
      'http://localhost/ogs-internal/ows?service=WFS&version=1.3.0&outputFormat=GML3&typeNames=geo%3AFOOBAR&srsName=EPSG%3A3005&request=GetFeature',
    );
  });

  it('limits the number of features returned when count parameter is supplied', async () => {
    const { getAllFeatures, getAllFeaturesLoading } = setup(wfsUrl);

    await act(async () => {
      const response = await getAllFeatures({ KEY: 'VALUE' }, { maxCount: 5 });
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(getAllFeaturesLoading).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].url).toBe(
      'http://localhost/ogs-internal/ows?service=WFS&version=2.0.0&outputFormat=json&typeNames=PMBC_PARCEL_POLYGON_FABRIC&srsName=EPSG%3A4326&request=GetFeature&count=5&cql_filter=KEY+ilike+%27%25VALUE%25%27',
    );
  });

  it('times out layer requests after 5 seconds by default', async () => {
    const { getAllFeatures, getAllFeaturesLoading } = setup(wfsUrl);

    await act(async () => {
      const response = await getAllFeatures({ SLOW_FIELD: 'SOME VALUE' });
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(getAllFeaturesLoading).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].timeout).toBe(5000);
  });

  it('supports increasing request timeouts for slow layers', async () => {
    const { getAllFeatures, getAllFeaturesLoading } = setup(wfsUrl);

    await act(async () => {
      const response = await getAllFeatures({ SLOW_FIELD: 'SOME VALUE' }, { timeout: 40000 });
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(getAllFeaturesLoading).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].timeout).toBe(40000);
  });
});
