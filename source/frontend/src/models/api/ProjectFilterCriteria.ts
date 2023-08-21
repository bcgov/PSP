export interface Api_PropertyFilterCriteria {
  projectId: number | null;

  leaseStatus: string | null;
  leaseTypes: string[];
  leasePurposes: string[];

  anomalyIds: string[];
}
