import { FeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { LtsaOrders, SpcpOrder } from '@/interfaces/ltsaModels';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';

export interface ComposedProperty {
  pid: string | undefined;
  pin: string | undefined;
  planNumber: string | undefined;
  id: number | undefined;
  ltsaOrders: LtsaOrders | undefined;
  spcpOrder: SpcpOrder | undefined;
  pimsProperty: ApiGen_Concepts_Property | undefined;
  propertyAssociations: ApiGen_Concepts_PropertyAssociations | undefined;
  bcAssessmentSummary: IBcAssessmentSummary | undefined;
  featureDataset: FeatureDataset;
}
