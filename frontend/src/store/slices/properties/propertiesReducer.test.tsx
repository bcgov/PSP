import { PointFeature } from 'components/maps/types';
import { PropertyTypes } from 'constants/propertyTypes';
import { mockBuildingDetail, mockParcelDetail, mockProperty } from 'mocks/filterDataMock';

import { initialState, propertiesSlice } from './propertiesSlice';

const pointFeature: PointFeature = {
  type: 'Feature',
  geometry: { coordinates: [1, 2] } as any,
  properties: { id: 1, propertyTypeId: PropertyTypes.Parcel, name: 'name' },
};

describe('property slice reducer functionality', () => {
  const propertiesReducer = propertiesSlice.reducer;
  it('saves the list of properties', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeParcels,
      payload: [mockProperty],
    });
    expect(result).toEqual({
      ...initialState,
      parcels: [mockProperty],
    });
  });
  it('saves the property detail', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeParcelDetail,
      payload: mockParcelDetail,
    });
    expect(result).toEqual({
      ...initialState,
      propertyDetail: {
        parcelDetail: mockParcelDetail,
        position: undefined,
        propertyTypeId: PropertyTypes.Parcel,
      },
    });
  });
  it('stores the parcel from the map', () => {
    const result = propertiesReducer(
      { ...initialState, parcels: [mockProperty] },
      {
        type: propertiesSlice.actions.storeParcelsFromMap,
        payload: mockProperty,
      },
    );
    expect(result).toEqual({
      ...initialState,
      parcels: [mockProperty],
    });
  });
  it('stores the draft parcels', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeDraftParcels,
      payload: [pointFeature],
    });
    expect(result).toEqual({
      ...initialState,
      draftParcels: [pointFeature],
    });
  });
  it('saves the building detail', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeBuildingDetail,
      payload: mockBuildingDetail,
    });
    expect(result).toEqual({
      ...initialState,
      propertyDetail: {
        parcelDetail: mockBuildingDetail,
        position: undefined,
        propertyTypeId: PropertyTypes.Building,
      },
    });
  });
});
