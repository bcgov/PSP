import { ApiGen_Concepts_EntityNote } from '@/models/api/generated/ApiGen_Concepts_EntityNote';
import { getEmptyBaseAudit } from '@/models/default_initializers';

import { NoteForm } from '../models';

export class EntityNoteForm {
  id?: number;
  note: NoteForm = new NoteForm();
  parentId: number = 0;
  rowVersion?: number;

  toApi(): ApiGen_Concepts_EntityNote {
    return {
      id: this.id ?? 0,
      note: this.note.toApi(),
      parent: {
        id: this.parentId,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
