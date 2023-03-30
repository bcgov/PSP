import { SideBarContext, TypedFile } from 'features/properties/map/context/sidebarContext';
import { Api_Activity, Api_PropertyActivity } from 'models/api/Activity';
import { Api_Property } from 'models/api/Property';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import * as React from 'react';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { useContext, useState } from 'react';

import { formContent } from '../../shared/content/formContent';
import { ActivityTemplateTypes } from '../../shared/content/models';
import { IActivityTrayProps } from '../ActivityTray/ActivityTray';
import { useActivityRepository } from '../hooks/useActivityRepository';
import ActivityPropertyModal from './ActivityPropertyModal';
import { ActivityModel } from './models';

export interface IActivityContainerProps {
  activityId?: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IActivityTrayProps>>;
}

export interface ActivityFile extends TypedFile {
  id: number;
}

export interface Activity extends Api_Activity {
  id: number;
}

export const ActivityContainer: React.FunctionComponent<
  React.PropsWithChildren<IActivityContainerProps>
> = ({ activityId, onClose, View }) => {
  const [editMode, setEditMode] = useState(false);
  const [display, setDisplay] = useState(false);
  const [selectedFileProperties, setSelectedFileProperties] = useState<Api_Property[]>([]);
  const {
    getActivity: { execute: getActivity, response, error, loading },
    updateActivity: { execute: updateActivity, loading: updateLoading },
    updateActivityProperties: { execute: updateActivityProperties },
  } = useActivityRepository();
  const { file, fileLoading, setStaleFile } = useContext(SideBarContext);
  const activityModel =
    response && file?.fileType ? ActivityModel.fromApi(response, file?.fileType) : undefined;

  const fetchActivity = useCallback(async () => {
    if (!!activityId) {
      const activity = await getActivity(activityId);
      if (!!activity && !!file?.fileType) {
        setSelectedFileProperties(
          getActivityPropertiesOnFile(
            file,
            ActivityModel.fromApi(activity, file.fileType).actInstPropFiles ?? [],
          ),
        );
      }
      return activity;
    }
  }, [activityId, file, getActivity]);
  useEffect(() => {
    fetchActivity();
  }, [activityId, getActivity, fetchActivity]);

  const editActivity = async (activity: Api_Activity) => {
    const updatedActivity = await updateActivity(activity);
    setStaleFile(true);
    fetchActivity();
    return updatedActivity;
  };

  const onSaveActivityProperties = async (updatedActivity: ActivityModel) => {
    if (file?.fileType && !!activityModel) {
      await updateActivityProperties(file?.fileType, updatedActivity.toApi());
    }
    setStaleFile(true);
    return await fetchActivity();
  };

  if (!!file && file?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  const currentFormContent = response?.activityTemplate?.activityTemplateTypeCode?.id
    ? formContent.get(
        response?.activityTemplate?.activityTemplateTypeCode?.id as ActivityTemplateTypes,
      )
    : undefined;
  return !!file?.id ? (
    <>
      <View
        onClose={onClose}
        loading={loading}
        updateLoading={updateLoading}
        error={!!error}
        activity={response}
        onEditRelatedProperties={() => setDisplay(true)}
        onSave={editActivity}
        editMode={editMode}
        setEditMode={setEditMode}
        file={file as ActivityFile}
        currentFormContent={currentFormContent}
      ></View>
      <ActivityPropertyModal
        display={display}
        setDisplay={setDisplay}
        activityModel={activityModel}
        selectedFileProperties={selectedFileProperties}
        setSelectedFileProperties={setSelectedFileProperties}
        allProperties={file.fileProperties ?? []}
        originalSelectedProperties={getActivityPropertiesOnFile(
          file,
          activityModel?.actInstPropFiles ?? [],
        )}
        onSave={onSaveActivityProperties}
      />
    </>
  ) : null;
};

export const getActivityPropertiesOnFile = (
  file: TypedFile,
  activityFileProperties: Api_PropertyActivity[],
): Api_PropertyFile[] => {
  const activityFilePropertyIds = activityFileProperties.map(af => af.propertyFileId);
  return file.fileProperties
    ?.filter(acquisitionProperty => activityFilePropertyIds.includes(acquisitionProperty.id))
    .map(p => ({ id: p.id })) as Api_PropertyFile[];
};

export default ActivityContainer;
