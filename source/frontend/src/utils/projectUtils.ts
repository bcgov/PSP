import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';

import { exists, isNullOrWhitespace } from './utils';

export function formatApiProjectName(project: ApiGen_Concepts_Project | null | undefined): string {
  if (!exists(project)) {
    return '';
  }
  return [project.code, project.description].filter(n => !isNullOrWhitespace(n)).join(' ');
}
