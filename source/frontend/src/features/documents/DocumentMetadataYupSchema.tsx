import * as Yup from 'yup';

import ValidDocumentExtensions from '@/constants/ValidDocumentExtensions';
import { exists } from '@/utils';

export const getDocumentMetadataYupSchema = (maxDocumentCount: number) => {
  return Yup.object().shape({
    documents: Yup.array()
      .min(1, 'There needs to be at least one document')
      .max(maxDocumentCount, 'Max number of uploads reached')
      .of(
        Yup.object().shape({
          documentTypeId: Yup.string().required('Document type is required'),
          file: Yup.mixed<File>().test('fileExtension', 'File type not supported!', value => {
            // If no file is uploaded, let 'required' handle it (or return true if optional)
            if (!exists(value)) {
              return true;
            }

            const fileName = value.name;
            if (typeof fileName !== 'string') return false;

            const fileExtension = fileName.split('.').pop().toLowerCase();
            return ValidDocumentExtensions.includes(fileExtension);
          }),
        }),
      ),
  });
};
