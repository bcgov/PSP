import { Feature, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { v4 as uuidv4 } from 'uuid';

import {
  LocationFeatureDataset,
  SelectedFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { exists } from '@/utils';

export class ParcelDataset {
  public id: string;
  public name: string;
  location: LatLngLiteral | null;
  public pmbcFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
  public pimsFeature: Feature<Geometry, PIMS_Property_Location_View> | null;
  public regionFeature: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | null;
  public districtFeature: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | null;

  public constructor(id?: string) {
    this.id = id ?? uuidv4();
    this.name = '';
    this.location = null;
    this.pmbcFeature = null;
    this.pimsFeature = null;
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

  public static fromPimsFeature(feature: Feature<Geometry, PIMS_Property_Location_View> | null) {
    const parcel = new ParcelDataset();
    parcel.pimsFeature = feature;
    return parcel;
  }

  public static fromSelectedFeatureDataset(featureSet: SelectedFeatureDataset): ParcelDataset {
    const parcel = new ParcelDataset();
    parcel.id = featureSet.id ?? uuidv4();
    parcel.location = featureSet.location;
    parcel.pmbcFeature = featureSet.parcelFeature;
    parcel.pimsFeature = featureSet.pimsFeature;
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
      pimsFeature: this.pimsFeature ?? null,
      regionFeature: this.regionFeature ?? null,
      districtFeature: this.districtFeature ?? null,
      municipalityFeature: null,
      isActive: true,
      displayOrder: 0,
    };
  }

  public toLocationFeatureDataset(): LocationFeatureDataset {
    const locationDataset: LocationFeatureDataset = {
      parcelFeatures: exists(this.pmbcFeature) ? [this.pmbcFeature] : null,
      pimsFeatures: exists(this.pimsFeature) ? [this.pimsFeature] : null,
      regionFeature: this.regionFeature ?? null,
      districtFeature: this.districtFeature ?? null,
      municipalityFeatures: null,
      highwayFeatures: null,
      crownLandLeasesFeatures: null,
      crownLandLicensesFeatures: null,
      crownLandTenuresFeatures: null,
      crownLandInventoryFeatures: null,
      crownLandInclusionsFeatures: null,
      selectingComponentId: null,
      location: this.location,
      fileLocation: null,
    };
    return locationDataset;
  }
}
