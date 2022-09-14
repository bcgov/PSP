import { FileTypes } from 'constants/fileTypes';
import { SideBarContext, TypedFile } from 'features/properties/map/context/sidebarContext';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Activity, Api_PropertyActivity } from 'models/api/Activity';
import { Api_File } from 'models/api/File';
import { Api_Property } from 'models/api/Property';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { useContext, useState } from 'react';

import { IActivityTrayProps } from '../ActivityTray/ActivityTray';
import { useActivityRepository } from '../hooks/useActivityRepository';
import ActivityPropertyModal from './ActivityPropertyModal';
import { ActivityModel } from './models';

export interface IActivityContainerProps {
  activityId?: number;
  onClose: () => void;
  View: React.FunctionComponent<IActivityTrayProps>;
}

export interface ActivityFile extends TypedFile {
  id: number;
}

export interface Activity extends Api_Activity {
  id: number;
}

export const ActivityContainer: React.FunctionComponent<IActivityContainerProps> = ({
  activityId,
  onClose,
  View,
}) => {
  const [editMode, setEditMode] = useState(false);
  const [display, setDisplay] = useState(false);
  const [selectedFileProperties, setSelectedFileProperties] = useState<Api_Property[]>([]);
  const {
    getActivity: { execute: getActivity, response, error, loading },
    updateActivity: { execute: updateActivity, loading: updateLoading },
    updateActivityProperties: { execute: updateActivityProperties },
  } = useActivityRepository();
  const { file, fileLoading, setStaleFile, getFileProperties } = useContext(SideBarContext);
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
    return updatedActivity;
  };

  const onSaveActivityProperties = async () => {
    if (file?.fileType && !!activityModel) {
      await updateActivityProperties(file?.fileType, activityModel?.toApi());
    }
    setStaleFile(true);
    return await fetchActivity();
  };

  if (!!file && file?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  return !!file?.id ? (
    <>
      <View
        file={file as ActivityFile}
        activity={response}
        onClose={onClose}
        loading={loading}
        updateLoading={updateLoading}
        error={!!error}
        editMode={editMode}
        setEditMode={setEditMode}
        onSave={editActivity}
        onEditRelatedProperties={() => setDisplay(true)}
      ></View>
      <ActivityPropertyModal
        display={display}
        setDisplay={setDisplay}
        activityModel={activityModel}
        selectedFileProperties={selectedFileProperties}
        setSelectedFileProperties={setSelectedFileProperties}
        allProperties={getFileProperties()}
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
) => {
  const activityFilePropertyIds = activityFileProperties.map(af => af.propertyFileId);
  let fileProperties: Api_Property[] = [];
  if (file?.fileType === FileTypes.Research) {
    fileProperties =
      (file as Api_ResearchFile).researchProperties
        ?.filter(rp => rp?.property !== undefined)
        ?.map(rp => rp.property as Api_Property) ?? [];
  } else if (file?.fileType === FileTypes.Acquisition) {
    fileProperties =
      (file as Api_AcquisitionFile).acquisitionProperties
        ?.filter(ap => ap?.property !== undefined)
        ?.map(ap => ap.property as Api_Property) ?? [];
  }
  return (
    fileProperties?.filter(fileProperty => activityFilePropertyIds.includes(fileProperty.id)) ?? []
  );
};

export default ActivityContainer;
