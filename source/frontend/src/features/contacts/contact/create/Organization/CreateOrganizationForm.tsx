import { Formik, FormikHelpers, FormikProps, getIn } from 'formik';
import { useCallback, useMemo, useRef, useState } from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';

import { Button } from '@/components/common/buttons/Button';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import { TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { FlexBox } from '@/components/common/styles';
import {
  Address,
  DuplicateContactModal,
  useAddressHelpers,
} from '@/features/contacts/contact/create/components';
import * as Styled from '@/features/contacts/contact/create/styles';
import { IEditableOrganizationForm } from '@/features/contacts/formModels';
import useAddContact from '@/features/contacts/hooks/useAddContact';
import { isValidId } from '@/utils';

import OrganizationSubForm from '../../Organization/OrganizationSubForm';
import { onValidateOrganization } from '../../utils/contactUtils';

/**
 * Formik-connected form to Create Organizational Contacts
 */
export const CreateOrganizationForm: React.FunctionComponent<unknown> = () => {
  const history = useHistory();
  const { addOrganization } = useAddContact();

  const [showDuplicateModal, setShowDuplicateModal] = useState(false);
  const [allowDuplicate, setAllowDuplicate] = useState(false);

  // validation needs to be adjusted when country == OTHER
  const { otherCountryId } = useAddressHelpers();

  const formikRef = useRef<FormikProps<IEditableOrganizationForm>>(null);

  const onSubmit = async (
    formOrganization: IEditableOrganizationForm,
    { setSubmitting }: FormikHelpers<IEditableOrganizationForm>,
  ) => {
    try {
      setShowDuplicateModal(false);

      const newOrganization = formOrganization.formOrganizationToApiOrganization();
      const organizationResponse = await addOrganization(
        newOrganization,
        setShowDuplicateModal,
        allowDuplicate,
      );

      if (isValidId(organizationResponse?.id)) {
        history.push(`/contact/O${organizationResponse?.id}`);
      }
    } finally {
      setSubmitting(false);
    }
  };

  const saveDuplicate = async () => {
    setAllowDuplicate(true);
    if (formikRef.current) {
      formikRef.current.handleSubmit();
    }
  };

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  return (
    <>
      <Formik<IEditableOrganizationForm>
        component={CreateOrganizationComponent}
        initialValues={new IEditableOrganizationForm()}
        validate={(values: IEditableOrganizationForm) =>
          onValidateOrganization(values, otherCountryId)
        }
        onSubmit={onSubmit}
        innerRef={formikRef}
      />
      <DuplicateContactModal
        variant="warning"
        display={showDuplicateModal}
        handleOk={() => saveDuplicate()}
        handleCancel={() => {
          setAllowDuplicate(false);
          setShowDuplicateModal(false);
        }}
      />
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </>
  );
};

export default CreateOrganizationForm;

/**
 * Sub-component that is wrapped by Formik
 */
const CreateOrganizationComponent: React.FC<FormikProps<IEditableOrganizationForm>> = ({
  errors,
  touched,
  submitForm,
}) => {
  const history = useHistory();

  const onCancel = () => {
    history.push('/contact/list');
  };

  const isContactMethodInvalid = useMemo(() => {
    return (
      !!touched.phoneContactMethods &&
      !!touched.emailContactMethods &&
      (!!touched.mailingAddress?.streetAddress1 ||
        !!touched.propertyAddress?.streetAddress1 ||
        !!touched.billingAddress?.streetAddress1) &&
      getIn(errors, 'needsContactMethod')
    );
  }, [touched, errors]);

  return (
    <>
      <Styled.CreateFormLayout>
        <Styled.Form id="createForm">
          <FlexBox column>
            <OrganizationSubForm isContactMethodInvalid={isContactMethodInvalid} />
            <Section header="Mailing Address" isCollapsable initiallyExpanded>
              <Address namespace="mailingAddress" />
            </Section>
            <Section header="Property Address" isCollapsable initiallyExpanded>
              <Address namespace="propertyAddress" />
            </Section>
            <Section header="Billing Address" isCollapsable initiallyExpanded>
              <Address namespace="billingAddress" />
            </Section>
            <Section header="Comments">
              <TextArea rows={5} field="comment" />
            </Section>
          </FlexBox>
        </Styled.Form>
      </Styled.CreateFormLayout>

      <Styled.ButtonGroup>
        {Object.keys(touched).length > 0 && Object.keys(errors).length > 0 ? (
          <Styled.ErrorMessage gap="0.5rem" className="mr-3">
            <AiOutlineExclamationCircle size="1.8rem" className="mt-1" />
            <span>Please complete required fields</span>
          </Styled.ErrorMessage>
        ) : null}
        <Button variant="secondary" onClick={onCancel}>
          Cancel
        </Button>
        <Button onClick={submitForm}>Save</Button>
      </Styled.ButtonGroup>
    </>
  );
};
