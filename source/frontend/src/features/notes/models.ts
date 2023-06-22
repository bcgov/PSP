import { Api_Note } from '@/models/api/Note';

export class NoteForm {
  id?: number;
  note?: string = '';
  rowVersion?: number;
  appCreateTimestamp?: string;
  appLastUpdateTimestamp?: string;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
  appLastUpdateUserGuid?: string;
  appCreateUserGuid?: string;

  static fromApi(base: Api_Note): NoteForm {
    var model = new NoteForm();
    model.id = base.id;
    model.note = base.note;
    model.rowVersion = base.rowVersion;
    model.appCreateTimestamp = base.appCreateTimestamp;
    model.appCreateUserGuid = base.appCreateUserGuid;
    model.appCreateUserid = base.appCreateUserid;
    model.appLastUpdateTimestamp = base.appLastUpdateTimestamp;
    model.appLastUpdateUserGuid = base.appLastUpdateUserGuid;
    model.appLastUpdateUserid = base.appLastUpdateUserid;
    return model;
  }

  toApi(): Api_Note {
    return {
      id: this.id,
      note: this.note,
      rowVersion: this.rowVersion,
      appCreateTimestamp: this.appCreateTimestamp,
      appCreateUserGuid: this.appCreateUserGuid,
      appCreateUserid: this.appCreateUserid,
      appLastUpdateTimestamp: this.appLastUpdateTimestamp,
      appLastUpdateUserGuid: this.appLastUpdateUserGuid,
      appLastUpdateUserid: this.appLastUpdateUserid,
    };
  }
}
