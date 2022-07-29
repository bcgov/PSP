import { FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';

import PropertyResearchContainer from './detail/PropertyResearchContainer';
// import ResearchSummaryView from './detail/ResearchSummaryView';
import ResearchTabsContainer from './ResearchTabsContainer';
import UpdatePropertyView from './update/property/UpdatePropertyView';
import UpdateSummaryView from './update/summary/UpdateSummaryView';

export interface IViewSelectorProps {
  researchFile: Api_ResearchFile;
  selectedIndex: number;

  isEditMode: boolean;
  setEditMode: (isEditing: boolean) => void;
  setFormikRef: (ref: React.RefObject<FormikProps<any>> | undefined) => void;
  onSuccess: () => void;
}

const ViewSelector: React.FunctionComponent<IViewSelectorProps> = props => {
  if (props.selectedIndex === 0) {
    if (props.isEditMode) {
      return (
        <UpdateSummaryView
          researchFile={props.researchFile}
          setFormikRef={props.setFormikRef}
          onSuccess={props.onSuccess}
        />
      );
    } else {
      return (
        <ResearchTabsContainer researchFile={props.researchFile} setEditMode={props.setEditMode} />
      );
    }
  } else {
    const properties = props.researchFile.researchProperties || [];
    const selectedPropertyIndex = props.selectedIndex - 1;
    const researchFileProperty = properties[selectedPropertyIndex];
    researchFileProperty.researchFile = props.researchFile;
    if (props.isEditMode) {
      return (
        <UpdatePropertyView
          researchFileProperty={researchFileProperty}
          setFormikRef={props.setFormikRef}
          onSuccess={props.onSuccess}
        />
      );
    } else {
      return (
        <PropertyResearchContainer
          researchFileProperty={researchFileProperty}
          setEditMode={props.setEditMode}
        />
      );
    }
  }
};

export default ViewSelector;
