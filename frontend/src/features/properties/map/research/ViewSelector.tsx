import { UpdatePropertyDetailsContainer } from 'features/mapSideBar/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import { FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';

import PropertyResearchContainer from './detail/PropertyResearchContainer';
import { FormKeys } from './FormKeys';
import ResearchTabsContainer from './ResearchTabsContainer';
import UpdatePropertyView from './update/property/UpdatePropertyView';
import UpdateSummaryView from './update/summary/UpdateSummaryView';

export interface IViewSelectorProps {
  researchFile?: Api_ResearchFile;
  selectedIndex: number;

  isEditMode: boolean;
  setEditMode: (isEditing: boolean) => void;
  // The "edit key" identifies which form is currently being edited: e.g.
  //  - property details info,
  //  - research summary,
  //  - research property info
  //  - 'none' means no form is being edited.
  editKey: FormKeys;
  setEditKey: (editKey: FormKeys) => void;

  setFormikRef: (ref: React.RefObject<FormikProps<any>> | undefined) => void;
  onSuccess: () => void;
}

const ViewSelector: React.FunctionComponent<IViewSelectorProps> = props => {
  if (props.selectedIndex === 0) {
    if (props.isEditMode && !!props.researchFile) {
      return (
        <UpdateSummaryView
          researchFile={props.researchFile}
          setFormikRef={props.setFormikRef}
          onSuccess={props.onSuccess}
        />
      );
    } else {
      return (
        <ResearchTabsContainer
          researchFile={props.researchFile}
          setEditMode={props.setEditMode}
          setEditKey={props.setEditKey}
        />
      );
    }
  } else {
    const properties = props.researchFile?.fileProperties || [];
    const selectedPropertyIndex = props.selectedIndex - 1;
    const researchFileProperty = properties[selectedPropertyIndex];
    researchFileProperty.file = props.researchFile;
    if (props.isEditMode) {
      if (props.editKey === FormKeys.propertyDetails) {
        return (
          <UpdatePropertyDetailsContainer
            id={researchFileProperty.property?.id as number}
            onSuccess={props.onSuccess}
            setFormikRef={props.setFormikRef}
          />
        );
      } else {
        return (
          <UpdatePropertyView
            researchFileProperty={researchFileProperty}
            setFormikRef={props.setFormikRef}
            onSuccess={props.onSuccess}
          />
        );
      }
    } else {
      return (
        <PropertyResearchContainer
          researchFileProperty={researchFileProperty}
          setEditMode={props.setEditMode}
          setEditKey={props.setEditKey}
        />
      );
    }
  }
};

export default ViewSelector;
