import orderBy from 'lodash/orderBy';
import React from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { NoteTypes } from '@/constants/noteTypes';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { exists, isValidId } from '@/utils';

import { IUpdateNotesStrategy } from '../models/IUpdateNotesStrategy';
import { INoteListViewProps } from './NoteListView';

export interface INoteListContainerProps {
  type: NoteTypes;
  entityId: number;
  View: React.FunctionComponent<React.PropsWithChildren<INoteListViewProps>>;
  statusSolver?: IUpdateNotesStrategy | null;
  onSuccess?: () => void;
}

/**
 * Page that displays notes information.
 */
export const NoteListContainer: React.FunctionComponent<
  React.PropsWithChildren<INoteListContainerProps>
> = ({ type, entityId, onSuccess, View, statusSolver }: INoteListContainerProps) => {
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

  const onChildSuccess = async () => {
    await getAllNotes(type, entityId);
    onSuccess?.();
  };

  // UI components
  const loading = loadingNotes || loadingDeleteNote;

  const editNotesEnabled = !statusSolver || (statusSolver && statusSolver.canEditNotes());

  return (
    <View
      loading={loading}
      notes={sortNotes(sort, notesResponse)}
      type={type}
      entityId={entityId}
      sort={sort}
      isAddNotesOpened={isAddNotesOpened}
      isViewNotesOpened={isViewNotesOpened}
      currentNote={currentNote}
      canEditNotes={editNotesEnabled}
      setSort={setSort}
      openAddNotes={openAddNotes}
      closeAddNotes={closeAddNotes}
      setCurrentNote={setCurrentNote}
      openViewNotes={openViewNotes}
      closeViewNotes={closeViewNotes}
      onChildSuccess={onChildSuccess}
      deleteNote={deleteNote}
    />
  );
};

export const sortNotes = (
  sort: TableSort<ApiGen_Concepts_Note>,
  notesResponse: ApiGen_Concepts_Note[],
) => {
  if (exists(sort) && notesResponse?.length > 0) {
    const sortFields = Object.keys(sort);
    if (sortFields?.length > 0) {
      const keyName = sort[sortFields[0]];
      return orderBy(notesResponse, sortFields[0], keyName);
    }
    return notesResponse;
  }
  return [];
};

export default NoteListContainer;
