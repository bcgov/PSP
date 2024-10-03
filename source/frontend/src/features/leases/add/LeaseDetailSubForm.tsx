import { FormikProps } from 'formik/dist/types';
import { useCallback, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import {
  FastDatePicker,
  ProjectSelector,
  Select,
  SelectOption,
  TextArea,
} from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as API from '@/constants/API';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { isValidId } from '@/utils';

import { LeaseFormModel } from '../models';

export interface ILeaseDetailsSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

export const LeaseDetailSubForm: React.FunctionComponent<ILeaseDetailsSubFormProps> = ({
  formikProps,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const { retrieveProjectProducts } = useProjectProvider();

  const { values, setFieldValue } = formikProps;
  const { statusTypeCode, terminationReason, cancellationReason, project } = values;

  const { setModalContent, setDisplayModal } = useModalContext();

  const leaseStatusTypes = getOptionsByType(API.LEASE_STATUS_TYPES);
  const paymentReceivableTypes = getOptionsByType(API.LEASE_PAYMENT_RECEIVABLE_TYPES);

  const [projectProducts, setProjectProducts] = useState<ApiGen_Concepts_Product[] | undefined>(
    undefined,
  );

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
      (statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD ||
        statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED) &&
      (terminationReason || cancellationReason)
    ) {
      setModalContent({
        variant: 'info',
        title: 'Are you sure?',
        message:
          statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD
            ? statusChangeModalContent('Cancelled')
            : statusChangeModalContent('Terminated'),
        okButtonText: 'Yes',
        handleOk: () => {
          setFieldValue('statusTypeCode', newStatus);
          if (statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD) {
            setFieldValue('cancellationReason', '');
          } else if (statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED) {
            setFieldValue('terminationReason', '');
            setFieldValue('terminationDate', '');
          }
          setDisplayModal(false);
        },
        cancelButtonText: 'No',
        handleCancel: () => {
          setFieldValue('statusTypeCode', statusTypeCode);
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    } else {
      setFieldValue('statusTypeCode', newStatus);
    }
  };

  const onMinistryProjectSelected = useCallback(
    async (param: IAutocompletePrediction[]) => {
      if (param.length > 0) {
        if (isValidId(param[0].id)) {
          const result = await retrieveProjectProducts(param[0].id);
          if (result !== undefined) {
            setProjectProducts(result);
          }
        }
      } else {
        setProjectProducts(undefined);
      }
    },
    [retrieveProjectProducts],
  );

  useEffect(() => {
    if (project !== undefined) {
      onMinistryProjectSelected([project]);
    }
  }, [onMinistryProjectSelected, project]);

  return (
    <Section header="Original Agreement">
      <SectionField label="Ministry project" labelWidth="3">
        <ProjectSelector
          field="project"
          onChange={(vals: IAutocompletePrediction[]) => {
            onMinistryProjectSelected(vals);
            if (vals.length === 0) {
              formikProps.setFieldValue('productId', null);
            }
          }}
        />
      </SectionField>
      {projectProducts !== undefined && (
        <SectionField label="Product" labelWidth="3">
          <Select
            field="productId"
            options={projectProducts.map<SelectOption>(x => {
              return { label: x.code + ' ' + x.description || '', value: x.id || 0 };
            })}
            placeholder="Select..."
          />
        </SectionField>
      )}
      <SectionField
        label="Status"
        labelWidth="3"
        contentWidth="4"
        tooltip={
          <TooltipIcon
            toolTipId="lease-status-tooltip"
            toolTip={
              <ul>
                <li>Draft: In progress but not finalized.</li>
                <li>
                  Active: Finalized and all requirements met. Lease/Licence being actively managed.
                </li>
                <li>
                  Terminated: The expiry date of the last agreement if by effluxion of time or the
                  early termination date for cause.
                </li>
                <li>Cancelled: Request cancelled by requestor or MOTI.</li>
                <li>Duplicate: Duplicate file created by accident or data transfer.</li>
                <li>Hold: Agreement in progress but will not be immediately addressed.</li>
                <li>Archived: File to be archived as per ARCS/ORCS.</li>
              </ul>
            }
            placement="right"
          ></TooltipIcon>
        }
        required
      >
        <Select
          placeholder="Select Status"
          field="statusTypeCode"
          value={statusTypeCode}
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

      {statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD && (
        <SectionField label="Cancellation reason" contentWidth="12" required>
          <TextArea field="cancellationReason" />
        </SectionField>
      )}

      <SectionField label="Account type" labelWidth="3" contentWidth="5" required>
        <Select field="paymentReceivableTypeCode" options={paymentReceivableTypes} />
      </SectionField>
      <Row>
        <Col>
          <SectionField
            label="Commencement"
            labelWidth="6"
            required={statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE}
            tooltip={
              <TooltipIcon
                toolTipId="lease-commencement-tooltip"
                toolTip="The start date defined in the original agreement"
                placement="right"
              />
            }
          >
            <FastDatePicker
              formikProps={formikProps}
              field="startDate"
              required={statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE}
            />
          </SectionField>
        </Col>
        <Col>
          <SectionField
            label="Expiry"
            required={statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE}
            tooltip={
              <TooltipIcon
                toolTipId="lease-expiry-tooltip"
                toolTip="The end date specified in the original agreement"
                placement="right"
              />
            }
          >
            <FastDatePicker formikProps={formikProps} field="expiryDate" />
          </SectionField>
        </Col>
      </Row>
      {statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED && (
        <>
          <SectionField
            label="Termination"
            labelWidth="3"
            tooltip={
              <TooltipIcon
                toolTipId="lease-termination-tooltip"
                toolTip="The expiry date of the last agreement if by effluxion of time or the early termination date for cause"
                placement="right"
              />
            }
          >
            <FastDatePicker formikProps={formikProps} field="terminationDate" />
          </SectionField>
          <SectionField label="Termination reason" contentWidth="12" required>
            <TextArea field="terminationReason" />
          </SectionField>
        </>
      )}
    </Section>
  );
};

export default LeaseDetailSubForm;
