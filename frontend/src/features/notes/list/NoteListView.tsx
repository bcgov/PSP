import GenericModal from 'components/common/GenericModal';
import { TableSort } from 'components/Table/TableSort';
import { NoteTypes } from 'constants/noteTypes';
import { useApiNotes } from 'hooks/pims-api/useApiNotes';
import useIsMounted from 'hooks/useIsMounted';
import { orderBy } from 'lodash';
import { Api_Note } from 'models/api/Note';
import React from 'react';
import { toast } from 'react-toastify';

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

  React.useEffect(() => {
    setIsLoading(true);
    getNotes(type, entityId)
      .then(({ data }) => {
        if (data && isMounted()) {
          setNoteResult(data);
        }
      })
      .finally(() => setIsLoading(false));
  }, [type, entityId, isMounted, getNotes]);

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

  const onDeleteNote = () => {
    setIsLoading(true);
    deleteNote(type, currentNote?.id ?? 0)
      .then(() => {
        setShowDeleteConfirm(false);
        toast.success('Deleted successfully.');
        setIsLoading(true);
        getNotes(type, entityId)
          .then(({ data }) => {
            setNoteResult(data);
          })
          .finally(() => setIsLoading(false));
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  return (
    <Styled.ListPage>
      <Styled.Scrollable vertical={true}>
        <Styled.PageHeader>Notes</Styled.PageHeader>

        <NoteResults
          results={sortedNoteList}
          loading={isLoading}
          sort={sort}
          setSort={setSort}
          onDelete={(note: Api_Note) => {
            setCurrentNote(note);
            setShowDeleteConfirm(true);
          }}
        />
        <GenericModal
          display={showDeleteConfirm}
          title="Delete Note"
          message={`Are you sure you want to delete note?`}
          handleOk={() => onDeleteNote()}
          okButtonText="OK"
          cancelButtonText="Cancel"
          closeButton={false}
          setDisplay={setShowDeleteConfirm}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default NoteListView;
