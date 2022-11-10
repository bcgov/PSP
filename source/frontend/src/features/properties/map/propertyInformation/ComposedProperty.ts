import { AxiosResponse } from 'axios';
import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { IWfsGetAllFeaturesOptions } from 'hooks/pims-api';
import { IResponseWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IBcAssessmentSummary } from 'hooks/useBcAssessmentLayer';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { Api_Property, Api_PropertyAssociations } from 'models/api/Property';

export default interface ComposedProperty {
  pid?: string;
  pin?: string;
  id?: number;
  ltsaWrapper?: IResponseWrapper<(pid: string) => Promise<AxiosResponse<LtsaOrders, any>>>;
  apiWrapper?: IResponseWrapper<(id: number) => Promise<AxiosResponse<Api_Property, any>>>;
  propertyAssociationWrapper?: IResponseWrapper<
    (id: number) => Promise<AxiosResponse<Api_PropertyAssociations, any>>
  >;
  parcelMapWrapper?: IResponseWrapper<
    (
      filter?: Record<string, string>,
      options?: IWfsGetAllFeaturesOptions | undefined,
    ) => Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>, any>>
  >;
  geoserverWrapper?: IResponseWrapper<
    (id: number) => Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>, any>>
  >;
  bcAssessmentWrapper?: IResponseWrapper<
    (pid: string) => Promise<AxiosResponse<IBcAssessmentSummary, any>>
  >;
  composedLoading: boolean;
}
