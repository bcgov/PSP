import GenericModal from 'components/common/GenericModal';
import { TableSort } from 'components/Table/TableSort';
import { NoteTypes } from 'constants/noteTypes';
import { Section } from 'features/mapSideBar/tabs/Section';
import { useApiNotes } from 'hooks/pims-api/useApiNotes';
import useIsMounted from 'hooks/useIsMounted';
import { useModalManagement } from 'hooks/useModalManagement';
import { orderBy } from 'lodash';
import { Api_Note } from 'models/api/Note';
import React, { useCallback } from 'react';
import { toast } from 'react-toastify';

import { AddNotesContainer } from '../add/AddNotesContainer';
import { NoteContainer } from '../NoteContainer';
import { NoteListHeader } from './NoteListHeader/NoteListHeader';
import { NoteResults } from './NoteResults/NoteResults';
import * as Styled from './styles';

export interface INoteListViewProps {
  type: NoteTypes;
  entityId: number;
}
/**
 * Page that displays notes information.
 */
export const NoteListView: React.FunctionComponent<INoteListViewProps> = (
  props: INoteListViewProps,
) => {
  const { type, entityId } = props;
  const isMounted = useIsMounted();
  const { getNotes, deleteNote } = useApiNotes();

  const [showDeleteConfirm, setShowDeleteConfirm] = React.useState<boolean>(false);
  const [currentNote, setCurrentNote] = React.useState<Api_Note>();
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [sort, setSort] = React.useState<TableSort<Api_Note>>({});
  const [noteResult, setNoteResult] = React.useState<Api_Note[]>([]);

  const [isAddNotesOpened, openAddNotes, closeAddNotes] = useModalManagement();
  const [isViewNotesOpened, openViewNotes, closeViewNotes] = useModalManagement();

  const fetchNotes = useCallback(async () => {
    setIsLoading(true);
    try {
      const { data } = await getNotes(type, entityId);
      setNoteResult(data);
      if (data && isMounted()) {
        setNoteResult(data);
      }
    } finally {
      setIsLoading(false);
    }
  }, [getNotes, type, entityId, isMounted]);

  React.useEffect(() => {
    fetchNotes();
  }, [fetchNotes]);

  const sortedNoteList = React.useMemo(() => {
    if (sort && noteResult?.length) {
      const sortFields = Object.keys(sort);
      if (sortFields?.length > 0) {
        const keyName = (sort as any)[sortFields[0]];
        return orderBy(noteResult, sortFields[0], keyName) as Api_Note[];
      }
      return noteResult;
    }
    return [];
  }, [noteResult, sort]);

  const onDeleteNoteConfirm = async () => {
    setIsLoading(true);
    try {
      await deleteNote(type, currentNote?.id ?? 0);
      setShowDeleteConfirm(false);
      toast.success('Deleted successfully.');
      fetchNotes();
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Styled.ListPage>
      <Styled.Scrollable vertical={true}>
        <Section
          header={<NoteListHeader title="Notes" onAddNote={openAddNotes} />}
          isCollapsable
          initiallyExpanded
        >
          <NoteResults
            results={sortedNoteList}
            loading={isLoading}
            sort={sort}
            setSort={setSort}
            onShowDetails={(note: Api_Note) => {
              setCurrentNote(note);
              openViewNotes();
            }}
            onDelete={(note: Api_Note) => {
              setCurrentNote(note);
              setShowDeleteConfirm(true);
            }}
          />

          <AddNotesContainer
            type={type}
            parentId={entityId}
            isOpened={isAddNotesOpened}
            openModal={openAddNotes}
            closeModal={closeAddNotes}
            onSuccess={fetchNotes}
          />

          {currentNote && (
            <NoteContainer
              type={type}
              noteId={currentNote.id as number}
              isOpened={isViewNotesOpened}
              openModal={openViewNotes}
              closeModal={closeViewNotes}
              onSuccess={fetchNotes}
            ></NoteContainer>
          )}

          <GenericModal
            display={showDeleteConfirm}
            title="Delete Note"
            message={`Are you sure you want to delete note?`}
            handleOk={onDeleteNoteConfirm}
            okButtonText="OK"
            cancelButtonText="Cancel"
            closeButton={false}
            setDisplay={setShowDeleteConfirm}
          />
        </Section>
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default NoteListView;
