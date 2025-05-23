/**
 * File autogenerated by TsGenerator.
 * Do not manually modify, changes made to this file will be lost when this file is regenerated.
 */
import { UtcIsoDate } from '@/models/api/UtcIsoDateTime';

import { ApiGen_Base_BaseConcurrent } from './ApiGen_Base_BaseConcurrent';
import { ApiGen_Base_CodeType } from './ApiGen_Base_CodeType';

// LINK: @backend/apimodels/Models/Concepts/DispositionFile/DispositionFileOfferModel.cs
export interface ApiGen_Concepts_DispositionFileOffer extends ApiGen_Base_BaseConcurrent {
  id: number | null;
  dispositionFileId: number;
  dispositionOfferStatusTypeCode: string | null;
  dispositionOfferStatusType: ApiGen_Base_CodeType<string> | null;
  offerName: string | null;
  offerDate: UtcIsoDate;
  offerExpiryDate: UtcIsoDate | null;
  offerAmount: number;
  offerNote: string | null;
}
