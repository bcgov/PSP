import { IAutocompletePrediction } from '@/interfaces';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

export interface CodeTypeSelectOption {
  codeType: string;
  codeTypeDescription: string;
}

export class PropertyFilterFormModel {
  public projectPrediction: IAutocompletePrediction | null = null;

  public leaseStatus: string = '';
  public leaseTypes: CodeTypeSelectOption[] = [];
  public leasePurposes: CodeTypeSelectOption[] = [];

  public anomalies: CodeTypeSelectOption[] = [];

  public toApi(): Api_PropertyFilterCriteria {
    return {
      projectId: this.projectPrediction !== null ? this.projectPrediction.id : null,
      leaseStatus: this.leaseStatus !== '' ? this.leaseStatus : null,
      leaseTypes: this.leaseTypes.map(lt => lt.codeType),
      leasePurposes: this.leasePurposes.map(lp => lp.codeType),
      anomalyIds: this.anomalies.map(a => a.codeType),
    };
  }
}
