import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';

import PropertyResearchView from './PropertyResearchView';
import ResearchSummaryView from './ResearchSummaryView';

export interface IDetailViewSelectorProps {
  researchFile: Api_ResearchFile;
  selectedIndex: number;
}

const DetailViewSelector: React.FunctionComponent<IDetailViewSelectorProps> = props => {
  if (props.selectedIndex === 0) {
    return <ResearchSummaryView researchFile={props.researchFile} />;
  } else {
    const properties = props.researchFile.researchProperties || [];
    const selectedPropertyIndex = props.selectedIndex - 1;
    const researchFileProperty = properties[selectedPropertyIndex];
    return <PropertyResearchView researchFileProperty={researchFileProperty} />;
  }
};

export default DetailViewSelector;
