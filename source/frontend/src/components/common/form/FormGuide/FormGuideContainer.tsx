import { useState } from 'react';

import FormGuideView from './FormGuideView';

export interface FormGuideContainerProps {
  title: string;
  guideBody: React.ReactNode;
}

const FormGuideContainer: React.FC<FormGuideContainerProps> = ({ title, guideBody }) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(true);

  const handleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

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
