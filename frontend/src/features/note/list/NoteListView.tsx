import GenericModal from 'components/common/GenericModal';
import { TableSort } from 'components/Table/TableSort';
import { useApiNotes } from 'hooks/pims-api/useApiNotes';
import useIsMounted from 'hooks/useIsMounted';
import { INoteResult } from 'interfaces/INoteResult';
import { orderBy } from 'lodash';
import { NoteType } from 'models/api/Note';
import React from 'react';
import { toast } from 'react-toastify';

import { NoteResults } from './NoteResults/NoteResults';
import * as Styled from './styles';

export interface INoteResultProps {
  type: NoteType;
}
/**
 * Page that displays notes information.
 */
export const NoteListView: React.FunctionComponent<INoteResultProps> = (
  props: INoteResultProps,
) => {
  const { type } = props;
  const isMounted = useIsMounted();
  const { getNotes, deleteNote } = useApiNotes();

  const [showDeleteConfirm, setShowDeleteConfirm] = React.useState<boolean>(false);
  const [currentNote, setCurrentNote] = React.useState<INoteResult>();
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [sort, setSort] = React.useState<TableSort<INoteResult>>({});
  const [noteResult, setNoteResult] = React.useState<INoteResult[]>([]);

  React.useEffect(() => {
    setIsLoading(true);
    getNotes(type)
      .then(({ data }) => {
        if (data && isMounted()) {
          setNoteResult(data);
        }
      })
      .finally(() => setIsLoading(false));
  }, [type, isMounted, getNotes]);

  const sortedNoteList = React.useMemo(() => {
    if (sort && noteResult?.length) {
      const sortFields = Object.keys(sort);
      if (sortFields?.length > 0) {
        const keyName = (sort as any)[sortFields[0]];
        return orderBy(noteResult, sortFields[0], keyName) as INoteResult[];
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
        getNotes(type)
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
          onDelete={(note: INoteResult) => {
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
