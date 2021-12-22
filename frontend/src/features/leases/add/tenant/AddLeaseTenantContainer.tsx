import { useFormikContext } from 'formik';
import { IContactSearchResult, IFormLease } from 'interfaces';
import { isEqual } from 'lodash';
import * as React from 'react';
import { useEffect } from 'react';
import { useState } from 'react';
import { useHistory } from 'react-router';
import { Prompt } from 'react-router-dom';

import AddLeaseTenantForm from './AddLeaseTenantForm';

interface IAddLeaseTenantContainerProps {}

export const AddLeaseTenantContainer: React.FunctionComponent<IAddLeaseTenantContainerProps> = () => {
  const formikProps = useFormikContext<IFormLease>();
  const { resetForm, values } = formikProps;
  const [selectedTenants, setSelectedTenants] = useState<IContactSearchResult[]>([]);
  const history = useHistory();
  const onCancel = () => {
    history.push(`/lease/${values.id}/tenant`);
  };

  // if we navigate away from this page successfully, reset the form.
  useEffect(() => {
    return () => {
      resetForm();
    };
  }, [resetForm]);

  return (
    <>
      <Prompt
        when={formikProps.dirty && !isEqual(formikProps.initialValues, formikProps.values)}
        message="You have made changes on this form. Do you wish to leave without saving?"
      />
      <AddLeaseTenantForm
        selectedTenants={selectedTenants}
        setSelectedTenants={setSelectedTenants}
        onCancel={onCancel}
      />
    </>
  );
};

export default AddLeaseTenantContainer;
