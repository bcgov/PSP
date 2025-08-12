import { Feature, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { v4 as uuidv4 } from 'uuid';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';

export class ParcelDataset {
  public id: string;
  public name: string;
  location: LatLngLiteral | null;
  public pmbcFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
  public regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  public districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;

  public constructor(id?: string) {
    this.id = id ?? uuidv4();
    this.name = '';
    this.location = null;
    this.pmbcFeature = null;
    this.regionFeature = null;
    this.districtFeature = null;
  }

  public static fromFullyAttributedFeature(
    feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
  ) {
    const parcel = new ParcelDataset();
    parcel.pmbcFeature = feature;
    return parcel;
  }

  public static fromSelectedFeatureDataset(featureSet: SelectedFeatureDataset): ParcelDataset {
    const parcel = new ParcelDataset();
    parcel.id = featureSet.id ?? uuidv4();
    parcel.location = featureSet.location;
    parcel.pmbcFeature = featureSet.parcelFeature;
    parcel.regionFeature = featureSet.regionFeature;
    parcel.districtFeature = featureSet.districtFeature;
    return parcel;
  }

  public toSelectedFeatureDataset(): SelectedFeatureDataset {
    return {
      selectingComponentId: null,
      location: this.location ?? { lat: 0, lng: 0 },
      fileLocation: this.location ?? null,
      id: this.id,
      parcelFeature: this.pmbcFeature ?? null,
      pimsFeature: null,
      regionFeature: this.regionFeature ?? null,
      districtFeature: this.districtFeature ?? null,
      municipalityFeature: null,
      isActive: true,
      displayOrder: 0,
    };
  }
}
