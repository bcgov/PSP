import { PointFeature } from 'components/maps/types';

import { initialState, propertiesSlice } from './propertiesSlice';

const pointFeature: PointFeature = {
  type: 'Feature',
  geometry: { coordinates: [1, 2] } as any,
  properties: { id: 1, name: 'name' },
};

describe('property slice reducer functionality', () => {
  const propertiesReducer = propertiesSlice.reducer;
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
});
