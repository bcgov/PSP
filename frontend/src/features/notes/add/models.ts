import { Api_EntityNote, Api_Note } from 'models/api/Note';

export class NoteForm {
  id?: number;
  note?: string = '';
  rowVersion?: number;

  toApi(): Api_Note {
    return {
      id: this.id,
      note: this.note,
      rowVersion: this.rowVersion,
    };
  }
}

export class EntityNoteForm {
  id?: number;
  note: NoteForm = new NoteForm();
  parentId: number = 0;
  rowVersion?: number;

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
