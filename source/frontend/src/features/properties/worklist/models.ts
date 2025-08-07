import { Feature, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { v4 as uuidv4 } from 'uuid';

import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';

export class ParcelFeature {
  public id: string;
  public name: string;
  location: LatLngLiteral | null;
  public feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;

  public constructor(id?: string) {
    this.id = id ?? uuidv4();
    this.name = '';
    this.location = null;
    this.feature = null;
  }

  public static fromFullyAttributedFeature(
    feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
  ) {
    const parcelFeature = new ParcelFeature();
    parcelFeature.feature = feature;
    return parcelFeature;
  }
}
