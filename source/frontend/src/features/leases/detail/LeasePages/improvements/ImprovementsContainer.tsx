import ProtectedComponent from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { LeasePageProps } from 'features/properties/map/lease/LeaseContainer';
import { Formik, FormikProps } from 'formik';
import { defaultFormLease, IFormLease } from 'interfaces';
import noop from 'lodash/noop';
import * as React from 'react';
import { useContext } from 'react';

import { StyledDetails } from '../details/LeaseDetailsForm';
import { AddImprovementsContainer } from './AddImprovementsContainer';
import { Improvements } from './Improvements';

export const ImprovementsContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps>
> = ({ isEditing, formikRef, onEdit }) => {
  const { lease } = useContext(LeaseStateContext);

  if (!lease?.improvements?.length) {
    return (
      <b className="mr-5">
        If this lease/license includes any commercial, residential or other improvements on the
        property, switch to edit mode to add details to this record.
      </b>
    );
  }

  return !!isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddImprovementsContainer
        formikRef={formikRef as React.RefObject<FormikProps<IFormLease>>}
        onEdit={onEdit}
      />
    </ProtectedComponent>
  ) : (
    <Formik onSubmit={noop} enableReinitialize initialValues={{ ...defaultFormLease, ...lease }}>
      <StyledDetails>
        <Improvements disabled={true} />
      </StyledDetails>
    </Formik>
  );
};

export default ImprovementsContainer;
