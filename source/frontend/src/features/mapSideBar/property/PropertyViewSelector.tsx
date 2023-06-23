import { FormikProps } from 'formik';
import * as React from 'react';

import { UpdatePropertyDetailsContainer } from '@/features/mapSideBar/property/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import ComposedPropertyState from '@/hooks/repositories/useComposedProperties';

import PropertyContainer from './PropertyContainer';

export interface IPropertyViewSelectorProps {
  isEditMode: boolean;
  setEditMode: (isEditing: boolean) => void;
  onSuccess: () => void;
  composedPropertyState: ComposedPropertyState;
}

const PropertyViewSelector = React.forwardRef<FormikProps<any>, IPropertyViewSelectorProps>(
  (props, formikRef) => {
    if (props.isEditMode && !!props?.composedPropertyState?.apiWrapper?.response?.id) {
      return (
        <UpdatePropertyDetailsContainer
          id={props.composedPropertyState?.apiWrapper?.response?.id}
          onSuccess={props.onSuccess}
          ref={formikRef}
        />
      );
    } else {
      return (
        <PropertyContainer
          composedPropertyState={props.composedPropertyState}
          setEditMode={props.setEditMode}
        />
      );
    }
  },
);

export default PropertyViewSelector;
