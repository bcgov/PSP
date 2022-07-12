import { UpdatePropertyDetailsContainer } from 'features/mapSideBar/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import { FormikProps } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';

import ComposedProperty from './ComposedProperty';
import PropertyContainer from './PropertyContainer';

export interface IPropertyViewSelectorProps {
  isEditMode: boolean;
  setEditMode: (isEditing: boolean) => void;
  setFormikRef: (ref: React.RefObject<FormikProps<any>> | undefined) => void;
  onSuccess: () => void;

  composedProperty: ComposedProperty;
}

const PropertyViewSelector: React.FunctionComponent<IPropertyViewSelectorProps> = props => {
  if (props.isEditMode) {
    return <UpdatePropertyDetailsContainer pid={''} />;
  } else {
    return <PropertyContainer pid={''} />;
  }
};

export default PropertyViewSelector;
