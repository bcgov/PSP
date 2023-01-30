import { LeaseFormModel } from 'features/leases/models';
import { LeaseFileTabNames } from 'features/mapSideBar/tabs/LeaseFileTabs';
import { FormikProps } from 'formik';
import { IFormLease, ILease } from 'interfaces';
import * as React from 'react';

import { LeaseTabsContainer } from './detail/LeaseTabsContainer';
import { LeaseContainerState, LeasePageNames, leasePages } from './LeaseContainer';

export interface IViewSelectorProps {
  lease?: ILease;
  isEditing: boolean;
  setContainerState: (value: Partial<LeaseContainerState>) => void;
  refreshLease: () => void;
  setLease: (lease: ILease) => void;
  activeEditForm?: LeasePageNames;
  activeTab?: LeaseFileTabNames;
  formikRef: React.RefObject<FormikProps<LeaseFormModel | IFormLease>>;
}

export const ViewSelector: React.FunctionComponent<IViewSelectorProps> = props => {
  if (props.isEditing && !!props.lease && props.activeEditForm) {
    const activeLeasePage = leasePages.get(props.activeEditForm);
    if (!activeLeasePage) {
      throw Error('Lease page not found');
    }
    const Component = activeLeasePage.component;
    return (
      <Component
        isEditing={props.isEditing}
        onEdit={(isEditing: boolean) => props.setContainerState({ isEditing: isEditing })}
        formikRef={props.formikRef}
      />
    );
  } else {
    // render read-only views
    return (
      <LeaseTabsContainer
        lease={props.lease}
        refreshLease={props.refreshLease}
        setLease={props.setLease}
        setContainerState={props.setContainerState}
        activeTab={props.activeTab}
        isEditing={props.isEditing}
        formikRef={props.formikRef}
      />
    );
  }
};

export default ViewSelector;
