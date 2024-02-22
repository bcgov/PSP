import { CodeTypeSelectOption } from '@/components/maps/leaflet/Control/AdvancedFilter/models';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';

type IdSelector = 'O' | 'P';

export class ExportProjectModel {
  public exportType: keyof typeof ProjectExportTypes | '' = '';
  public projects: CodeTypeSelectOption[] = [];
  public acquisitionTeam: CodeTypeSelectOption[] = [];

  public toApi(): Api_ExportProjectFilter {
    return {
      type: this.exportType === '' ? undefined : this.exportType,
      projects: this.projects.map(p => +p.codeType),
      acquisitionTeamPersons: getParameterIdFromOptions(this.acquisitionTeam),
      acquisitionTeamOrganizations: getParameterIdFromOptions(this.acquisitionTeam, 'O'),
    };
  }
}

export enum ProjectExportTypes {
  COMPENSATION = 'Compensation Requisition Export',
  AGREEMENT = 'Agreement Export',
}

const getParameterIdFromOptions = (
  options: CodeTypeSelectOption[],
  selector: IdSelector = 'P',
): number[] => {
  if (options.length === 0) {
    return [];
  }

  const filteredItems = options.filter(option => String(option.codeType).startsWith(selector));
  if (filteredItems.length === 0) {
    return [];
  }

  return filteredItems.map(x => {
    const number = x.codeType.split('-').pop() ?? '';
    return parseInt(number) ?? 0;
  });
};
