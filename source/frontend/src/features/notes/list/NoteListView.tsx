import orderBy from 'lodash/orderBy';
import React from 'react';
import { FaPlus } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableSort } from '@/components/Table/TableSort';
import { Claims } from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { exists, isValidId } from '@/utils';

import { AddNotesContainer } from '../add/AddNotesContainer';
import { NoteContainer } from '../NoteContainer';
import { NoteResults } from './NoteResults/NoteResults';

export interface INoteListViewProps {
  type: NoteTypes;
  entityId: number;
  onSuccess?: () => void;
}

/**
 * Page that displays notes information.
 */
export const NoteListView: React.FunctionComponent<React.PropsWithChildren<INoteListViewProps>> = (
  props: INoteListViewProps,
) => {
  const { type, entityId, onSuccess } = props;
  const { setModalContent, setDisplayModal } = useModalContext();
  const {
    getAllNotes: { execute: getAllNotes, loading: loadingNotes, response: notesResponse },
    deleteNote: { execute: deleteNote, loading: loadingDeleteNote },
  } = useNoteRepository();

  const [currentNote, setCurrentNote] = React.useState<ApiGen_Concepts_Note>();

  // Notes should display by default in descending order of created date
  const [sort, setSort] = React.useState<TableSort<ApiGen_Concepts_Note>>({
    appCreateTimestamp: 'desc',
  });

  const [isAddNotesOpened, openAddNotes, closeAddNotes] = useModalManagement();
  const [isViewNotesOpened, openViewNotes, closeViewNotes] = useModalManagement();

  React.useEffect(() => {
    if (isValidId(entityId)) {
      getAllNotes(type, entityId);
    }
  }, [entityId, getAllNotes, type]);

  const sortedNoteList: ApiGen_Concepts_Note[] = React.useMemo(() => {
    if (exists(sort) && notesResponse?.length > 0) {
      const sortFields = Object.keys(sort);
      if (sortFields?.length > 0) {
        const keyName = sort[sortFields[0]];
        return orderBy(notesResponse, sortFields[0], keyName);
      }
      return notesResponse;
    }
    return [];
  }, [notesResponse, sort]);

  const onChildSuccess = async () => {
    await getAllNotes(type, entityId);
    onSuccess?.();
  };

  // UI components
  const loading = loadingNotes || loadingDeleteNote;

  return (
    <Section
      header={
        <SectionListHeader
          claims={[Claims.NOTE_ADD]}
          title="Notes"
          addButtonText="Add a Note"
          addButtonIcon={<FaPlus size={'2rem'} />}
          onAdd={openAddNotes}
        />
      }
      title="notes"
      isCollapsable
      initiallyExpanded
    >
      <NoteResults
        results={sortedNoteList}
        loading={loading}
        sort={sort}
        setSort={setSort}
        onShowDetails={(note: ApiGen_Concepts_Note) => {
          setCurrentNote(note);
          openViewNotes();
        }}
        onDelete={(note: ApiGen_Concepts_Note) => {
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
        }}
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
