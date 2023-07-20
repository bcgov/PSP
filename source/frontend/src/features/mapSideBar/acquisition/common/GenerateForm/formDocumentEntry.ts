import { FormDocumentType } from '@/constants/formDocumentTypes';

interface FormDocumentEntry {
  formType: FormDocumentType;
  text: string;
}
export const generateDocumentEntries: FormDocumentEntry[] = [
  { formType: FormDocumentType.LETTER, text: 'Generate Letter' },
  { formType: FormDocumentType.H0443, text: 'Conditions of Entry (H0443)' },
];
