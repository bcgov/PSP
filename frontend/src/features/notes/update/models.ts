import { Api_EntityNote, Api_Note } from 'models/api/Note';

export class UpdateNoteForm {
  id?: number;
  note?: string = '';
  rowVersion?: number;

  static fromApi(base: Api_Note): UpdateNoteForm {
    var model = new UpdateNoteForm();
    model.id = base.id;
    model.note = base.note;
    model.rowVersion = base.rowVersion;
    return model;
  }

  toApi(): Api_Note {
    return {
      id: this.id,
      note: this.note,
      rowVersion: this.rowVersion,
    };
  }
}

export class UpdateEntityNoteForm {
  id?: number;
  note: UpdateNoteForm = new UpdateNoteForm();
  parentId: number = 0;
  rowVersion?: number;

  static fromApi(base: Api_EntityNote): UpdateEntityNoteForm {
    var model = new UpdateEntityNoteForm();
    model.id = base.id;
    model.parentId = base.parent?.id;
    model.rowVersion = base.rowVersion;
    model.note = UpdateNoteForm.fromApi(base.note);
    return model;
  }

  toApi(): Api_EntityNote {
    return {
      id: this.id,
      note: this.note.toApi(),
      parent: {
        id: this.parentId,
      },
      rowVersion: this.rowVersion,
    };
  }
}
