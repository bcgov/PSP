import { Api_Note } from 'models/api/Note';

export class NoteForm {
  id?: number;
  parentId: number = 0;
  note: string = '';
  rowVersion?: number;

  toApi(): Api_Note {
    return {
      id: this.id,
      note: this.note,
      rowVersion: this.rowVersion,
    };
  }
}
