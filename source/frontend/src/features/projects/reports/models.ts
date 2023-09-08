import { CodeTypeSelectOption } from '@/components/maps/leaflet/Control/AdvancedFilter/models';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';

export class ExportProjectModel {
  public exportType: keyof typeof ProjectExportTypes | '' = '';
  public projects: CodeTypeSelectOption[] = [];
  public acquisitionTeam: CodeTypeSelectOption[] = [];

  public toApi(): Api_ExportProjectFilter {
    return {
      type: this.exportType === '' ? undefined : this.exportType,
      projects: this.projects.map(p => +p.codeType),
      acquisitionTeamPersons: this.acquisitionTeam.map(t => +t.codeType),
    };
  }
}

export enum ProjectExportTypes {
  COMPENSATION = 'Compensation Requisition Export',
  AGREEMENT = 'Agreement Export',
}
