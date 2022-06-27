import NoteListView from 'features/note/list/NoteListView';
import { NoteType } from 'models/api/Note';
import React from 'react';

export const TestNotes: React.FunctionComponent = () => {
  return (
    <div style={{ width: '100%' }}>
      <NoteListView type={NoteType.Activity} />
    </div>
  );
};
