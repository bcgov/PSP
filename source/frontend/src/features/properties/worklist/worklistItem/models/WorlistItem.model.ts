import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { ParcelDataset } from '@/features/properties/parcelList/models';
import {
  isStrataPlanCommonPropertyFromSelectedFeatureSet,
  planFromFeatureSet,
} from '@/utils/mapPropertyUtils';

export class WorklistItemModel {
  private readonly _ds: SelectedFeatureDataset;
  private _parcelGroup: ParcelDataset[];

  constructor(readonly _parcel: ParcelDataset) {
    this._parcel = _parcel;
    this._ds = _parcel.toSelectedFeatureDataset();
    this._parcelGroup = [];
  }

  get Parcel(): ParcelDataset {
    return this._parcel;
  }

  get IsStrataPlanCommonProperty(): boolean {
    return isStrataPlanCommonPropertyFromSelectedFeatureSet(this._ds);
  }

  get PlanNumber(): string {
    return planFromFeatureSet(this._ds);
  }

  get CommonPropertyParcels(): ParcelDataset[] {
    return this._parcelGroup;
  }

  get CommonPropertyParcelsCount(): number {
    return this._parcelGroup.length;
  }

  addParcelToGroup(parcel: ParcelDataset): void {
    this._parcelGroup.push(parcel);
  }
}
