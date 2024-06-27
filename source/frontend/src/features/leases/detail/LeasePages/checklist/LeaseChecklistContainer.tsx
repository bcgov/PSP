import { FormikProps } from 'formik';
import React from 'react';

import ProtectedComponent from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants';
import * as API from '@/constants/API';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { ChecklistView } from '@/features/mapSideBar/shared/tabs/checklist/detail/ChecklistView';
import { ChecklistFormModel } from '@/features/mapSideBar/shared/tabs/checklist/update/models';
import { UpdateChecklistForm } from '@/features/mapSideBar/shared/tabs/checklist/update/UpdateChecklistForm';

import UpdateLeaseChecklistContainer from './update/UpdateLeaseChecklistContainer';

const LeaseChecklistContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps<void>>
> = ({ isEditing, formikRef, onEdit, onSuccess }) => {
  const { lease } = React.useContext(LeaseStateContext);

  return !!isEditing && !!onEdit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <UpdateLeaseChecklistContainer
        View={UpdateChecklistForm}
        formikRef={formikRef as unknown as React.RefObject<FormikProps<ChecklistFormModel>>}
        onSuccess={onSuccess}
      />
    </ProtectedComponent>
  ) : (
    lease && (
      <ChecklistView
        apiFile={lease}
        showEditButton={false}
        onEdit={null}
        sectionTypeName={API.LEASE_CHECKLIST_SECTION_TYPES}
        editClaim={Claims.LEASE_EDIT}
        prefix="lease"
      />
    )
  );
};

export default LeaseChecklistContainer;
