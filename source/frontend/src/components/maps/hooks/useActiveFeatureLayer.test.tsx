import { waitFor } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { createMemoryHistory } from 'history';
import { geoJSON } from 'leaflet';
import { noop } from 'lodash';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useMapProperties } from '@/hooks/repositories/useMapProperties';

import { createPoints } from '../leaflet/Layers/util';
import useActiveFeatureLayer from './useActiveFeatureLayer';

const mapRef = { current: { leafletMap: {} } };
jest.mock('leaflet');
jest.mock('@/components/maps/leaflet/LayerPopup/components/LayerPopupContent');
jest.mock('@/hooks/layer-api/useLayerQuery');
jest.mock('@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer');
jest.mock('@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer');
jest.mock('@/hooks/repositories/useMapProperties');

let clearLayers = jest.fn();
let addData = jest.fn();
const setLayerPopup = jest.fn();
(geoJSON as jest.Mock).mockReturnValue({
  addTo: () => ({ clearLayers, addData } as any),
  getBounds: jest.fn(),
});

const useFullyAttributedParcelMapLayerMock = {
  findByLegalDescription: jest.fn(),
  findByPid: jest.fn(),
  findByPin: jest.fn(),
  findByPlanNumber: jest.fn(),
  findByWrapper: jest.fn(),
  findOne: jest.fn(),
};
(useFullyAttributedParcelMapLayer as jest.Mock).mockReturnValue(
  useFullyAttributedParcelMapLayerMock,
);

const useAdminBoundaryMapLayerMock = {
  findRegion: jest.fn(),
  findDistrict: jest.fn(),
  findElectoralDistrict: jest.fn(),
};
(useAdminBoundaryMapLayer as jest.Mock).mockReturnValue(useAdminBoundaryMapLayerMock);

const useLayerQueryMock = {
  findOneWhereContains: jest.fn(),
  findByPid: jest.fn(),
  findByPin: jest.fn(),
  findByPlanNumber: jest.fn(),
  findMetadataByLocation: jest.fn(),
  findOneWhereContainsWrapped: { execute: jest.fn() },
};
(useLayerQuery as jest.Mock).mockReturnValue(useLayerQueryMock);

(useMapProperties as unknown as jest.Mock<Partial<typeof useMapProperties>>).mockReturnValue({
  loadProperties: {
    execute: jest.fn().mockResolvedValue({
      features: createPoints([
        {
          id: 1,
          latitude: 54.917061,
          longitude: -122.749672,
          pid: '123-456-789',
          address: { provinceId: 1, streetAddress1: 'test' },
        },
      ]),
      type: 'FeatureCollection',
      bbox: undefined,
    }),
  },
});

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();
const getStore = (values?: any) => mockStore(values ?? { properties: {} });
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    (
      <Provider store={store}>
        <Router history={history}>{children}</Router>
      </Provider>
    );

