import { FormikProps } from 'formik';
import { ILease } from 'interfaces';
import React from 'react';

import { LeaseTabsContainer } from './detail/LeaseTabsContainer';
import { LeaseContainerState } from './LeaseContainer';

export interface IViewSelectorProps {
  lease?: ILease;
  isEditing: boolean;
  setContainerState: (value: Partial<LeaseContainerState>) => void;
}

export const ViewSelector = React.forwardRef<FormikProps<any>, IViewSelectorProps>(
  (props, formikRef) => {
    // TODO: render edit forms
    // TODO: use formikRef prop to pass as a ref to formik forms when edit forms get implemented
    if (props.isEditing && !!props.lease) {
      return null;
    } else {
      // render read-only views
      return <LeaseTabsContainer lease={props.lease} />;
    }
  },
);

export default ViewSelector;
