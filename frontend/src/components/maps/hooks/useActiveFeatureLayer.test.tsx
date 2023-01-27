import { waitFor } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { createMemoryHistory } from 'history';
import { geoJSON } from 'leaflet';
import { noop } from 'lodash';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import useActiveFeatureLayer from './useActiveFeatureLayer';

const mapRef = { current: { leafletMap: {} } };
jest.mock('leaflet');
jest.mock('components/maps/leaflet/LayerPopup');

let clearLayers = jest.fn();
let addData = jest.fn();
(geoJSON as jest.Mock).mockReturnValue({
  addTo: () => ({ clearLayers, addData } as any),
});

const useLayerQueryMock = {
  findOneWhereContains: jest.fn(),
  findMetadataByLocation: jest.fn(),
};
(useLayerQuery as jest.Mock).mockReturnValue(useLayerQueryMock);

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();
const getStore = (values?: any) => mockStore(values ?? { properties: {} });
const getWrapper = (store: any) => ({ children }: any) => (
  <Provider store={store}>
    <Router history={history}>{children}</Router>
  </Provider>
);

describe('useActiveFeatureLayer hook tests', () => {
  beforeEach(() => {
    useLayerQueryMock.findMetadataByLocation.mockResolvedValue({
      REGION_NUMBER: 2,
      REGION_NAME: 'South Coast',
      DISTRICT_NUMBER: 2,
      DISTRICT_NAME: 'Vancouver Island',
    });
  });
  afterEach(() => {
    clearLayers.mockClear();
    addData.mockClear();
    useLayerQueryMock.findOneWhereContains.mockClear();
    useLayerQueryMock.findMetadataByLocation.mockClear();
  });

  it('sets the active feature only when there is a selected property', async () => {
    useLayerQueryMock.findOneWhereContains.mockResolvedValue({ features: [{ properties: [{}] }] });
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
    // call to parcelmap BC
    expect(useLayerQueryMock.findOneWhereContains).toHaveBeenCalledTimes(1);
    // calls to region and district layers
    expect(useLayerQueryMock.findMetadataByLocation).toHaveBeenCalledTimes(2);
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
    useLayerQueryMock.findOneWhereContains.mockResolvedValue({});
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
    // call to parcelmap BC
    expect(useLayerQueryMock.findOneWhereContains).toHaveBeenCalledTimes(1);
    // calls to region and district layers
    expect(useLayerQueryMock.findMetadataByLocation).toHaveBeenCalledTimes(2);
    await waitFor(() => {
      expect(geoJSON().addTo({} as any).addData).not.toHaveBeenCalled();
    });
  });
});
