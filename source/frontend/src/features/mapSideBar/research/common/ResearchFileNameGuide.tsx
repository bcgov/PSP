import { FunctionComponent, PropsWithChildren, ReactNode } from 'react';

import FormGuideContainer from '@/components/common/form/FormGuide/FormGuideContainer';

export const ResearchFileNameGuide: FunctionComponent<PropsWithChildren<unknown>> = () => {
  const guideBodyContent = (): ReactNode => {
    return (
      <>
        <p className="mb-4">
          Provide a predictable research file name that will be easy to search for. Recommended
          format is to use the road name(s) followed by some descriptive text that may include (but
          not limited to) one or more of the following:
        </p>
        <ul>
          <li>Ministry project name</li>
          <li>Name of the area</li>
          <li>Name of the MoTI highway district</li>
          <li>Name of the enquirer</li>
          <li>Legal description</li>
        </ul>
        <p className="mb-4 mt-3">
          If the road name is not available /applicable, the descriptive text should make the
          research file easy to search for in the future
        </p>
        <p>
          <b>Note:</b> This name does not need to be entirely unique, as a unique file number will
          be generated for your research file when you save it.
        </p>
      </>
    );
  };

  return (
    <FormGuideContainer
      tittle="Help with choosing a name"
      guideBody={guideBodyContent()}
    ></FormGuideContainer>
  );
};
