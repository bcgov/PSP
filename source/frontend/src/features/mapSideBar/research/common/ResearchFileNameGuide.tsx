import { FunctionComponent, PropsWithChildren, ReactNode } from 'react';

import FormGuideContainer from '@/components/common/form/FormGuide/FormGuideContainer';

export const ResearchFileNameGuide: FunctionComponent<PropsWithChildren<unknown>> = () => {
  const guideBodyContent = (): ReactNode => {
    return (
      <>
        <p className="mb-4">
          The recommended format for naming Research files is to provide the Road name, followed by
          the corresponding MOTT District, followed by supporting descriptive text.
        </p>
        <p className="mb-4">
          If the road name is not available /applicable, use supporting descriptive text that
          identifies the location and enables searchability of the file in the future.
        </p>
        <p className="mb-4">
          Descriptive text may include (but is not limited to) one or more of the following:
        </p>
        <ul>
          <li>Project name</li>
          <li>Road intersection (or similar unique reference)</li>
          <li>Geographic area (municipality/regional district/unincorporated area)</li>
          <li>Legal description (District lot, Section, Plan)</li>
        </ul>
        <p className="mb-4">
          <i>Examples:</i>
        </p>
        <ul>
          <li>
            <i>Hwy 16 – Skeena District – New Hazelton to College St</i>
          </li>
          <li>
            <i>Hwy 97 – Okanagan Shuswap – Smith property</i>
          </li>
          <li>
            <i>Plywood Rd – Cariboo District – DL 222</i>
          </li>
        </ul>
        <p>
          <b>Note:</b> a unique file number will be generated for your research file when you save
          it.
        </p>
      </>
    );
  };

  return (
    <FormGuideContainer
      title="Help with choosing a name"
      guideBody={guideBodyContent()}
    ></FormGuideContainer>
  );
};
