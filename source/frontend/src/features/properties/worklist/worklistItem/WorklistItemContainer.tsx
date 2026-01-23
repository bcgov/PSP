import { useEffect, useState } from 'react';

import { WorklistItemModel } from './models/WorlistItem.model';
import WorklistItemView from './WorklistItemView';

export interface WorklistItemContainerProps {
  isCollapsedDefault?: boolean;
  worklistItem: WorklistItemModel;
  parcelIndex: number;
  onRemove: (id: string) => void;
}

const WorklistItemContainer: React.FC<WorklistItemContainerProps> = ({
  isCollapsedDefault,
  worklistItem,
  parcelIndex,
  onRemove,
}) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(null);

  useEffect(() => {
    if (isCollapsed === null) {
      setIsCollapsed(isCollapsedDefault ?? true);
    }
  }, [isCollapsed, isCollapsedDefault]);

  return (
    <WorklistItemView
      isCollapsed={isCollapsed}
      worklistItem={worklistItem}
      parcelIndex={parcelIndex}
      toggleCollapse={() => setIsCollapsed(!isCollapsed)}
      onRemove={onRemove}
    ></WorklistItemView>
  );
};

export default WorklistItemContainer;
