import { IAutocompletePrediction } from '@/interfaces';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

export interface CodeTypeSelectOption {
  codeType: string;
  codeTypeDescription: string;
}

export class PropertyFilterFormModel {
  // Project Filters
  public projectPrediction: IAutocompletePrediction | null = null;

  // Tenure Filters
  public tenureStatuses: CodeTypeSelectOption[] = [];
  public tenurePPH = '';
  public tenureRoadTypes: CodeTypeSelectOption[] = [];

  // Lease filters
  public leaseStatus = '';
  public leasePayRcvblType = '';
  public leaseTypes: CodeTypeSelectOption[] = [];
  public leasePurposes: CodeTypeSelectOption[] = [];

  // Anomaly filters
  public anomalies: CodeTypeSelectOption[] = [];

  public toApi(): Api_PropertyFilterCriteria {
    return {
      projectId: this.projectPrediction !== null ? this.projectPrediction.id : null,

      tenureStatuses: this.tenureStatuses.map(lp => lp.codeType),
      tenurePPH: this.tenurePPH !== '' ? this.tenurePPH : null,
      tenureRoadTypes: this.tenureRoadTypes.map(lp => lp.codeType),

      leaseStatus: this.leaseStatus !== '' ? this.leaseStatus : null,
      leasePayRcvblType: this.leasePayRcvblType !== '' ? this.leasePayRcvblType : null,
      leaseTypes: this.leaseTypes.map(lt => lt.codeType),
      leasePurposes: this.leasePurposes.map(lp => lp.codeType),

      anomalyIds: this.anomalies.map(a => a.codeType),
    };
  }
}
