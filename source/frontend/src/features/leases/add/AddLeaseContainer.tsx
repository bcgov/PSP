import GenericModal from 'components/common/GenericModal';
import { SidebarStateContext } from 'components/layout/SideNavBar/SideNavbarContext';
import { SidebarContextType } from 'components/layout/SideNavBar/SideTray';
import { MapStateActionTypes, MapStateContext } from 'components/maps/providers/MapStateContext';
import { AddLeaseLayout, LeaseBreadCrumb, LeaseHeader, LeaseIndex } from 'features/leases';
import { FormikProps } from 'formik';
import { Api_Lease } from 'models/api/Lease';
import * as React from 'react';
import { useContext, useRef, useState } from 'react';
import { useHistory } from 'react-router';

import { LeasePageNames, leasePages } from '../detail/LeaseContainer';
import { useAddLease } from '../hooks/useAddLease';
import { FormLease } from '../models';
import AddLeaseForm from './AddLeaseForm';

export interface IAddLeaseContainerProps {}

export const AddLeaseContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseContainerProps>
> = props => {
  const leasePage = leasePages.get(LeasePageNames.DETAILS);
  const { setTrayPage } = useContext(SidebarStateContext);
  const { setState } = useContext(MapStateContext);
  const onClickManagement = () => setTrayPage(SidebarContextType.LEASE);
  const history = useHistory();
  const { addLease } = useAddLease();
  const [addLeaseParams, setAddLeaseParams] = useState<
    { lease: Api_Lease; userOverride?: string } | undefined
  >();
  const formikRef = useRef<FormikProps<FormLease>>(null);

  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }
  const onSubmit = async (lease: Api_Lease) => {
    const leaseResponse = await addLease(lease, (userOverrideMessage?: string) =>
      setAddLeaseParams({ lease: lease, userOverride: userOverrideMessage }),
    );
    if (!!leaseResponse?.id) {
      history.push(`/lease/${leaseResponse?.id}`);
    }
  };

  // when leaving this page, reset any selected lease properties to ensure that future uses of this page do not include previously selected values.
  React.useEffect(() => {
    return () => {
      setState({
        type: MapStateActionTypes.SELECTED_LEASE_PROPERTY,
        selectedLeaseProperty: null,
      });
    };
  }, [setState]);

  const onCancel = () => {
    history.push('/lease/list');
  };

  return (
    <MapStateContext.Consumer>
      {({ selectedLeaseProperty }) => (
        <>
          <AddLeaseLayout>
            <LeaseBreadCrumb leasePage={leasePage} onClickManagement={onClickManagement} />
            <LeaseHeader />
            <LeaseIndex currentPageName={LeasePageNames.DETAILS}></LeaseIndex>
            <AddLeaseForm
              propertyInfo={selectedLeaseProperty}
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
                  if (!!leaseResponse?.id) {
                    history.push(`/lease/${leaseResponse?.id}`);
                  }
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
