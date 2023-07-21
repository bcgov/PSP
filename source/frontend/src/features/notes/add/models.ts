import { Api_EntityNote } from '@/models/api/Note';

import { NoteForm } from '../models';

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