describe('useActiveFeatureLayer hook tests', () => {
  beforeEach(() => {
    useAdminBoundaryMapLayerMock.findDistrict.mockResolvedValue({
      properties: {
        DISTRICT_NUMBER: 2,
        DISTRICT_NAME: 'Vancouver Island',
      },
    });

    useAdminBoundaryMapLayerMock.findRegion.mockResolvedValue({
      properties: {
        REGION_NUMBER: 2,
        REGION_NAME: 'South Coast',
      },
    });
  });
  afterEach(() => {
    clearLayers.mockClear();
    addData.mockClear();
    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockClear();
    useLayerQueryMock.findMetadataByLocation.mockClear();
    useAdminBoundaryMapLayerMock.findDistrict.mockClear();
    useAdminBoundaryMapLayerMock.findRegion.mockClear();
  });

  it('sets the active feature only when there is a selected property', async () => {
    useFullyAttributedParcelMapLayerMock.findOne.mockResolvedValueOnce({
      geometry: { type: 'Polygon', coordinates: [1, 2] },
      properties: [{}],
    });

    renderHook(
      () =>
        useActiveFeatureLayer({
          mapRef: mapRef as any,
          selectedProperty: { latitude: 1, longitude: 1 } as any,
          layerPopup: undefined,
          setLayerPopup: noop,
        }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
    expect(clearLayers).toHaveBeenCalled();
    // call to parcelmap BC and to internal pims layer
    expect(useFullyAttributedParcelMapLayerMock.findOne).toHaveBeenCalledTimes(1);
    // calls to region and district layers
    expect(useAdminBoundaryMapLayerMock.findDistrict).toHaveBeenCalledTimes(1);
    expect(useAdminBoundaryMapLayerMock.findRegion).toHaveBeenCalledTimes(1);

    await waitFor(() => {
      expect(geoJSON().addTo({} as any).addData).toHaveBeenCalledTimes(1);
      expect(geoJSON().addTo({} as any).addData).toHaveBeenCalledWith(
        expect.objectContaining({
          properties: expect.objectContaining({
            REGION_NUMBER: 2,
            REGION_NAME: 'South Coast',
            DISTRICT_NUMBER: 2,
            DISTRICT_NAME: 'Vancouver Island',
          }),
        }),
      );
    });
  });

  it('does not set the active parcel when the selected property has no matching parcel data', async () => {
    useFullyAttributedParcelMapLayerMock.findOne.mockResolvedValueOnce(undefined);
    renderHook(
      () =>
        useActiveFeatureLayer({
          mapRef: mapRef as any,
          selectedProperty: { latitude: 1, longitude: 1 } as any,
          layerPopup: undefined,
          setLayerPopup: noop,
        }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
    expect(clearLayers).toHaveBeenCalled();
    // call to parcelmap BC and to internal pims layer
    expect(useFullyAttributedParcelMapLayerMock.findOne).toHaveBeenCalledTimes(1);
    // calls to region and district layers
    expect(useAdminBoundaryMapLayerMock.findDistrict).toHaveBeenCalledTimes(1);
    expect(useAdminBoundaryMapLayerMock.findRegion).toHaveBeenCalledTimes(1);
    await waitFor(() => {
      expect(geoJSON().addTo({} as any).addData).not.toHaveBeenCalled();
    });
  });

  it('sets the layer popup with the expected data', async () => {
    useFullyAttributedParcelMapLayerMock.findOne.mockResolvedValueOnce({
      geometry: { type: 'Polygon', coordinates: [1, 2] },
      properties: { PID: '123456789' },
    });

    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockResolvedValueOnce({
      features: [{ properties: { pid: '123456789' } }],
    });
    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockResolvedValueOnce({
      features: [{ properties: { PROPERTY_ID: 200 } }],
    });
    renderHook(
      () =>
        useActiveFeatureLayer({
          mapRef: mapRef as any,
          selectedProperty: { latitude: 1, longitude: 1 } as any,
          layerPopup: undefined,
          setLayerPopup: setLayerPopup,
        }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
    await waitFor(() => {
      expect(setLayerPopup).toHaveBeenCalledWith(
        expect.objectContaining({
          data: { PID: '123456789' },
          title: 'LTSA ParcelMap data',
        }),
      );
    });
  });

  it('sets the layer popup with the expected municipality data', async () => {
    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockResolvedValueOnce({
      features: [],
    });
    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockResolvedValueOnce({
      features: [{ properties: { PROPERTY_ID: 200 } }],
    });
    //this will return data for the municipality layer.
    renderHook(
      () =>
        useActiveFeatureLayer({
          mapRef: mapRef as any,
          selectedProperty: { latitude: 1, longitude: 1 } as any,
          layerPopup: undefined,
          setLayerPopup: setLayerPopup,
        }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
    await waitFor(() => {
      expect(setLayerPopup).toHaveBeenCalledWith(
        expect.objectContaining({
          data: { PROPERTY_ID: 200 },
          title: 'Municipality Information',
        }),
      );
    });
  });

  it('sets the layer popup with no data', async () => {
    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockResolvedValueOnce({
      features: [],
    });
    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockResolvedValueOnce({
      features: [],
    });
    useLayerQueryMock.findOneWhereContainsWrapped.execute.mockResolvedValueOnce({
      features: [],
    });
    renderHook(
      () =>
        useActiveFeatureLayer({
          mapRef: mapRef as any,
          selectedProperty: { latitude: 1, longitude: 1 } as any,
          layerPopup: undefined,
          setLayerPopup: setLayerPopup,
        }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
    await waitFor(() => {
      expect(setLayerPopup).toHaveBeenCalledWith(
        expect.objectContaining({
          data: undefined,
          title: 'Location Information',
        }),
      );
    });
  });
});
