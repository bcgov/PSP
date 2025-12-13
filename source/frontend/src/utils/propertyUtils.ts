import { FormikProps, getIn } from 'formik';
import { Feature, Geometry } from 'geojson';
import { toast } from 'react-toastify';

import { ModalContent } from '@/components/common/GenericModal';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { UploadResponseModel } from '@/features/properties/shapeUpload/models';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { arePropertyFormsEqual, firstOrNull } from '@/utils';

import { exists, isNumber, isValidString } from './utils';

/**
 * The pidFormatter is used to format the specified PID value
 * @param {string} pid This is the target PID to be formatted
 */
export const pidFormatter = (pid?: string) => {
  if (isValidString(pid)) {
    let result = pid.toString().padStart(9, '0');
    const regex = /(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d)/;
    const format = result.match(regex);
    if (format !== null && format.length === 4) {
      result = `${format[1]}-${format[2]}-${format[3]}`;
    }
    return result;
  }
  return '';
};

/**
 * The pidParser is used to return a numeric pid value from a formatted pid.
 * @param {string} pid This is the target PID to be parsed
 */
export const pidParser = (pid?: string | number | null): number | undefined => {
  if (typeof pid === 'number') {
    return pid;
  }
  if (isValidString(pid)) {
    const regex = /(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d)/;
    const format = pid.match(regex);
    if (format !== null && format.length === 4) {
      return parseInt(`${format[1]}${format[2]}${format[3]}`);
    } else {
      return parseInt(pid);
    }
  }
  return undefined;
};

/**
 * The pinParser is used to return a numeric pin value from a formatted pin.
 * @param {string} pin This is the target PIN to be parsed
 */
export const pinParser = (pin?: string | number | null): number | undefined => {
  if (isNumber(pin)) {
    return pin;
  }
  if (isValidString(pin)) {
    return parseInt(pin);
  }
  return undefined;
};

/**
 * Provides a formatted address as a string.
 * @param address Address object from property.
 * @returns Civic address string value.
 */
export const formatApiAddress = (address: ApiGen_Concepts_Address | null | undefined) => {
  return formatSplitAddress(
    address?.streetAddress1 ?? '',
    address?.streetAddress2 ?? '',
    address?.streetAddress3 ?? '',
    address?.municipality ?? '',
    address?.province?.code ?? '',
    address?.postal ?? '',
  );
};

/**
 * Provides a formatted address as a string.
 * @returns Civic address string value.
 */
export const formatSplitAddress = (
  streetAddress1: string,
  streetAddress2: string,
  streetAddress3: string,
  municipality: string,
  provinceCode: string,
  postal: string,
) => {
  const values = [streetAddress1, streetAddress2, streetAddress3, municipality, provinceCode];
  return values.filter(text => text !== '').join(' ') + (postal ? ', ' + (postal ?? '') : '');
};

/**
 * Provides a formatted street address as a string.
 * Combines data from a BC assessment address into a formatted address string.
 *
 * @param address Address object from bc assessment.
 * @returns Civic address string value.
 */
export const formatBcaAddress = (address?: IBcAssessmentSummary['ADDRESSES'][0]) =>
  [
    address?.UNIT_NUMBER,
    address?.STREET_NUMBER,
    address?.STREET_DIRECTION_PREFIX,
    address?.STREET_NAME,
    address?.STREET_TYPE,
    address?.STREET_DIRECTION_SUFFIX,
  ]
    .filter(a => !!a)
    .join(' ');

export function formatApiPropertyManagementLease(
  base: ApiGen_Concepts_PropertyManagement | undefined | null,
): string {
  if (base?.hasActiveLease && base.activeLeaseHasExpiryDate) {
    return 'Yes';
  } else if (base?.hasActiveLease && !base.activeLeaseHasExpiryDate) {
    return 'Yes (No Expiry Date)';
  } else {
    return 'No';
  }
}

