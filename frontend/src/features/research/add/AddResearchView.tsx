import GenericModal from 'components/common/GenericModal';
import { SidebarStateContext } from 'components/layout/SideNavBar/SideNavbarContext';
import { SidebarContextType } from 'components/layout/SideNavBar/SideTray';
import { AddLeaseLayout, LeaseBreadCrumb, LeaseHeader, LeaseIndex } from 'features/leases';
import { FormikProps } from 'formik';
import { IAddFormLease, ILease } from 'interfaces';
import * as React from 'react';
import { useContext, useRef, useState } from 'react';
import { useHistory } from 'react-router';
import { useAddResearch } from '../hooks/useAddResearch';

export interface IAddResearchViewProps {}

interface IResearchParams {
  lease?: ILease;
  userOverride?: string;
}

export const AddResearchView: React.FunctionComponent<IAddResearchViewProps> = props => {
  const history = useHistory();
  const { addResearch } = useAddResearch();
  const [addLeaseParams, setAddLeaseParams] = useState<IResearchParams | undefined>();
  const formikRef = useRef<FormikProps<IAddFormLease>>(null);

  const onSubmit = async (lease: ILease) => {
    const leaseResponse = await addResearch(lease, (userOverrideMessage?: string) =>
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
    <>
      <AddLeaseLayout>
        <LeaseHeader />
        <AddLeaseForm onCancel={onCancel} onSubmit={onSubmit} formikRef={formikRef} />
        <GenericModal
          title="Warning"
          display={!!addLeaseParams}
          message={addLeaseParams?.userOverride}
          handleOk={async () => {
            if (!!addLeaseParams?.lease) {
              const addRResponse = await addResearch(addLeaseParams.lease, undefined, true);
              setAddLeaseParams(undefined);
              history.push(`/research/${addRResponse?.id}`);
            }
          }}
          handleCancel={() => setAddLeaseParams(undefined)}
          okButtonText="Save Anyways"
          okButtonVariant="warning"
          cancelButtonText="Cancel"
        />
      </AddLeaseLayout>
    </>
  );
};

export default AddLeaseContainer;
