import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { EpochIsoDateTime, UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export class NoteForm {
  id?: number;
  note?: string = '';
  isSystemGenerated = false;
  rowVersion?: number;
  appCreateTimestamp?: UtcIsoDateTime;
  appLastUpdateTimestamp?: UtcIsoDateTime;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
  appLastUpdateUserGuid?: string;
  appCreateUserGuid?: string;

  static fromApi(base: ApiGen_Concepts_Note): NoteForm {
    const model = new NoteForm();
    model.id = base.id;
    model.note = base.note ?? undefined;
    model.isSystemGenerated = base.isSystemGenerated;
    model.rowVersion = base.rowVersion ?? undefined;
    model.appCreateTimestamp = base.appCreateTimestamp;
    model.appCreateUserGuid = base.appCreateUserGuid ?? undefined;
    model.appCreateUserid = base.appCreateUserid ?? undefined;
    model.appLastUpdateTimestamp = base.appLastUpdateTimestamp;
    model.appLastUpdateUserGuid = base.appLastUpdateUserGuid ?? undefined;
    model.appLastUpdateUserid = base.appLastUpdateUserid ?? undefined;
    return model;
  }

  toApi(): ApiGen_Concepts_Note {
    return {
      id: this.id ?? 0,
      note: this.note ?? null,
      isSystemGenerated: this.isSystemGenerated,
      ...getEmptyBaseAudit(this.rowVersion),
      appCreateTimestamp: this.appCreateTimestamp ?? EpochIsoDateTime,
      appCreateUserGuid: this.appCreateUserGuid ?? null,
      appCreateUserid: this.appCreateUserid ?? null,
      appLastUpdateTimestamp: this.appLastUpdateTimestamp ?? EpochIsoDateTime,
      appLastUpdateUserGuid: this.appLastUpdateUserGuid ?? null,
      appLastUpdateUserid: this.appLastUpdateUserid ?? null,
    };
  }
}
