import { NoteTypes } from 'constants/index';
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
  /** Success callback */
  onSuccess?: () => void;
}

export const NoteContainer: React.FC<INotesDetailContainerProps> = props => {
  const [isEditMode, setEditMode] = useState<boolean>(!!props.editMode);
  const {
    getNote: { execute, loading, response },
  } = useNoteRepository();

  useEffect(() => {
    execute(props.type, props.noteId);
  }, [execute, props.noteId, props.type]);

  // re-fetch note from API after update
  const onSuccess = () => {
    execute(props.type, props.noteId);
    props.onSuccess && props.onSuccess();
  };

  const close = () => {
    props.closeModal();
    setEditMode(false);
  };

  if (isEditMode) {
    return (
      <UpdateNoteContainer
        type={props.type}
        isOpened={props.isOpened}
        loading={loading}
        note={response}
        onSuccess={onSuccess}
        onSaveClick={close}
        onCancelClick={close}
      ></UpdateNoteContainer>
    );
  } else {
    return (
      <NoteDetailsFormModal
        loading={loading}
        note={response}
        isOpened={props.isOpened}
        onCloseClick={close}
        onEdit={() => setEditMode(true)}
      ></NoteDetailsFormModal>
    );
  }
};
