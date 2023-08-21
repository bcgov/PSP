import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';
import { formatMoney } from '@/utils/numberFormatUtils';

import { Api_GenerateExpropriationPaymentItem } from '../GenerateExpropriationPaymentItem';
import { Api_GenerateOrganization } from '../GenerateOrganization';
import { Api_GenerateOwner } from '../GenerateOwner';

export class Api_GenerateExpropriationForm8 {
  payee: Api_GenerateOwner;
  exp_authority: Api_GenerateOrganization;
  payment_items: Api_GenerateExpropriationPaymentItem[];
  total_amount: string;

  constructor(form8: Api_ExpropriationPayment) {
    this.payee = getForm8Recipient(form8);
    this.exp_authority = new Api_GenerateOrganization(form8.expropriatingAuthority);
    this.payment_items =
      form8.paymentItems?.map(item => new Api_GenerateExpropriationPaymentItem(item)) ?? [];
    this.total_amount = formatMoney(
      form8.paymentItems?.map(f => f.totalAmount ?? 0).reduce((prev, next) => prev + next, 0) ?? 0,
    );
  }
}

const getForm8Recipient = (form8: Api_ExpropriationPayment): Api_GenerateOwner => {
  if (form8.acquisitionOwnerId && form8.acquisitionOwnerId) {
    return new Api_GenerateOwner(form8.acquisitionOwner);
  }

  if (form8.interestHolderId && form8.interestHolder) {
    const interesHolder = form8.interestHolder;
    if (interesHolder.personId && interesHolder.person) {
      return Api_GenerateOwner.fromApiPerson(interesHolder.person);
    } else if (interesHolder.organizationId && interesHolder.organization) {
      return Api_GenerateOwner.fromApiOrganization(interesHolder.organization);
    }
  }

  return new Api_GenerateOwner(null);
};
