import { ApiGen_Concepts_PropertyActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_PropertyActivityInvoice';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const getMockPropertyActivityInvoice = (
  id = 1,
  propertyActivityId = 1,
): ApiGen_Concepts_PropertyActivityInvoice => ({
  id,
  propertyActivityId,
  propertyActivity: null,
  invoiceDateTime: '2024-10-09',
  invoiceNum: 'INV-0001',
  description: 'Extra description goes here',
  pretaxAmount: 0.0,
  gstAmount: 0.0,
  pstAmount: 0.0,
  totalAmount: 0.0,
  isPstRequired: false,
  isDisabled: false,
  ...getEmptyBaseAudit(0),
});
