import { FeatureCollection, Geometry } from 'geojson';

import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { IKeycloak } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { firstOrNull } from '@/utils';

import { Api_GenerateProduct } from '../GenerateProduct';
import { Api_GenerateProject } from '../GenerateProject';
import { Api_GenerateProperty } from '../GenerateProperty';

export class Api_GenerateAcquisitionPropertyIntake {
  user_full_name: string;
  project: Api_GenerateProject;
  product: Api_GenerateProduct;
  project_name: string;
  project_number: string;
  today: UtcIsoDateTime;
  property: Api_GenerateProperty;
  property_historical_lis: string;
  file_number: string;
  isStrata: boolean;

  constructor(
    file: ApiGen_Concepts_AcquisitionFile,
    composedProperty: ComposedProperty,
    lisHistoricalNumbers: ApiGen_Concepts_HistoricalFileNumber[],
    keycloak: IKeycloak,
  ) {
    this.user_full_name =
      keycloak.displayName ??
      (!!keycloak.firstName && !!keycloak.surname
        ? `${keycloak.firstName} ${keycloak.surname}`
        : '');
    this.file_number = file?.fileNumber ?? '';
    this.project = new Api_GenerateProject(file?.project ?? null);
    this.product = new Api_GenerateProduct(file?.product ?? null);

    this.file_number = file?.fileNumber ?? '';
    this.project_name = this.project?.name ?? '';
    this.project_number = this.project?.number ?? '';

    const parcelMapFeatures =
      (
        composedProperty.parcelMapFeatureCollection as FeatureCollection<
          Geometry,
          PMBC_FullyAttributed_Feature_Properties
        >
      )?.features ?? [];

    this.property = new Api_GenerateProperty(
      composedProperty.pimsProperty,
      firstOrNull(parcelMapFeatures)?.properties,
    );
    this.property_historical_lis = lisHistoricalNumbers.map(x => x.historicalFileNumber).join(', ');
  }
}
