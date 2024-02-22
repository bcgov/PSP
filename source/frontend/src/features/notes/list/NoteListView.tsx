import { orderBy } from 'lodash';
import React, { useCallback } from 'react';
import { toast } from 'react-toastify';

import GenericModal from '@/components/common/GenericModal';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableSort } from '@/components/Table/TableSort';
import { Claims } from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useModalManagement } from '@/hooks/useModalManagement';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';

import { AddNotesContainer } from '../add/AddNotesContainer';
import { NoteContainer } from '../NoteContainer';
import { NoteResults } from './NoteResults/NoteResults';
import * as Styled from './styles';

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
  const isMounted = useIsMounted();
  const { getNotes, deleteNote } = useApiNotes();

  const [showDeleteConfirm, setShowDeleteConfirm] = React.useState<boolean>(false);
  const [currentNote, setCurrentNote] = React.useState<ApiGen_Concepts_Note>();
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  // Notes should display by default in descending order of created date
  const [sort, setSort] = React.useState<TableSort<ApiGen_Concepts_Note>>({
    appCreateTimestamp: 'desc',
  });
  const [noteResult, setNoteResult] = React.useState<ApiGen_Concepts_Note[]>([]);

  const [isAddNotesOpened, openAddNotes, closeAddNotes] = useModalManagement();
  const [isViewNotesOpened, openViewNotes, closeViewNotes] = useModalManagement();

  const fetchNotes = useCallback(async () => {
    setIsLoading(true);
    try {
      const { data } = await getNotes(type, entityId);
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
        return orderBy(noteResult, sortFields[0], keyName) as ApiGen_Concepts_Note[];
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
      onSuccess?.();
    } finally {
      setIsLoading(false);
    }
  };

  const onChildSuccess = () => {
    fetchNotes();
    onSuccess?.();
  };

  return (
    <Styled.ListPage>
      <Styled.Scrollable vertical={true}>
        <Section
          header={
            <SectionListHeader
              claims={[Claims.NOTE_ADD]}
              title="Notes"
              addButtonText="Add a Note"
              onAdd={openAddNotes}
            />
          }
          title="notes"
          isCollapsable
          initiallyExpanded
        >
          <NoteResults
            results={sortedNoteList}
            loading={isLoading}
            sort={sort}
            setSort={setSort}
            onShowDetails={(note: ApiGen_Concepts_Note) => {
              setCurrentNote(note);
              openViewNotes();
            }}
            onDelete={(note: ApiGen_Concepts_Note) => {
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
            onSuccess={onChildSuccess}
          />

          {currentNote && (
            <NoteContainer
              type={type}
              noteId={currentNote.id as number}
              isOpened={isViewNotesOpened}
              openModal={openViewNotes}
              closeModal={closeViewNotes}
              onSuccess={onChildSuccess}
            ></NoteContainer>
          )}

          <GenericModal
            variant="info"
            display={showDeleteConfirm}
            title="Delete Note"
            message={`Are you sure you want to delete this note?`}
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
