import { ProjectExportTypes } from '@/features/projects/reports/models';

export interface Api_ExportProjectFilter {
  projects: number[];
  acquisitionTeamPersons: number[];
  type?: keyof typeof ProjectExportTypes;
}
