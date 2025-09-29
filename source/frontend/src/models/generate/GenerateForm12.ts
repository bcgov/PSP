import { FeatureCollection, Geometry } from 'geojson';
import moment from 'moment';

import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { convertArea, exists, firstOrNull } from '@/utils';

import { ApiGen_CodeTypes_AreaUnitTypes } from '../api/generated/ApiGen_CodeTypes_AreaUnitTypes';
import { PMBC_FullyAttributed_Feature_Properties } from '../layers/parcelMapBC';
import { Api_GenerateProperty } from './GenerateProperty';
export class Api_GenerateForm12 {
  first_property_plan_number: string;
  date_today: string;
  properties: Api_GenerateProperty[];
  total_area_ha: number;

  constructor(composedProperties: ComposedProperty[]) {
    this.properties = composedProperties.filter(exists).map(p => {
      const parcelMapFeatures =
        (
          p.parcelMapFeatureCollection as FeatureCollection<
            Geometry,
            PMBC_FullyAttributed_Feature_Properties
          >
        )?.features ?? [];

      return new Api_GenerateProperty(p.pimsProperty, firstOrNull(parcelMapFeatures)?.properties);
    });

    const firstProperty = firstOrNull(this.properties);
    this.first_property_plan_number = firstProperty?.plan_number;

    const totalSqm = this.properties.reduce(
      (accumulator, current) => accumulator + current.area_sqm,
      0,
    );
    this.total_area_ha = convertArea(
      totalSqm,
      ApiGen_CodeTypes_AreaUnitTypes.M2,
      ApiGen_CodeTypes_AreaUnitTypes.HA,
    );

    this.date_today = moment().format('MMMM Do, YYYY');
  }
}
