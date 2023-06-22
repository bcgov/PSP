import { ConvertToTypes } from '@/constants/convertToTypes';

export interface DocumentGenerationRequest {
  templateData: object;
  templateType: string;
  convertToType: ConvertToTypes | null;
}
