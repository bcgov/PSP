import { SideBarContext } from 'features/properties/map/context/sidebarContext';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { Api_Activity } from 'models/api/Activity';
import { Api_File } from 'models/api/File';
import * as React from 'react';
import { useContext, useState } from 'react';

import { IActivityTrayProps } from '../ActivityTray/ActivityTray';
import { useActivityRepository } from '../hooks/useActivityRepository';

export interface IActivityContainerProps {
  activityId?: number;
  onClose: () => void;
  View: React.FunctionComponent<IActivityTrayProps>;
}

export interface ActivityFile extends Api_File {
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
  const {
    getActivity: { execute: getActivity, response, error, loading },
    updateActivity: { execute: updateActivity, loading: updateLoading },
  } = useActivityRepository();
  const { file, fileLoading, setStaleFile } = useContext(SideBarContext);

  useDeepCompareEffect(() => {
    fetchActivity();
  }, [activityId, getActivity]);
  const fetchActivity = async () => {
    if (!!activityId) {
      await getActivity(activityId);
    }
  };

  const editActivity = async (activity: Api_Activity) => {
    const updatedActivity = await updateActivity(activity);
    setStaleFile(true);
    return updatedActivity;
  };

  if (!!file && file?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }
  const activityFile = file as ActivityFile;

  return !!file ? (
    <View
      file={activityFile}
      activity={response}
      onClose={onClose}
      loading={loading}
      updateLoading={updateLoading}
      error={!!error}
      editMode={editMode}
      setEditMode={setEditMode}
      onSave={editActivity}
    ></View>
  ) : null;
};

export default ActivityContainer;
