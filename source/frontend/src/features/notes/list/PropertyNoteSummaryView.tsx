import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants/claims';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { exists } from '@/utils/utils';

import { NoteContainer } from '../NoteContainer';
import { INoteListViewProps } from './NoteListView';
import {
  createNoteActionsColumn,
  createNoteLinkColumn,
  createNoteTableColumns,
} from './NoteResults/columns';
import { NoteResults } from './NoteResults/NoteResults';

export type IPropertyNotesSummaryViewProps = Omit<INoteListViewProps, 'entityId'>;

/**
 * Page that displays a summary of notes from a management file.
 */
export const PropertyNoteSummaryView: React.FunctionComponent<IPropertyNotesSummaryViewProps> = ({
  notes,
  loading,
  sort,
  currentNote,
  type,
  isViewNotesOpened,
  setSort,
  openViewNotes,
  closeViewNotes,
  setCurrentNote,
  getNoteNavigationUrlTitle,
}: INoteListViewProps) => {
  const columns = [
    createNoteLinkColumn('Property Name', getNoteNavigationUrlTitle),
    ...createNoteTableColumns(),
    createNoteActionsColumn((note: ApiGen_Concepts_Note) => {
      setCurrentNote(note);
      openViewNotes();
    }, null),
  ];

  return (
    <Section
      header={
        <div className="d-flex">
          <SectionListHeader claims={[Claims.NOTE_VIEW]} title="Property Notes" className="mr-2" />
          <TooltipIcon
            toolTipId="property-note-summary"
            toolTip="These are all the notes that are associated directly with any property on the file."
            className="align-self-end"
          />
        </div>
      }
      title="notes"
      isCollapsable
      initiallyExpanded
    >
      <NoteResults
        results={notes}
        loading={loading}
        sort={sort}
        setSort={setSort}
        columns={columns}
      />
      {exists(currentNote) && (
        <NoteContainer
          type={type}
          noteId={currentNote.id}
          isOpened={isViewNotesOpened}
          openModal={openViewNotes}
          closeModal={closeViewNotes}
          isReadOnly
        ></NoteContainer>
      )}
    </Section>
  );
};

export default PropertyNoteSummaryView;
