import { NoteTypes } from 'constants/index';
import { Api_Note } from 'models/api/Note';
import { useEffect, useState } from 'react';

import { NoteDetailsFormModal } from './detail/NoteDetailsFormModal';
import { useNoteRepository } from './hooks/useNoteRepository';
import { UpdateNoteContainer } from './update/UpdateNoteContainer';

export interface INotesDetailContainerProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  type: NoteTypes;
  /** The note's ID */
  noteId: number;
  /** Initial edit mode value */
  editMode?: boolean;
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** set the value of the externally tracked 'isOpened' prop above. */
  openModal: () => void;
  /** set the value of the externally tracked 'isOpened' prop above. */
  closeModal: () => void;
  /** Edit note callback */
  onEdit?: (note?: Api_Note) => void;
}

export const NoteContainer: React.FC<INotesDetailContainerProps> = props => {
  const { getNote } = useNoteRepository();
  const [isEditMode, setEditMode] = useState<boolean>(!!props.editMode);

  useEffect(() => {
    getNote.execute(props.type, props.noteId);
  }, [getNote, props.noteId, props.type]);

  if (isEditMode) {
    return (
      <UpdateNoteContainer
        type={props.type}
        isOpened={props.isOpened}
        openModal={props.openModal}
        closeModal={props.closeModal}
        loading={getNote.loading}
        note={getNote.response}
      ></UpdateNoteContainer>
    );
  } else {
    return (
      <NoteDetailsFormModal
        loading={getNote.loading}
        note={getNote.response}
        isOpened={props.isOpened}
        openModal={props.openModal}
        closeModal={props.closeModal}
        onEdit={() => setEditMode(true)}
      ></NoteDetailsFormModal>
    );
  }
};
