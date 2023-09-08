export interface Api_PropertyFilterCriteria {
  projectId: number | null;

  tenureStatuses: string[];
  tenurePPH: string | null;
  tenureRoadTypes: string[];

  leaseStatus: string | null;
  leaseTypes: string[];
  leasePurposes: string[];

  anomalyIds: string[];
}
