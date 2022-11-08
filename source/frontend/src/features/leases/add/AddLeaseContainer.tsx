import GenericModal from 'components/common/GenericModal';
import { SidebarStateContext } from 'components/layout/SideNavBar/SideNavbarContext';
import { SidebarContextType } from 'components/layout/SideNavBar/SideTray';
import { MapStateContext } from 'components/maps/providers/MapStateContext';
import { AddLeaseLayout, LeaseBreadCrumb, LeaseHeader, LeaseIndex } from 'features/leases';
import { FormikProps } from 'formik';
import { IAddFormLease, ILease } from 'interfaces';
import * as React from 'react';
import { useContext, useRef, useState } from 'react';
import { useHistory } from 'react-router';

import { LeasePageNames, leasePages } from '../detail/LeaseContainer';
import { useAddLease } from '../hooks/useAddLease';
import AddLeaseForm from './AddLeaseForm';

export interface IAddLeaseContainerProps {}

interface IAddLeaseParams {
  lease?: ILease;
  userOverride?: string;
}

export const AddLeaseContainer: React.FunctionComponent<IAddLeaseContainerProps> = props => {
  const leasePage = leasePages.get(LeasePageNames.DETAILS);
  const { setTrayPage } = useContext(SidebarStateContext);
  const onClickManagement = () => setTrayPage(SidebarContextType.LEASE);
  const history = useHistory();
  const { addLease } = useAddLease();
  const [addLeaseParams, setAddLeaseParams] = useState<IAddLeaseParams | undefined>();
  const formikRef = useRef<FormikProps<IAddFormLease>>(null);

  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }
  const onSubmit = async (lease: ILease) => {
    const leaseResponse = await addLease(lease, (userOverrideMessage?: string) =>
      setAddLeaseParams({ lease: lease, userOverride: userOverrideMessage }),
    );
    if (!!leaseResponse?.id) {
      history.push(`/lease/${leaseResponse?.id}`);
    }
  };

  const onCancel = () => {
    history.push('/lease/list');
  };

  return (
    <MapStateContext.Consumer>
      {({ selectedInventoryProperty }) => (
        <>
          <AddLeaseLayout>
            <LeaseBreadCrumb leasePage={leasePage} onClickManagement={onClickManagement} />
            <LeaseHeader />
            <LeaseIndex currentPageName={LeasePageNames.DETAILS}></LeaseIndex>
            <AddLeaseForm
              propertyInfo={selectedInventoryProperty}
              onCancel={onCancel}
              onSubmit={onSubmit}
              formikRef={formikRef}
            />
            <GenericModal
              title="Warning"
              display={!!addLeaseParams}
              message={addLeaseParams?.userOverride}
              handleOk={async () => {
                if (!!addLeaseParams?.lease) {
                  const leaseResponse = await addLease(addLeaseParams.lease, undefined, true);
                  setAddLeaseParams(undefined);
                  history.push(`/lease/${leaseResponse?.id}`);
                }
              }}
              handleCancel={() => setAddLeaseParams(undefined)}
              okButtonText="Save Anyways"
              okButtonVariant="warning"
              cancelButtonText="Cancel"
            />
          </AddLeaseLayout>
        </>
      )}
    </MapStateContext.Consumer>
  );
};

export default AddLeaseContainer;
