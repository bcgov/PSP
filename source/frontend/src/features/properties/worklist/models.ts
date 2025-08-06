import { Feature, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { v4 as uuidv4 } from 'uuid';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';

export class ParcelFeature {
  public id: string;
  public name: string;
  location: LatLngLiteral | null;
  public pmbcFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;

  public constructor(id?: string) {
    this.id = id ?? uuidv4();
    this.name = '';
    this.location = null;
    this.pmbcFeature = null;
  }

  public static fromFullyAttributedFeature(
    feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
  ) {
    const parcelFeature = new ParcelFeature();
    parcelFeature.pmbcFeature = feature;
    return parcelFeature;
  }

  public static fromSelectedFeatureDataset(featureSet: SelectedFeatureDataset): ParcelFeature {
    const parcelFeature = new ParcelFeature();
    parcelFeature.id = featureSet.id ?? uuidv4();
    parcelFeature.location = featureSet.location;
    parcelFeature.pmbcFeature = featureSet.parcelFeature;
    return parcelFeature;
  }

  public toSelectedFeatureDataset(): SelectedFeatureDataset {
    return {
      selectingComponentId: null,
      location: this.location ?? { lat: 0, lng: 0 },
      fileLocation: this.location ?? null,
      id: this.id,
      parcelFeature: this.pmbcFeature,
      pimsFeature: null,
      regionFeature: null,
      districtFeature: null,
      municipalityFeature: null,
      isActive: true,
      displayOrder: 0,
    };
  }
}
