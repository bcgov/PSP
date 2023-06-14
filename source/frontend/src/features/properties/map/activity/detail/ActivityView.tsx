import * as React from 'react';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { Claims } from '@/constants/claims';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import { NoteTypes } from '@/constants/noteTypes';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { NoteListView } from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { Activity, ActivityFile } from './ActivityContainer';
import { ActivityControlsBar } from './ActivityControlsBar';
import { ActivityDescription } from './ActivityDescription';
import ActivityHeader from './ActivityHeader';

export interface IActivityViewProps {
  file: ActivityFile;
  activity: Activity;
  editMode: boolean;
  isEditable: boolean;
  onEditRelatedProperties: () => void;
}

export const ActivityView: React.FunctionComponent<React.PropsWithChildren<IActivityViewProps>> = ({
  file,
  activity,
  editMode,
  isEditable,
  onEditRelatedProperties,
  children,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <ActivityHeader file={file} activity={activity} />
      <StyledContent>
        <ActivityControlsBar
          editMode={editMode}
          onEditRelatedProperties={onEditRelatedProperties}
        />
        <Section header="Description" isCollapsable initiallyExpanded title="description">
          <ActivityDescription isEditable={isEditable} editMode={editMode} />
        </Section>
        {children}
        {hasClaim(Claims.DOCUMENT_VIEW) && (
          <DocumentListContainer
            relationshipType={DocumentRelationshipType.ACTIVITIES}
            parentId={activity.id.toString()}
          />
        )}
        {hasClaim(Claims.NOTE_VIEW) && (
          <NoteListView type={NoteTypes.Activity} entityId={activity.id} />
        )}
      </StyledContent>
    </>
  );
};

export default ActivityView;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  margin-bottom: 1.5rem;
`;
