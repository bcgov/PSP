import { PointFeature } from 'components/maps/types';
import { PropertyTypes } from 'constants/propertyTypes';
import {
  mockBuildingDetail,
  mockParcel,
  mockParcelDetail,
  mockProperties,
} from 'mocks/filterDataMock';

import { initialState, propertiesSlice } from './propertiesSlice';

const pointFeature: PointFeature = {
  type: 'Feature',
  geometry: { coordinates: [1, 2] } as any,
  properties: { id: 1, propertyTypeId: PropertyTypes.Land, name: 'name' },
};

describe('property slice reducer functionality', () => {
  const propertiesReducer = propertiesSlice.reducer;
  it('saves the list of properties', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeProperties,
      payload: mockProperties,
    });
    expect(result).toEqual({
      ...initialState,
      properties: mockProperties,
    });
  });
  it('saves the property detail', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeProperty,
      payload: mockParcelDetail,
    });
    expect(result).toEqual({
      ...initialState,
      propertyDetail: {
        propertyDetail: mockParcelDetail,
        position: undefined,
        propertyTypeId: PropertyTypes.Land,
      },
    });
  });
  it('stores the parcel from the map', () => {
    const result = propertiesReducer(
      { ...initialState, properties: [mockProperties[0]] },
      {
        type: propertiesSlice.actions.storePropertiesFromMap,
        payload: mockParcel,
      },
    );
    expect(result).toEqual({
      ...initialState,
      properties: [mockProperties[0]],
    });
  });
  it('stores the draft parcels', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeDraftProperties,
      payload: [pointFeature],
    });
    expect(result).toEqual({
      ...initialState,
      draftProperties: [pointFeature],
    });
  });
  it('saves the building detail', () => {
    const result = propertiesReducer(undefined, {
      type: propertiesSlice.actions.storeProperty,
      payload: mockBuildingDetail,
    });
    expect(result).toEqual({
      ...initialState,
      propertyDetail: {
        propertyDetail: mockBuildingDetail,
        position: undefined,
        propertyTypeId: PropertyTypes.Building,
      },
    });
  });
});
