import {
  InventoryTabNames,
  InventoryTabs,
  TabInventoryView,
} from 'features/mapSideBar/tabs/InventoryTabs';
import { UpdatePropertyDetailsContainer } from 'features/mapSideBar/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import PropertyResearchTabView from 'features/mapSideBar/tabs/propertyResearch/PropertyResearchTabView';
import { FormikProps } from 'formik';
import noop from 'lodash/noop';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';

import PropertyFileContainer from '../shared/detail/PropertyFileContainer';
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
  onSuccess: () => void;
}

const ViewSelector = React.forwardRef<FormikProps<any>, IViewSelectorProps>((props, formikRef) => {
  if (props.selectedIndex === 0) {
    if (props.isEditMode && !!props.researchFile) {
      return (
        <UpdateSummaryView
          researchFile={props.researchFile}
          ref={formikRef}
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
            ref={formikRef}
          />
        );
      } else {
        return (
          <UpdatePropertyView
            researchFileProperty={researchFileProperty}
            onSuccess={props.onSuccess}
            ref={formikRef}
          />
        );
      }
    } else {
      const researchPropertyTab: TabInventoryView = {
        content: (
          <PropertyResearchTabView
            researchFile={researchFileProperty}
            setEditMode={(editable: boolean) => {
              props.setEditMode(editable);
              props.setEditKey(FormKeys.propertyResearch);
            }}
          />
        ),
        key: InventoryTabNames.research,
        name: 'Property Research',
      };

      return (
        <PropertyFileContainer
          fileProperty={researchFileProperty}
          setEditFileProperty={() => {
            props.setEditKey(FormKeys.propertyDetails);
            props.setEditMode(true);
          }}
          setEditTakes={noop}
          defaultTab={InventoryTabNames.research}
          customTabs={[researchPropertyTab]}
          View={InventoryTabs}
        />
      );
    }
  }
});

export default ViewSelector;
