import React, { useCallback, useState } from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { NoteTypes } from '@/constants/noteTypes';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_Association } from '@/models/api/generated/ApiGen_Concepts_Association';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { isValidId } from '@/utils';

import { sortNotes } from './NoteListContainer';
import { INoteListViewProps } from './NoteListView';

export interface INoteSummaryContainerProps {
  associationType: NoteTypes;
  entityId: number;
  onSuccess?: () => void;
  NoteListView: React.FunctionComponent<React.PropsWithChildren<INoteListViewProps>>;
}

/**
 * Container that retrieved a summary of notes from a management file, related to a property.
 * It retrieves the notes from the management file and displays them in a list.
 * @param entityId The ID of the entity to retrieve notes for.
 * @param onSuccess Callback function to be called when the notes are successfully retrieved.
 * @param NoteListView The component to display the list of notes.
 * @returns A React component that displays a summary of notes from a management file.
 */
export const NoteSummaryContainer: React.FunctionComponent<
  React.PropsWithChildren<INoteSummaryContainerProps>
> = ({ associationType, entityId, onSuccess, NoteListView }: INoteSummaryContainerProps) => {
  const { execute: getAllPropertyAssociations, loading: loadingAssociations } =
    usePropertyAssociations();
  const {
    getAllNotes: { execute: getNotes, loading: loadingNotes },
  } = useNoteRepository();

  const [noteAssociations, setNoteAssociations] = useState<
    { note: ApiGen_Concepts_Note; association: ApiGen_Concepts_Association }[]
  >([]);

  const getAllNotes = useCallback(
    async (entityId: number) => {
      setNoteAssociations([]);
      const associations = await getAllPropertyAssociations(entityId);

      const noteAssociations: {
        note: ApiGen_Concepts_Note;
        association: ApiGen_Concepts_Association;
      }[] = [];
      const notesPromises = associations?.managementAssociations?.map(association =>
        getNotes(associationType, association.id).then(notes => {
          notes.forEach(note => {
            noteAssociations.push({ note, association });
          });
        }),
      );
      await Promise.all(notesPromises);
      setNoteAssociations(noteAssociations);
    },
    [getAllPropertyAssociations, getNotes, associationType],
  );

  const [currentNote, setCurrentNote] = React.useState<ApiGen_Concepts_Note>();

  // Notes should display by default in descending order of created date
  const [sort, setSort] = React.useState<TableSort<ApiGen_Concepts_Note>>({
    appCreateTimestamp: 'desc',
  });

  const [isViewNotesOpened, openViewNotes, closeViewNotes] = useModalManagement();

  React.useEffect(() => {
    if (isValidId(entityId)) {
      getAllNotes(entityId);
    }
  }, [entityId, getAllNotes]);

  const onChildSuccess = async () => {
    await getAllNotes(entityId);
    onSuccess?.();
  };

  const loading = loadingNotes || loadingAssociations;

  return (
    <NoteListView
      loading={loading}
      notes={sortNotes(
        sort,
        noteAssociations?.map(na => na.note),
      )}
      type={NoteTypes.Property}
      entityId={entityId}
      sort={sort}
      isViewNotesOpened={isViewNotesOpened}
      currentNote={currentNote}
      setSort={setSort}
      setCurrentNote={setCurrentNote}
      openViewNotes={openViewNotes}
      closeViewNotes={closeViewNotes}
      onChildSuccess={onChildSuccess}
      getNoteNavigationUrlTitle={(row: ApiGen_Concepts_Note) => {
        const noteAssociation = noteAssociations?.find(
          association => association.note.id === row.id,
        );
        return {
          url: `/mapview/sidebar/management/${noteAssociation?.association.id}/notes`,
          title: `M-${noteAssociation?.association.fileName}`,
        };
      }}
    />
  );
};

export default NoteSummaryContainer;
