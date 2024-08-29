import { useState } from 'react';

import FormGuideView from './FormGuideView';

export interface FormGuideContainerProps {
  tittle: string;
  guideBody: React.ReactNode;
}

const FormGuideContainer: React.FC<FormGuideContainerProps> = ({ tittle, guideBody }) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(true);

  const handleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  return (
    <FormGuideView
      title={tittle}
      isCollapsed={isCollapsed}
      toggleCollapse={handleCollapse}
      guideBody={guideBody}
    ></FormGuideView>
  );
};

export default FormGuideContainer;
