import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { NoteTypes } from 'constants/noteTypes';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { Section } from 'features/mapSideBar/tabs/Section';
import { NoteListView } from 'features/notes/list/NoteListView';
import noop from 'lodash/noop';
import * as React from 'react';

import { Activity, ActivityFile } from './ActivityContainer';
import { ActivityControlsBar } from './ActivityControlsBar';
import { ActivityDescription } from './ActivityDescription';
import ActivityHeader from './ActivityHeader';

export interface IActivityViewProps {
  activity: Activity;
  file: ActivityFile;
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
}

export const ActivityView: React.FunctionComponent<IActivityViewProps> = ({
  file,
  activity,
  editMode,
  setEditMode,
}) => {
  return (
    <>
      <ActivityHeader file={file} activity={activity} />
      <ActivityControlsBar
        editMode={editMode}
        setEditMode={setEditMode}
        onEditRelatedProperties={noop}
      />
      <Section header="Description" isCollapsable initiallyExpanded title="description">
        <ActivityDescription editMode={editMode} />
      </Section>
      <DocumentListContainer
        relationshipType={DocumentRelationshipType.ACTIVITIES}
        parentId={activity.id}
      />
      <NoteListView type={NoteTypes.Activity} entityId={activity.id} />
    </>
  );
};

export default ActivityView;
