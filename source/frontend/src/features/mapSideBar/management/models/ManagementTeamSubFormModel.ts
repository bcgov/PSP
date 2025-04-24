import { isNumber } from 'lodash';

import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { fromTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import { exists, isValidId } from '@/utils/utils';

export interface WithManagementTeam {
  team: ManagementTeamSubFormModel[];
}

export class ManagementTeamSubFormModel {
  contact: IContactSearchResult | null = null;
  teamProfileTypeCode = '';
  primaryContactId = '';

  constructor(
    readonly id: number | null = null,
    readonly rowVersion: number | null = null,
    contact: IContactSearchResult | null = null,
  ) {
    this.id = id;
    this.rowVersion = rowVersion;
    this.contact = contact;
  }

  toApi(managementFileId: number): ApiGen_Concepts_ManagementFileTeam | null {
    const personId = this.contact?.personId ?? null;
    const organizationId = !personId ? this.contact?.organizationId ?? null : null;
    if (!isValidId(personId) && !isValidId(organizationId)) {
      return null;
    }

    return {
      id: this.id ?? 0,
      rowVersion: this.rowVersion ?? 0,
      managementFileId: managementFileId,
      personId: personId ?? null,
      person: null,
      organizationId: organizationId ?? null,
      organization: null,
      primaryContactId:
        !!this.primaryContactId && isNumber(+this.primaryContactId)
          ? Number(this.primaryContactId)
          : null,
      teamProfileType: toTypeCodeNullable(this.teamProfileTypeCode),
      teamProfileTypeCode: this.teamProfileTypeCode,
      primaryContact: null,
    };
  }

  static fromApi(model: ApiGen_Concepts_ManagementFileTeam | null): ManagementTeamSubFormModel {
    // todo:the method 'exists' here should allow the compiler to validate the child property. this works correctly in typescropt 5.3 +
    let contact: IContactSearchResult | undefined;
    if (exists(model?.person)) {
      contact = fromApiPerson(model.person);
    } else if (exists(model?.organization)) {
      contact = fromApiOrganization(model.organization);
    } else {
      contact = undefined;
    }

    const newForm = new ManagementTeamSubFormModel(model?.id ?? 0, model?.rowVersion, contact);
    newForm.teamProfileTypeCode = fromTypeCode(model?.teamProfileType) ?? '';

    if (model?.primaryContactId) {
      newForm.primaryContactId = model.primaryContactId.toString();
    }

    return newForm;
  }
}
