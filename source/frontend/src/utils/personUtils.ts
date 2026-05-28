import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';

import { exists } from './utils';

export type IdSelector = 'O' | 'P';

export function formatApiPersonNames(person: ApiGen_Concepts_Person | null | undefined): string {
  return formatNames([person?.firstName, person?.middleNames, person?.surname]);
}

export function formatNames(nameParts: Array<string | undefined | null>): string {
  return nameParts.filter(n => exists(n) && n.trim() !== '').join(' ');
}

export const getParameterIdFromOptions = (
  options: MultiSelectOption[],
  selector: IdSelector = 'P',
): string => {
  if (options.length === 0) {
    return '';
  }

  const filterOrgItems = options.filter(option => String(option.id).startsWith(selector));
  if (filterOrgItems.length === 0) {
    return '';
  }

  return filterOrgItems[0].id.split('-').pop() ?? '';
};
