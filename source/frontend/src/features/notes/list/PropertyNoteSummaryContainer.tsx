import React, { useCallback, useEffect, useState } from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { NoteTypes } from '@/constants/noteTypes';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { useModalManagement } from '@/hooks/useModalManagement';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { exists, getFilePropertyName } from '@/utils';

import { sortNotes } from './NoteListContainer';
import { IPropertyNotesSummaryViewProps } from './PropertyNoteSummaryView';

export interface IPropertyNoteSummaryContainerProps {
  fileProperties: ApiGen_Concepts_FileProperty[];
  onSuccess?: () => void;
  View: React.FunctionComponent<IPropertyNotesSummaryViewProps>;
}

interface FilePropertyNote {
  note: ApiGen_Concepts_Note;
  fileProperty: ApiGen_Concepts_FileProperty;
}

/**
 * Container that retrieves a summary of notes from properties belonging to a management file.
 * It retrieves the notes from the properties and displays them in a list.
 * @returns A React component that displays a summary of notes from properties in a management file.
 */
export const PropertyNoteSummaryContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyNoteSummaryContainerProps>
> = ({ fileProperties, onSuccess, View }: IPropertyNoteSummaryContainerProps) => {
  const {
    getAllNotes: { execute: getNotes },
  } = useNoteRepository();

  const isMounted = useIsMounted();

  const [propertyNotes, setPropertyNotes] = useState<FilePropertyNote[]>([]);
  const [loadingNotes, setLoadingNotes] = useState(false);

  const getAllPropertyNotes = useCallback(
    async (fileProperties: ApiGen_Concepts_FileProperty[]) => {
      setLoadingNotes(true);
      setPropertyNotes([]);
      const propertyNotes: FilePropertyNote[] = [];

      // fetch the notes for each property within the file
      const notesPromises = fileProperties?.map(fp =>
        getNotes(NoteTypes.Property, fp.propertyId).then(notes => {
          for (const note of notes) {
            propertyNotes.push({ note, fileProperty: fp });
          }
        }),
      );

      await Promise.all(notesPromises);

      if (isMounted()) {
        setPropertyNotes(propertyNotes);
        setLoadingNotes(false);
      }
    },
    [getNotes, isMounted],
  );

  const [currentNote, setCurrentNote] = useState<ApiGen_Concepts_Note>();

  // Notes should display by default in descending order of created date
  const [sort, setSort] = useState<TableSort<ApiGen_Concepts_Note>>({
    appCreateTimestamp: 'desc',
  });

  const [isViewNotesOpened, openViewNotes, closeViewNotes] = useModalManagement();

  useEffect(() => {
    if (exists(fileProperties) && fileProperties.length > 0) {
      getAllPropertyNotes(fileProperties);
    }
  }, [fileProperties, getAllPropertyNotes]);

  const onChildSuccess = async () => {
    if (exists(fileProperties) && fileProperties.length > 0) {
      await getAllPropertyNotes(fileProperties);
    }
    onSuccess?.();
  };

  return (
    <View
      loading={loadingNotes}
      notes={sortNotes(
        sort,
        propertyNotes?.map(pn => pn.note),
      )}
      type={NoteTypes.Property}
      sort={sort}
      isViewNotesOpened={isViewNotesOpened}
      currentNote={currentNote}
      canEditNotes={true}
      setSort={setSort}
      setCurrentNote={setCurrentNote}
      openViewNotes={openViewNotes}
      closeViewNotes={closeViewNotes}
      onChildSuccess={onChildSuccess}
      getNoteNavigationUrlTitle={(row: ApiGen_Concepts_Note) => {
        const propertyNote = propertyNotes?.find(pn => pn.note.id === row.id);
        const propertyName = getFilePropertyName(propertyNote?.fileProperty);
        return {
          url: `/mapview/sidebar/property/${propertyNote?.fileProperty?.property?.id}/notes`,
          title: `${propertyName.label}: ${propertyName.value}`,
        };
      }}
    />
  );
};

export default PropertyNoteSummaryContainer;
