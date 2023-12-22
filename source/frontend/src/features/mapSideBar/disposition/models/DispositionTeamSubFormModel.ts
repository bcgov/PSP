import { isNumber } from 'lodash';

import { IContactSearchResult } from '@/interfaces/IContactSearchResult';
import { Api_DispositionFileTeam } from '@/models/api/DispositionFile';
import { toTypeCode } from '@/utils/formUtils';

export interface WithDispositionTeam {
  team: DispositionTeamSubFormModel[];
}

export class DispositionTeamSubFormModel {
  contact: IContactSearchResult | null = null;
  teamProfileTypeCode: string = '';
  primaryContactId: string = '';

  constructor(
    readonly id: number | null = null,
    readonly rowVersion: number | null = null,
    contact: IContactSearchResult | null = null,
  ) {
    this.id = id;
    this.contact = contact;
    this.rowVersion = rowVersion;
  }

  toApi(dispositionFileId: number): Api_DispositionFileTeam | null {
    const personId = this.contact?.personId ?? null;
    const organizationId = !personId ? this.contact?.organizationId ?? null : null;
    if (personId === null && organizationId === null) {
      return null;
    }

    return {
      id: this.id ?? 0,
      rowVersion: this.rowVersion ?? 0,
      dispositionFileId: dispositionFileId,
      personId: personId ?? undefined,
      person: undefined,
      organizationId: organizationId ?? undefined,
      organization: undefined,
      primaryContactId:
        !!this.primaryContactId && isNumber(+this.primaryContactId)
          ? Number(this.primaryContactId)
          : undefined,
      teamProfileType: toTypeCode(this.teamProfileTypeCode),
      teamProfileTypeCode: this.teamProfileTypeCode,
    };
  }
}
