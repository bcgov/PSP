import { FormikProps } from 'formik/dist/types';
import { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { FastDatePicker, ProjectSelector, Select, TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';

import { LeaseFormModel } from '../models';

export interface ILeaseDetailsSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

export const LeaseDetailSubForm: React.FunctionComponent<ILeaseDetailsSubFormProps> = ({
  formikProps,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();

  const { values, setFieldValue } = formikProps;
  const { statusTypeCode, terminationReason, cancellationReason } = values;

  const [currentlLeaseStatus, setLeaseStatus] = useState<string>(statusTypeCode);
  const { setModalContent, setDisplayModal } = useModalContext();

  const leaseStatusTypes = getOptionsByType(API.LEASE_STATUS_TYPES);
  const paymentReceivableTypes = getOptionsByType(API.LEASE_PAYMENT_RECEIVABLE_TYPES);

  useEffect(() => {
    if (statusTypeCode !== currentlLeaseStatus) {
      setLeaseStatus(statusTypeCode);
    }
  }, [currentlLeaseStatus, statusTypeCode]);

  const statusChangeModalContent = (status: string): React.ReactNode => {
    return (
      <>
        <p>
          The lease is no longer in <strong>{status}</strong> state. The reason for doing so will be
          cleared from the file details and can only be viewed in the notes tab.
          <br />
          Do you want to proceed?
        </p>
      </>
    );
  };

  const onLeaseStatusChanged = (newStatus: string): void => {
    if (
      (currentlLeaseStatus === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD ||
        currentlLeaseStatus === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED) &&
      (terminationReason || cancellationReason) &&
      newStatus !== currentlLeaseStatus
    ) {
      setModalContent({
        variant: 'info',
        title: 'Are you sure?',
        message:
          currentlLeaseStatus === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD
            ? statusChangeModalContent('Cancelled')
            : statusChangeModalContent('Terminated'),
        okButtonText: 'Yes',
        handleOk: () => {
          setFieldValue('statusTypeCode', newStatus);
          if (currentlLeaseStatus === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD) {
            setFieldValue('cancellationReason', '');
          } else if (currentlLeaseStatus === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED) {
            setFieldValue('terminationReason', '');
          }
          setDisplayModal(false);
        },
        cancelButtonText: 'No',
        handleCancel: () => {
          setFieldValue('statusTypeCode', currentlLeaseStatus);
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    } else {
      setFieldValue('statusTypeCode', newStatus);
    }
  };

  return (
    <Section>
      <SectionField label="Ministry project" labelWidth="2">
        <ProjectSelector field="project" />
      </SectionField>
      <SectionField label="Status" labelWidth="2" contentWidth="4" required>
        <Select
          placeholder="Select Status"
          field="statusTypeCode"
          value={currentlLeaseStatus}
          options={leaseStatusTypes}
          onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
            const selectedValue = [].slice
              .call(e.target.selectedOptions)
              .map((option: HTMLOptionElement & number) => option.value)[0];
            onLeaseStatusChanged(selectedValue);
          }}
          required
        />
      </SectionField>

      {statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED && (
        <SectionField label="Termination reason" contentWidth="12" required>
          <TextArea field="terminationReason" />
        </SectionField>
      )}

      {statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD && (
        <SectionField label="Cancellation reason" contentWidth="12" required>
          <TextArea field="cancellationReason" />
        </SectionField>
      )}

      <SectionField label="Account type" labelWidth="2" contentWidth="5" required>
        <Select field="paymentReceivableTypeCode" options={paymentReceivableTypes} />
      </SectionField>
      <Row>
        <Col>
          <SectionField label="Start date" required>
            <FastDatePicker formikProps={formikProps} field="startDate" required />
          </SectionField>
        </Col>
        <Col>
          <SectionField label="Expiry date">
            <FastDatePicker formikProps={formikProps} field="expiryDate" />
          </SectionField>
        </Col>
      </Row>
    </Section>
  );
};

export default LeaseDetailSubForm;
