import {
  ProjectRiskTypes,
  ProjectStatusTypes,
  ProjectTierTypes,
  ProjectTypes,
} from 'constants/index';

/**
 * A project represents a way to group related activities performed on property.
 */
export interface IProject {
  id?: number;
  projectTypeId: ProjectTypes;
  projectType?: string;
  statusId: ProjectStatusTypes;
  status?: string;
  riskId: ProjectRiskTypes;
  risk?: string;
  tierId: ProjectTierTypes;
  tier?: string;
  rowVersion?: number;
}
