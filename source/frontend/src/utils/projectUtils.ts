import { Api_Project } from 'models/api/Project';

import { isNullOrWhitespace } from './utils';

export function formatApiProjectName(project?: Api_Project): string {
  if (project === undefined) {
    return '';
  }
  return [project.code, project.description].filter(n => !isNullOrWhitespace(n)).join(' ');
}
