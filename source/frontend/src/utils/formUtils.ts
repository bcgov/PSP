import { SelectOption } from '@/components/common/form';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_CodeType } from '@/models/api/generated/ApiGen_Concepts_CodeType';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';

import { exists, isValidId, isValidString } from './utils';

/**
 * append the passed name and index to the existing namespace, ideal for nesting forms within formik.
 * @param nameSpace the namespace of the current formik form.
 * @param name the name to append to the namespace, may either be a field name or an object within the form (if passing the namespace to a subform).
 * @param index optional index to append to the namespace and name, used for formik arrays.
 */
// eslint-disable-next-line @typescript-eslint/ban-types
export const withNameSpace: Function = (nameSpace?: string, name?: string, index?: number) => {
  return [nameSpace ?? '', `${index ?? ''}`, name].filter(x => x).join('.');
};

/**
 * The phoneFormatter is used to format the specified phone number value
 * @param {string} phoneNumber This is the target phone number to be formatted
 */
export const phoneFormatter = (phoneNumber: string | null | undefined) => {
  if (isValidString(phoneNumber)) {
    let result = phoneNumber;
    const regex =
      phoneNumber.length === 10
        ? /(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d\d)/
        : /\d(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d\d)/;
    const format = phoneNumber.match(regex);
    if (format !== null && format.length === 4) {
      result = `1 ${format[1]}-${format[2]}-${format[3]}`;
    }
    return result;
  }
  return '';
};

export function emptyStringtoNullable(value: string | null): string | null {
  if (typeof value === 'string' && value === '') {
    return null;
  }
  return value;
}

export function stringToUndefined<T extends string | number>(
  value: T | null | undefined,
): T | undefined {
  return emptyStringToUndefined(value, value);
}

export function emptyStringToUndefined<T extends string | number>(
  value: T | null | undefined,
  originalValue: T | null | undefined,
) {
  if (typeof originalValue === 'string' && originalValue === '') {
    return undefined;
  }
  return exists(value) ? value : undefined;
}

export function stringToNull<T extends string>(value: T | null | undefined): T | null {
  return emptyStringToNull(value);
}

export function stringToNumber<T extends string | number>(value: T | null | undefined): number {
  if (typeof value === 'string' && value === '') {
    value = emptyStringToNull(value);
  }

  return exists(value) ? Number(value) : 0;
}

export function stringToNumberOrNull<T extends string | number>(
  value: T | null | undefined,
): number | null {
  if (typeof value === 'string' && value === '') {
    value = emptyStringToNull(value);
  }

  return exists(value) ? Number(value) : null;
}

export function emptyStringToNull<T extends string | number>(
  value: T | null | undefined,
): T | null {
  if (typeof value === 'string' && value === '') {
    return null;
  }
  return exists(value) ? value : null;
}

export function fromTypeCode<T = string>(
  value: ApiGen_Base_CodeType<T> | null | undefined,
): T | null {
  return value?.id ?? null;
}

export function fromTypeCodeNullable<T = string>(value?: ApiGen_Base_CodeType<T> | null): T | null {
  return value?.id ?? null;
}

export function toTypeCodeNullable<T = string>(
  value: T | null | undefined,
): ApiGen_Base_CodeType<T> | null {
  if (typeof value === 'string') {
    return isValidString(value) ? toTypeCode(value) : null;
  }
  if (typeof value === 'number') {
    return isValidId(value) ? toTypeCode(value) : null;
  }
  return exists(value) ? toTypeCode(value) : null;
}

export function toTypeCode<T = string>(value: T): ApiGen_Base_CodeType<T> {
  return { id: value, description: null, displayOrder: null, isDisabled: false };
}

export function toFinancialCode(
  id: number,
  code: ApiGen_Concepts_FinancialCodeTypes,
): ApiGen_Concepts_FinancialCode {
  return {
    id: id,
    type: code,
    code: null,
    effectiveDate: EpochIsoDateTime,
    expiryDate: null,
    description: null,
    displayOrder: null,
    ...getEmptyBaseAudit(),
  };
}

export function toTypeCodeConcept(
  value: number | null | undefined,
): ApiGen_Concepts_CodeType | null {
  return value ? { id: value, code: null, description: null, displayOrder: null } : null;
}

export function stringToBoolean(value: string | boolean): boolean {
  if (typeof value === 'string') {
    return value === 'true';
  }
  return value;
}

export function booleanToString(value?: boolean | null): string {
  if (!exists(value)) {
    return 'false';
  }
  return value.toString();
}

export function stringToNullableBoolean(value: string): boolean | null {
  if (value === 'null') {
    return null;
  }
  return value === 'true';
}

export function stringToBooleanOrNull(value: string): boolean | null {
  if (value === '') {
    return null;
  }
  return value === 'true';
}

export function nullableBooleanToString(value?: boolean | null): string {
  if (!exists(value)) {
    return 'null';
  }
  return value.toString();
}

export function yesNoUnknownToBoolean(value?: string): boolean | null {
  if (value?.toLowerCase() === 'yes') return true;
  else if (value?.toLowerCase() === 'no') return false;
  return null;
}

export function booleanToYesNoUnknownString(value?: boolean | null): string {
  if (value === true) return 'Yes';
  else if (value === false) return 'No';
  return 'Unknown';
}

export const yesNoUnknownOptions: SelectOption[] = [
  { label: 'Unknown', value: '' },
  { label: 'Yes', value: 'Yes' },
  { label: 'No', value: 'No' },
];

export function numberFieldToRequiredNumber(value: NumberFieldValue) {
  if (value === '') {
    throw new Error('Number field is required, cannot be empty');
  }
  return Number(value);
}

export function toRequiredTypeCode<T>(value?: ApiGen_Base_CodeType<T>): ApiGen_Base_CodeType<T> {
  if (!value) {
    throw new Error('TypeCode is required, cannot be empty');
  }
  return value;
}

/**
 * This functions returns the numeric value of a currency formated number.
 * i.e.  ( $1,000.00 ) => 1000.00
 * @param {string} stringValue This is the target phone number to be formatted
 */
export const getCurrencyCleanValue = (stringValue: string): number => {
  return Number(stringValue.replace(/[^0-9.-]/g, ''));
};

export function formatMinistryProject(projectNumber?: string | null, projectName?: string | null) {
  const formattedValue = [projectNumber, projectName].filter(x => x).join(' - ');
  return formattedValue;
}
