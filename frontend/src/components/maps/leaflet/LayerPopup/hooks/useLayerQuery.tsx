import axios, { AxiosError } from 'axios';
import { layerData } from 'constants/toasts';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { geoJSON, LatLngLiteral } from 'leaflet';
import { Dispatch, useCallback } from 'react';
import { toast } from 'react-toastify';
import * as rax from 'retry-axios';
import { useAppSelector } from 'store/hooks';
import { logError } from 'store/slices/network/networkSlice';
import parcelLayerDataSlice, {
  saveParcelLayerData,
} from 'store/slices/parcelLayerData/parcelLayerDataSlice';

export interface IUserLayerQuery {
  /**
   * function to find GeoJSON shape containing a point (x, y)
   * @param latlng = {lat, lng}
   */
  findOneWhereContains: (latlng: LatLngLiteral) => Promise<FeatureCollection>;
  /**
   * function to find GeoJSON shape matching the passed non-zero padded pid.
   * @param pid
   */
  findByPid: (pid: string) => Promise<FeatureCollection>;
  /**
   * function to find GeoJSON shape matching the passed pin.
   * @param pin
   */
  findByPin: (pin: string) => Promise<FeatureCollection>;
  /**
   * function to find GeoJSON shape matching the passed administrative area.
   * @param city
   */
  findByAdministrative: (city: string) => Promise<Feature | null>;
  /**
   * Standard logic to handle a parcel layer data response, independent of whether this is a lat/lng or pid query response.
   * @param response axios response
   * @param dispatch redux store, required to save results.
   */
  handleParcelDataLayerResponse: (
    response: Promise<FeatureCollection<Geometry, GeoJsonProperties>>,
    dispatch: Dispatch<any>,
    latLng?: LatLngLiteral,
  ) => Promise<void>;
}

/**
 * Save the parcel data layer response to redux for use within other components. Also save an entire copy of the feature for display on the map.
 * @param resp
 * @param dispatch
 */
export const saveParcelDataLayerResponse = (
  resp: FeatureCollection<Geometry, GeoJsonProperties>,
  dispatch: Dispatch<any>,
  latLng?: LatLngLiteral,
) => {
  if (resp?.features?.length > 0) {
    //save with a synthetic event to timestamp the relevance of this data.
    dispatch(
      saveParcelLayerData({
        e: { timeStamp: document?.timeline?.currentTime ?? 0 } as any,
        data: {
          ...resp.features[0].properties!,
          CENTER:
            latLng ??
            geoJSON(resp.features[0].geometry)
              .getBounds()
              .getCenter(),
        },
      }),
    );
  } else {
    toast.warning(`Failed to find parcel layer data. Ensure that the search criteria is valid`);
  }
};

/**
 * Standard logic to handle a parcel layer data response, independent of whether this is a lat/lng or pid query response.
 * @param response axios response
 * @param dispatch redux store, required to save results.
 */
export const handleParcelDataLayerResponse = (
  response: Promise<FeatureCollection<Geometry, GeoJsonProperties>>,
  dispatch: Dispatch<any>,
  latLng?: LatLngLiteral,
) => {
  return response
    .then((resp: FeatureCollection<Geometry, GeoJsonProperties>) => {
      saveParcelDataLayerResponse(resp, dispatch, latLng);
    })
    .catch((axiosError: AxiosError) => {
      dispatch(
        logError({
          name: parcelLayerDataSlice.reducer.name,
          status: axiosError?.response?.status,
          error: axiosError,
        }),
      );
    });
};
const MAX_RETRIES = 2;
const wfsAxios = () => {
  const instance = axios.create({ timeout: 5000 });
  instance.defaults.raxConfig = {
    retry: MAX_RETRIES,
    instance: instance,
    shouldRetry: (error: AxiosError) => {
      const cfg = rax.getConfig(error);
      if (cfg?.currentRetryAttempt === MAX_RETRIES) {
        toast.dismiss(layerData.LAYER_DATA_LOADING_ID);
        layerData.LAYER_DATA_ERROR();
      }
      return rax.shouldRetryRequest(error);
    },
  };
  rax.attach(instance);

  instance.interceptors.request.use(config => {
    layerData.LAYER_DATA_LOADING();
    return config;
  });
  return instance;
};

/**
 * Custom hook to fetch layer feature collection from wfs url
 * @param url wfs request url
 * @param geometry the name of the geometry in the feature collection
 */
export const useLayerQuery = (url: string, geometryName: string = 'SHAPE'): IUserLayerQuery => {
  const parcelLayerData = useAppSelector(state => state.parcelLayerData?.parcelLayerData);
  const baseUrl = `${url}&srsName=EPSG:4326&count=1`;

  const findOneWhereContains = useCallback(
    async (latlng: LatLngLiteral): Promise<FeatureCollection> => {
      const data: FeatureCollection = (
        await wfsAxios().get(
          `${baseUrl}&cql_filter=CONTAINS(${geometryName},SRID=4326;POINT ( ${latlng.lng} ${latlng.lat}))`,
        )
      )?.data;
      return data;
    },
    [baseUrl, geometryName],
  );

  const findByAdministrative = useCallback(
    async (city: string): Promise<Feature | null> => {
      try {
        const data: any = (
          await wfsAxios().get(
            `${baseUrl}&cql_filter=ADMIN_AREA_NAME='${city}' OR ADMIN_AREA_ABBREVIATION='${city}'&outputformat=json`,
          )
        )?.data;

        if (data.totalFeatures === 0) {
          return null;
        }
        return data.features[0];
      } catch (error) {
        console.log('Failed to find municipality feature', error);
        return null;
      }
    },
    [baseUrl],
  );

  const findByPid = useCallback(
    async (pid: string): Promise<FeatureCollection> => {
      //Do not make a request if we our currently cached response matches the requested pid.
      const formattedPid = pid.replace(/-/g, '');
      const data: FeatureCollection =
        parcelLayerData?.data?.PID === formattedPid ||
        parcelLayerData?.data?.PID_NUMBER.toString() === formattedPid
          ? undefined
          : (await wfsAxios().get(`${baseUrl}&CQL_FILTER=PID_NUMBER=${+formattedPid}`)).data;
      return data;
    },
    [baseUrl, parcelLayerData],
  );

  const findByPin = useCallback(
    async (pin: string): Promise<FeatureCollection> => {
      //Do not make a request if we our currently cached response matches the requested pid.
      const data: FeatureCollection =
        parcelLayerData?.data?.PIN === pin
          ? undefined
          : (await wfsAxios().get(`${baseUrl}&CQL_FILTER=PIN=${pin}`)).data;
      return data;
    },
    [baseUrl, parcelLayerData],
  );

  return {
    findOneWhereContains,
    findByPid,
    findByPin,
    findByAdministrative,
    handleParcelDataLayerResponse,
  };
};
