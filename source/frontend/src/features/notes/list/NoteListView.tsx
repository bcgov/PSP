import React from 'react';
import { FaPlus } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableSort } from '@/components/Table/TableSort';
import { Claims } from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_EntityNote } from '@/models/api/generated/ApiGen_Concepts_EntityNote';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { exists, isValidId } from '@/utils';

import { AddNotesContainer } from '../add/AddNotesContainer';
import { NoteContainer } from '../NoteContainer';
import { createNoteActionsColumn, createNoteTableColumns } from './NoteResults/columns';
import { NoteResults } from './NoteResults/NoteResults';

export interface INoteListViewProps {
  type: NoteTypes;
  entityId: number;
  isAddNotesOpened?: boolean;
  isViewNotesOpened?: boolean;
  currentNote?: ApiGen_Concepts_Note;
  loading?: boolean;
  notes: ApiGen_Concepts_Note[];
  entityNotes?: ApiGen_Concepts_EntityNote[];
  sort: TableSort<ApiGen_Concepts_Note>;
  canEditNotes: boolean;
  openAddNotes?: () => void;
  closeAddNotes?: () => void;
  deleteNote?: (type: NoteTypes, noteId: number) => Promise<boolean>;
  onChildSuccess?: () => void;
  setSort?: (value: TableSort<ApiGen_Concepts_Note>) => void;
  setCurrentNote?: (note: ApiGen_Concepts_Note) => void;
  openViewNotes?: () => void;
  closeViewNotes?: () => void;
  getNoteNavigationUrlTitle?: (row: ApiGen_Concepts_Note) => {
    url: string;
    title: string;
  };
}

/**
 * Page that displays notes information.
 */
export const NoteListView: React.FunctionComponent<React.PropsWithChildren<INoteListViewProps>> = ({
  notes,
  loading,
  type,
  entityId,
  sort,
  isViewNotesOpened,
  currentNote,
  isAddNotesOpened,
  canEditNotes,
  openAddNotes,
  setSort,
  setCurrentNote,
  deleteNote,
  openViewNotes,
  onChildSuccess,
  closeAddNotes,
  closeViewNotes,
}: INoteListViewProps) => {
  const { hasClaim } = useKeycloakWrapper();

  const { setModalContent, setDisplayModal } = useModalContext();
  const columns = [
    ...createNoteTableColumns(),
    createNoteActionsColumn(
      canEditNotes,
      (note: ApiGen_Concepts_Note) => {
        setCurrentNote(note);
        openViewNotes();
      },
      (note: ApiGen_Concepts_Note) => {
        // show confirmation popup before actually removing the note
        setModalContent({
          ...getDeleteModalProps(),
          variant: 'error',
          title: 'Delete Note',
          message: `Are you sure you want to delete this note?`,
          okButtonText: 'Yes',
          cancelButtonText: 'No',
          handleOk: async () => {
            if (isValidId(note?.id)) {
              const result = await deleteNote(type, note.id);
              if (result === false) {
                console.error('Unable to delete note');
              }
              onChildSuccess();
            } else {
              console.error('Invalid note');
            }
            setDisplayModal(false);
          },
          handleCancel: () => {
            setDisplayModal(false);
          },
        });

        setDisplayModal(true);
      },
    ),
  ];

  const getHeader = (): React.ReactNode => {
    if (hasClaim([Claims.NOTE_ADD]) && canEditNotes) {
      return (
        <SectionListHeader
          claims={[Claims.NOTE_ADD]}
          title="Notes"
          addButtonText="Add a Note"
          addButtonIcon={<FaPlus size={'2rem'} />}
          onButtonAction={openAddNotes}
          title-data-testId="notes-header"
          button-data-testId="note-add-button"
        />
      );
    } else {
      return 'Notes';
    }
  };

  return (
    <Section header={getHeader()} title="notes" isCollapsable initiallyExpanded>
      <NoteResults
        results={notes}
        loading={loading}
        sort={sort}
        setSort={setSort}
        columns={columns}
      />

      <AddNotesContainer
        type={type}
        parentId={entityId}
        isOpened={isAddNotesOpened}
        openModal={openAddNotes}
        closeModal={closeAddNotes}
        onSuccess={onChildSuccess}
      />

      {exists(currentNote) && (
        <NoteContainer
          type={type}
          noteId={currentNote.id}
          isOpened={isViewNotesOpened}
          openModal={openViewNotes}
          closeModal={closeViewNotes}
          onSuccess={onChildSuccess}
        ></NoteContainer>
      )}
    </Section>
  );
};

export default NoteListView;
