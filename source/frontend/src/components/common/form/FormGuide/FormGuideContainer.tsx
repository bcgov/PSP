import { useEffect, useState } from 'react';

import FormGuideView from './FormGuideView';

export interface FormGuideContainerProps {
  title: string;
  guideBody: React.ReactNode;
  isCollapsedDefault?: boolean;
}

const FormGuideContainer: React.FC<FormGuideContainerProps> = ({
  title,
  guideBody,
  isCollapsedDefault,
}) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(null);

  const handleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  useEffect(() => {
    if (isCollapsed === null) {
      setIsCollapsed(isCollapsedDefault ?? true);
    }
  }, [isCollapsed, isCollapsedDefault]);

  return (
    <FormGuideView
      title={title}
      isCollapsed={isCollapsed}
      toggleCollapse={handleCollapse}
      guideBody={guideBody}
    ></FormGuideView>
  );
};

export default FormGuideContainer;
