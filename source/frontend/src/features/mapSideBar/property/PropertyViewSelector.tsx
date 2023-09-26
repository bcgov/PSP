import { FormikProps } from 'formik';
import React, { useEffect, useState } from 'react';

import { UpdatePropertyDetailsContainer } from '@/features/mapSideBar/property/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import ComposedPropertyState from '@/hooks/repositories/useComposedProperties';

import PropertyContainer from './PropertyContainer';
import { PropertyContactEditContainer } from './tabs/propertyDetailsManagement/update/PropertyContactEditContainer';
import { PropertyContactEditForm } from './tabs/propertyDetailsManagement/update/PropertyContactEditForm';
import { PropertyManagementUpdateContainer } from './tabs/propertyDetailsManagement/update/summary/PropertyManagementUpdateContainer';
import { PropertyManagementUpdateForm } from './tabs/propertyDetailsManagement/update/summary/PropertyManagementUpdateForm';

export enum PropertyEditForms {
  UpdatePropertyDetailsContainer = 'UpdatePropertyDetailsContainer',
  UpdateManagementSummaryContainer = 'UpdateManagementSummaryContainer',
  UpdateContactContainer = 'UpdateContactContainer',
}

export interface EditManagementState {
  form: PropertyEditForms;
  childId: number | null;
}

export interface IPropertyViewSelectorProps {
  isEditMode: boolean;
  setEditMode: (isEditing: boolean) => void;
  onSuccess: () => void;
  composedPropertyState: ComposedPropertyState;
}

const PropertyViewSelector = React.forwardRef<FormikProps<any>, IPropertyViewSelectorProps>(
  (props, formikRef) => {
    const [editManagementState, setEditManagementState] = useState<EditManagementState | null>(
      null,
    );

    useEffect(() => {
      if (editManagementState === null) {
        props.setEditMode(false);
      } else {
        props.setEditMode(true);
      }
    }, [editManagementState, props, props.setEditMode]);

    useEffect(() => {
      if (props.isEditMode === false) {
        setEditManagementState(null);
      }
    }, [props, props.isEditMode]);

    if (props.isEditMode && !!props?.composedPropertyState?.apiWrapper?.response?.id) {
      switch (editManagementState?.form) {
        case PropertyEditForms.UpdatePropertyDetailsContainer:
          return (
            <UpdatePropertyDetailsContainer
              id={props.composedPropertyState?.apiWrapper?.response?.id}
              onSuccess={() => {
                setEditManagementState(null);
                props.onSuccess();
              }}
              ref={formikRef}
            />
          );
        case PropertyEditForms.UpdateManagementSummaryContainer:
          return (
            <PropertyManagementUpdateContainer
              propertyId={props.composedPropertyState?.apiWrapper?.response?.id}
              View={PropertyManagementUpdateForm}
              onSuccess={() => {
                setEditManagementState(null);
                props.onSuccess();
              }}
              ref={formikRef}
              onSuccess={props.onSuccess}
              ref={formikRef}
            />
          );
        case PropertyEditForms.UpdateContactContainer:
          return (
            <PropertyContactEditContainer
              propertyId={props.composedPropertyState?.apiWrapper?.response?.id}
              contactId={editManagementState.childId ?? 0}
              View={PropertyContactEditForm}
              onSuccess={() => {
                setEditManagementState(null);
                props.onSuccess();
              }}
              ref={formikRef}
            />
          );
        default:
          return <></>;
      }
    } else {
      return (
        <PropertyContainer
          composedPropertyState={props.composedPropertyState}
          setEditManagementState={setEditManagementState}
        />
      );
    }
  },
);

export default PropertyViewSelector;
