import { ApiGen_CodeTypes_DocumentQueueStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_DocumentQueueStatusTypes';
import { isValidString } from '@/utils/utils';

/**
 * This functions returns a boolean that identifies if the Mayan Processed an error.
 * @param {string} queueCode The ApiGen_CodeTypes_DocumentQueueStatusTypes Code.
 */
export const documentQueueInError = (queueCode: string | null | undefined): boolean => {
  if (queueCode === null || queueCode === undefined) {
    return false;
  }

  if (isValidString(queueCode)) {
    switch (queueCode) {
      case ApiGen_CodeTypes_DocumentQueueStatusTypes.PIMS_ERROR:
      case ApiGen_CodeTypes_DocumentQueueStatusTypes.MAYAN_ERROR:
        return true;
      default:
        return false;
    }
  }
  throw new Error('Invalid parameter QueueCode');
};

/**
 * This functions returns a boolean that identifies if the Mayan is processing the document.
 * @param {string} queueCode The ApiGen_CodeTypes_DocumentQueueStatusTypes Code.
 */
export const documentQueueInProcess = (queueCode: string | null | undefined): boolean => {
  if (queueCode === null || queueCode === undefined) {
    return false;
  }

  if (isValidString(queueCode)) {
    switch (queueCode) {
      case ApiGen_CodeTypes_DocumentQueueStatusTypes.PENDING:
      case ApiGen_CodeTypes_DocumentQueueStatusTypes.PROCESSING:
        return true;
      default:
        return false;
    }
  }
  throw new Error('Invalid parameter QueueCode');
};
