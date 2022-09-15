import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import * as React from 'react';

import { ActivityTray } from '../ActivityTray/ActivityTray';
import { useActivityRepository } from '../hooks/useActivityRepository';

export interface IActivityContainerProps {
  activityId?: number;
  onClose: () => void;
}

export const ActivityContainer: React.FunctionComponent<IActivityContainerProps> = ({
  activityId,
  onClose,
}) => {
  const {
    getActivity: { execute, response, error, loading },
  } = useActivityRepository();

  useDeepCompareEffect(() => {
    const fetchActivity = async () => {
      if (!!activityId) {
        await execute(activityId);
      }
    };
    fetchActivity();
  }, [activityId, execute]);
  return (
    <ActivityTray
      activityId={activityId}
      activity={response}
      onClose={onClose}
      loading={loading}
      error={!!error}
    ></ActivityTray>
  );
};

export default ActivityContainer;
