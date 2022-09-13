import clsx from 'classnames';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Api_Activity } from 'models/api/Activity';
import React from 'react';
import { useState } from 'react';
import { MdClose } from 'react-icons/md';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

import { ActivityFile } from '../detail/ActivityContainer';
import { ActivityForm } from '../detail/ActivityForm';
import { IActivityFormContent } from '../detail/content/models';
import * as Styled from './styles';

export interface IActivityTrayProps {
  activity?: Api_Activity;
  onClose: () => void;
  loading: boolean;
  updateLoading: boolean;
  error: boolean;
  file: ActivityFile;
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
  onSave: (activity: Api_Activity) => Promise<Api_Activity | undefined>;
  onEditRelatedProperties: () => void;
  currentFormContent?: IActivityFormContent;
}

export const ActivityTray: React.FunctionComponent<IActivityTrayProps> = ({
  activity,
  onClose,
  onSave,
  error,
  loading,
  file,
  editMode,
  setEditMode,
  onEditRelatedProperties,
  currentFormContent,
}) => {
  const [show, setShow] = useState(true);
  let trayContent = (
    <HalfHeightDiv>
      {!!activity?.id && (
        <ActivityForm
          activity={{ ...activity, id: +activity.id }}
          file={file as ActivityFile}
          editMode={editMode}
          setEditMode={setEditMode}
          onSave={onSave}
          onEditRelatedProperties={onEditRelatedProperties}
          formContent={currentFormContent}
        />
      )}
    </HalfHeightDiv>
  );
  if (error) {
    trayContent = <b>Failed to load activity. Refresh the page or load another activity.</b>;
  } else if (loading) {
    trayContent = <LoadingBackdrop parentScreen show={loading} />;
  }

  return (
    <ReactVisibilitySensor
      onChange={isVisible => {
        !isVisible && setShow(true);
      }}
    >
      <Styled.ActivityTray className={clsx({ show: show })} data-testid="activity-tray">
        <Styled.TrayHeader>
          Activity&nbsp;-&nbsp;
          {loading
            ? ''
            : activity?.activityTemplate?.activityTemplateTypeCode?.description ?? 'Unknown'}
          <Styled.CloseButton
            id="close-tray"
            icon={<MdClose size={20} />}
            title="close"
            onClick={() => {
              setShow(false);
              onClose();
            }}
          ></Styled.CloseButton>
        </Styled.TrayHeader>
        <Styled.ActivityTrayPage>{trayContent}</Styled.ActivityTrayPage>
      </Styled.ActivityTray>
    </ReactVisibilitySensor>
  );
};

const HalfHeightDiv = styled.div`
  flex-direction: column;
  display: flex;
  height: 50%;
`;
