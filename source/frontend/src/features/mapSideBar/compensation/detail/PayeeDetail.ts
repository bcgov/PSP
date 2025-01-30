import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { isValidString } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { DetailAcquisitionFileOwner } from '../../acquisition/models/DetailAcquisitionFileOwner';

export class PayeeDetail {
  compReqPayeeId: number;
  displayName: string;
  isPaymentInTrust: boolean;
  contactEnabled: boolean;
  contactString: string | null;

  constructor() {
    this.compReqPayeeId = 0;
    this.displayName = '';
    this.isPaymentInTrust = false;
    this.contactEnabled = false;
    this.contactString = null;
  }

  static createFromOwner = (owner: ApiGen_Concepts_AcquisitionFileOwner): PayeeDetail => {
    const payeeDetail = new PayeeDetail();
    const ownerDetail = DetailAcquisitionFileOwner.fromApi(owner);
    payeeDetail.displayName = ownerDetail?.ownerName;
    return payeeDetail;
  };

  static createFromPerson = (person: ApiGen_Concepts_Person): PayeeDetail => {
    const payeeDetail = new PayeeDetail();

    payeeDetail.displayName = formatApiPersonNames(person);
    payeeDetail.contactString = 'P' + person?.id;
    payeeDetail.contactEnabled = true;
    return payeeDetail;
  };

  static createFromOrganization = (organization: ApiGen_Concepts_Organization): PayeeDetail => {
    const payeeDetail = new PayeeDetail();

    payeeDetail.displayName = organization?.name ?? '';
    payeeDetail.contactString = 'O' + organization?.id;
    payeeDetail.contactEnabled = true;
    return payeeDetail;
  };

  static createFromLegacyPayee = (legacyPayee: string | null): PayeeDetail => {
    const payeeDetail = new PayeeDetail();
    payeeDetail.displayName = isValidString(legacyPayee) ? legacyPayee : '';
    return payeeDetail;
  };
}
