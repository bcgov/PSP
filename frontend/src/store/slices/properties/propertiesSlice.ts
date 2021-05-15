import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { PointFeature } from 'components/maps/types';
import { IProperty, IParcel, IBuilding } from 'interfaces';
import { IParcelState, IPropertyDetail, IStorePropertyDetail } from './interfaces';

export const initialState: IParcelState = {
  parcels: [],
  draftParcels: [],
  propertyDetail: null,
  associatedBuildingDetail: null,
  pid: 0,
};

const getPropertyDetail = (
  property: IParcel | IBuilding | null,
  position?: [number, number],
): IPropertyDetail => {
  if (property === null) {
    return { parcelDetail: null, propertyTypeId: undefined };
  }
  return { parcelDetail: property, propertyTypeId: property.propertyTypeId, position };
};

/**
 * The properties reducer provides actions to manage the displayed properties used within the application.
 */
export const propertiesSlice = createSlice({
  name: 'properties',
  initialState: initialState,
  reducers: {
    storeParcels(state: IParcelState, action: PayloadAction<IProperty[]>) {
      state.parcels = action.payload;
    },
    storeDraftParcels(state: IParcelState, action: PayloadAction<PointFeature[]>) {
      state.draftParcels = action.payload;
    },
    storeParcelsFromMap(state: IParcelState, action: PayloadAction<IProperty>) {
      state.parcels = [
        ...state.parcels.filter(parcel => parcel.id !== action.payload.id),
        action.payload,
      ];
    },
    storeParcelDetail(
      state: IParcelState,
      action: PayloadAction<IStorePropertyDetail | IBuilding | IParcel | null>,
    ) {
      const storeProperty = action.payload as IStorePropertyDetail;
      if (!!storeProperty?.property) {
        state.propertyDetail = getPropertyDetail(
          storeProperty?.property ?? null,
          storeProperty?.position,
        );
      } else {
        state.propertyDetail = getPropertyDetail(action.payload as IParcel | IBuilding | null);
      }
    },
    storeBuildingDetail(
      state: IParcelState,
      action: PayloadAction<IStorePropertyDetail | IBuilding | IParcel | null>,
    ) {
      const storeProperty = action.payload as IStorePropertyDetail;
      if (!!storeProperty?.property) {
        state.propertyDetail = getPropertyDetail(
          storeProperty?.property ?? null,
          storeProperty?.position,
        );
      } else {
        state.propertyDetail = getPropertyDetail(action.payload as IParcel | IBuilding | null);
      }
    },
  },
});

// Destructure and export the plain action creators
export const {
  storeParcels,
  storeDraftParcels,
  storeParcelsFromMap,
  storeParcelDetail,
  storeBuildingDetail,
} = propertiesSlice.actions;
