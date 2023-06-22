import { useEffect, useState } from 'react';

import { NoteTypes } from '@/constants/index';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';

import { NoteDetailsFormModal } from './detail/NoteDetailsFormModal';
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

export const NoteContainer: React.FC<
  React.PropsWithChildren<INotesDetailContainerProps>
> = props => {
  const { type, noteId, editMode, isOpened, onSuccess, closeModal } = props;
  const [isEditMode, setEditMode] = useState<boolean>(!!editMode);
  const {
    getNote: { execute, loading, response },
  } = useNoteRepository();

  useEffect(() => {
    execute(noteId);
  }, [execute, noteId, type]);

  // re-fetch note from API after update
  const onSuccessHandler = () => {
    execute(noteId);
    onSuccess && onSuccess();
  };

  const close = () => {
    closeModal();
    setEditMode(false);
  };

  if (isEditMode) {
    return (
      <UpdateNoteContainer
        type={type}
        isOpened={isOpened}
        loading={loading}
        note={response}
        onSuccess={onSuccessHandler}
        onSaveClick={close}
        onCancelClick={close}
      ></UpdateNoteContainer>
    );
  } else {
    return (
      <NoteDetailsFormModal
        loading={loading}
        note={response}
        isOpened={isOpened}
        onCloseClick={close}
        onEdit={() => setEditMode(true)}
      ></NoteDetailsFormModal>
    );
  }
};
