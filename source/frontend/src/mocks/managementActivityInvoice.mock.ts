import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const getMockManagementActivityInvoice = (
  id = 1,
  managementActivityId = 1,
): ApiGen_Concepts_ManagementActivityInvoice => ({
  id,
  managementActivityId,
  managementActivity: null,
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
