import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { PointFeature } from 'components/maps/types';
import { IProperty } from 'interfaces';

import { IPropertyDetail, IPropertyState, IStorePropertyDetail } from './interfaces';

export const initialState: IPropertyState = {
  properties: [],
  draftProperties: [],
  propertyDetail: null,
  pid: 0,
};

const getPropertyDetail = (
  property: IProperty | null,
  position?: [number, number],
): IPropertyDetail => {
  if (property === null) {
    return { propertyDetail: null, propertyTypeId: undefined };
  }
  return { propertyDetail: property, propertyTypeId: property.propertyTypeId, position };
};

/**
 * The properties reducer provides actions to manage the displayed properties used within the application.
 */
export const propertiesSlice = createSlice({
  name: 'properties',
  initialState: initialState,
  reducers: {
    storeProperties(state: IPropertyState, action: PayloadAction<IProperty[]>) {
      state.properties = action.payload;
    },
    storeDraftProperties(state: IPropertyState, action: PayloadAction<PointFeature[]>) {
      state.draftProperties = action.payload;
    },
    storePropertiesFromMap(state: IPropertyState, action: PayloadAction<IProperty>) {
      state.properties = [
        ...state.properties.filter(property => property.id !== action.payload.id),
        action.payload,
      ];
    },
    storeProperty(
      state: IPropertyState,
      action: PayloadAction<IStorePropertyDetail | IProperty | null>,
    ) {
      const storeProperty = action.payload as IStorePropertyDetail;
      if (!!storeProperty?.property) {
        state.propertyDetail = getPropertyDetail(
          storeProperty?.property ?? null,
          storeProperty?.position,
        );
      } else {
        state.propertyDetail = getPropertyDetail(action.payload as IProperty | null);
      }
    },
  },
});

// Destructure and export the plain action creators
export const {
  storeProperties,
  storeDraftProperties,
  storePropertiesFromMap,
  storeProperty,
} = propertiesSlice.actions;
