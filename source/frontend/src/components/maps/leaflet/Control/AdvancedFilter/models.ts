import { IAutocompletePrediction } from '@/interfaces';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

export class PropertyFilterFormModel {
  public projectPrediction: IAutocompletePrediction | null = null;

  public toApi(): Api_PropertyFilterCriteria {
    return {
      projectId: this.projectPrediction !== null ? this.projectPrediction.id : null,
    };
  }
}