export function isPlanNumberSPCP(planNumber: string): boolean {
  if (!exists(planNumber)) {
    return false;
  }

  const nonNumericPrefix = firstOrNull(planNumber?.match(/^\D+/)); // Extract non-numeric prefix
  const isStrataCommonPropertyPrefix = nonNumericPrefix?.toUpperCase()?.endsWith('S'); // Check if the last character is 'S' PSP-10455

  return isStrataCommonPropertyPrefix;
}

export function isStrataCommonProperty(
  feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined | null,
) {
  if (!exists(feature)) {
    return false;
  }

  const planNumber = feature.properties.PLAN_NUMBER;

  return (
    isPlanNumberSPCP(planNumber) &&
    feature.properties.PID === null &&
    feature.properties.PIN === null &&
    feature.properties.OWNER_TYPE === 'Unclassified'
  );
}

export const addPropertiesToCurrentFile = <T extends { [key: string]: any }>(
  formikRef: React.RefObject<FormikProps<T>>,
  fieldName: keyof T,
  propertyForms: PropertyForm[],
  notifyAddComplete: () => void,
) => {
  const existingProperties = getIn(formikRef?.current?.values, fieldName as string) ?? [];
  const uniqueProperties = propertyForms.filter(newProperty => {
    return !existingProperties.some((existingProperty: PropertyForm) =>
      arePropertyFormsEqual(existingProperty, newProperty),
    );
  });

  const duplicatesSkipped = propertyForms.length - uniqueProperties.length;

  // If there are unique properties, add them to the formik values
  if (uniqueProperties.length > 0) {
    formikRef.current?.setFieldValue(fieldName as string, [
      ...existingProperties,
      ...uniqueProperties,
    ]);
    formikRef.current?.setFieldTouched(fieldName as string, true);
    toast.success(`Added ${uniqueProperties.length} new property(s) to the file.`);
  }

  if (duplicatesSkipped > 0) {
    toast.warn(`Skipped ${duplicatesSkipped} duplicate property(s).`);
  }
  notifyAddComplete();
};

/**
 * Adds the uploaded shape boundary to the property form.
 * @param property The property form to update.
 * @param uploadResponse The upload response containing the new boundary.
 * @returns The updated property form with the new boundary.
 */
export const addShapeToProperty = (
  property: PropertyForm,
  uploadResponse: UploadResponseModel,
): PropertyForm => {
  // Update the property boundary based on the uploaded shapefile
  if (exists(uploadResponse) && uploadResponse.isSuccess && exists(uploadResponse.boundary)) {
    const updatedFormProperty = new PropertyForm(property);
    updatedFormProperty.fileBoundary = uploadResponse.boundary;
    return updatedFormProperty;
  } else {
    // return the original property if no update is made
    return property;
  }
};

/**
 * Removes the shape boundary from the property form.
 * @param property The property form to update.
 * @returns The updated property form with the boundary removed.
 */
export const removeShapeFromProperty = (property: PropertyForm): PropertyForm => {
  const updatedFormProperty = new PropertyForm(property);
  updatedFormProperty.fileBoundary = null;
  return updatedFormProperty;
};

/**
 * Prompts the user for confirmation before removing the shape boundary from the property form.
 * @param property The property form to update.
 * @param setModalContent Function to set the modal content.
 * @param setDisplayModal Function to control the display of the modal.
 * @param onConfirm Callback function to execute upon confirmation of shape removal.
 */
export const removeShapeFromPropertyWithConfirmation = (
  property: PropertyForm,
  setModalContent: (modalProps?: ModalContent) => void,
  setDisplayModal: (display: boolean) => void,
  onConfirm: (updatedProperty: PropertyForm) => void,
): void => {
  setModalContent({
    variant: 'info',
    title: 'Confirm shape removal',
    message: 'Are you sure you want to remove this uploaded shape?',
    handleOk: () => {
      const updatedFormProperty = removeShapeFromProperty(property);
      onConfirm(updatedFormProperty);
      setDisplayModal(false);
    },
    handleCancel: () => setDisplayModal(false),
    okButtonText: 'Remove',
    cancelButtonText: 'Cancel',
  });

  setDisplayModal(true);
};
