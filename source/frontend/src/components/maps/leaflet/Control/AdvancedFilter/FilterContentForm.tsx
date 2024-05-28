import { Form, Formik, useFormikContext } from 'formik';
import noop from 'lodash/noop';
import React, { useEffect, useMemo } from 'react';
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image';
import Row from 'react-bootstrap/Row';
import styled from 'styled-components';

import DisposedPng from '@/assets/images/pins/disposed.png';
import PropertyOfInterestPng from '@/assets/images/pins/land-poi.png';
import CoreInventoryPng from '@/assets/images/pins/land-reg.png';
import OtherInterestPng from '@/assets/images/pins/other-interest.png';
import RetiredPng from '@/assets/images/pins/retired.png';
import { Check, Select, SelectOption } from '@/components/common/form';
import { Multiselect } from '@/components/common/form/Multiselect';
import { ProjectSelector } from '@/components/common/form/ProjectSelector/ProjectSelector';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

import { CodeTypeSelectOption, PropertyFilterFormModel } from './models';

interface IFormObserverProps {
  onChange: (model: PropertyFilterFormModel) => void;
}

const FormObserver: React.FC<IFormObserverProps> = ({ onChange }) => {
  const { values } = useFormikContext<PropertyFilterFormModel>();
  useEffect(() => {
    onChange(values);
  }, [onChange, values]);

  return null;
};

export interface IFilterContentFormProps {
  onChange: (model: PropertyFilterFormModel) => void;
  isLoading: boolean;
}

export const FilterContentForm: React.FC<React.PropsWithChildren<IFilterContentFormProps>> = ({
  onChange,
  isLoading,
}) => {
  const initialFilter = useMemo(() => {
    return new PropertyFilterFormModel();
  }, []);

  useEffect(() => {
    const firstLoad = () => {
      onChange(initialFilter);
    };
    firstLoad();
  }, [initialFilter, onChange]);

  const { getByType } = useLookupCodeHelpers();

  // Property options
  const anomalyOptions = getByType(API.PROPERTY_ANOMALY_TYPES).map<CodeTypeSelectOption>(x => {
    return { codeType: x.id.toString(), codeTypeDescription: x.name };
  });

  // Tenure options
  const tenureStatusOptions = getByType(API.PROPERTY_TENURE_TYPES).map<CodeTypeSelectOption>(x => {
    return { codeType: x.id.toString(), codeTypeDescription: x.name };
  });

  const tenureProvincePublicHigwayTypeOptions = getByType(API.PPH_STATUS_TYPES).map<SelectOption>(
    x => {
      return { value: x.id.toString(), label: x.name };
    },
  );

  const tenureRoadTypeOptions = getByType(API.PROPERTY_ROAD_TYPES).map<CodeTypeSelectOption>(x => {
    return { codeType: x.id.toString(), codeTypeDescription: x.name };
  });

  // Lease options
  const leaseStatusOptions = getByType(API.LEASE_STATUS_TYPES).map<SelectOption>(x => {
    return { value: x.id.toString(), label: x.name };
  });

  const leaseTypeOptions = getByType(API.LEASE_TYPES).map<CodeTypeSelectOption>(x => {
    return { codeType: x.id.toString(), codeTypeDescription: x.name };
  });

  const leasePurposeOptions = getByType(API.LEASE_PURPOSE_TYPES).map<CodeTypeSelectOption>(x => {
    return { codeType: x.id.toString(), codeTypeDescription: x.name };
  });

  const leasePaymentRcvblOptions = getByType(API.LEASE_PAYMENT_RECEIVABLE_TYPES).map<SelectOption>(
    x => {
      return { value: x.id.toString(), label: x.name };
    },
  );
  leasePaymentRcvblOptions.push({ value: 'all', label: 'Payable and Receivable' });

  return (
    <Formik<PropertyFilterFormModel> initialValues={initialFilter} onSubmit={noop}>
      <Form>
        <FormObserver onChange={onChange} />
        <LoadingBackdrop show={isLoading} parentScreen />
        <Section header="Project" isCollapsable initiallyExpanded>
          <SectionField label={null} contentWidth="12">
            <ProjectSelector field="projectPrediction" />
          </SectionField>
        </Section>
        <Section header="Tenure" isCollapsable initiallyExpanded>
          <SectionField label="Status" contentWidth="12">
            <Multiselect
              field="tenureStatuses"
              displayValue="codeTypeDescription"
              placeholder=""
              hidePlaceholder
              options={tenureStatusOptions}
            />
          </SectionField>
          <SectionField label="Province Public Highway" labelWidth="12" contentWidth="12">
            <Select
              field="tenurePPH"
              options={tenureProvincePublicHigwayTypeOptions}
              placeholder="Select a highway"
            />
          </SectionField>

          <SectionField label="Highway / Road Details" labelWidth="12" contentWidth="12">
            <Multiselect
              field="tenureRoadTypes"
              displayValue="codeTypeDescription"
              placeholder=""
              hidePlaceholder
              options={tenureRoadTypeOptions}
            />
          </SectionField>
        </Section>
        <Section header="Lease / License" isCollapsable initiallyExpanded>
          <SectionField
            label="Lease Transaction"
            contentWidth="12"
            tooltip="Selecting the Payable and Receivable lease transaction option will display properties that have both a payable and a receivable lease on them."
          >
            <Select
              field="leasePayRcvblType"
              placeholder="Select Lease Transaction"
              options={leasePaymentRcvblOptions}
              data-testid="leasePayRcvblType"
            />
          </SectionField>
          <SectionField label="Status" contentWidth="12">
            <Select
              field="leaseStatus"
              options={leaseStatusOptions}
              placeholder="Select a Lease Status"
            />
          </SectionField>
          <SectionField label="Type(s)" contentWidth="12">
            <Multiselect
              field="leaseTypes"
              displayValue="codeTypeDescription"
              placeholder=""
              hidePlaceholder
              options={leaseTypeOptions}
            />
          </SectionField>
          <SectionField label="Purpose(s)" contentWidth="12">
            <Multiselect
              field="leasePurposes"
              displayValue="codeTypeDescription"
              placeholder=""
              hidePlaceholder
              options={leasePurposeOptions}
            />
          </SectionField>
        </Section>
        <Section header="Anomaly" isCollapsable initiallyExpanded>
          <SectionField label={null} contentWidth="12">
            <Multiselect
              field="anomalies"
              displayValue="codeTypeDescription"
              placeholder=""
              hidePlaceholder
              options={anomalyOptions}
            />
          </SectionField>
        </Section>
        <Section header="Show Ownership" isCollapsable initiallyExpanded>
          <SectionField label={null} contentWidth="12">
            <Row>
              <Col xs={1}>
                <Check field="isCoreInventory" />
              </Col>
              <Col xs={1}>
                <Image height={36} src={CoreInventoryPng} />
              </Col>
              <Col className="pl-5">
                <StyledSpan className="mb-3">Core Inventory</StyledSpan>
              </Col>
            </Row>
            <Row className="pt-4">
              <Col xs={1}>
                <Check field="isPropertyOfInterest" />
              </Col>
              <Col xs={1}>
                <Image height={36} src={PropertyOfInterestPng} />
              </Col>
              <Col className="pl-5">
                <StyledSpan className="mb-3">Property of Interest</StyledSpan>
              </Col>
            </Row>
            <Row className="pt-4">
              <Col xs={1}>
                <Check field="isOtherInterest" />
              </Col>
              <Col xs={1}>
                <Image height={36} src={OtherInterestPng} />
              </Col>
              <Col className="pl-5">
                <StyledSpan className="mb-3">Other Interest</StyledSpan>
              </Col>
            </Row>
            <Row className="pt-4">
              <Col xs={1}>
                <Check field="isDisposed" />
              </Col>
              <Col xs={1}>
                <Image height={36} src={DisposedPng} />
              </Col>
              <Col className="pl-5">
                <StyledSpan className="mb-3">Disposed</StyledSpan>
              </Col>
            </Row>
            <Row className="pt-4">
              <Col xs={1}>
                <Check field="isRetired" />
              </Col>
              <Col xs={1}>
                <Image height={36} src={RetiredPng} />
              </Col>
              <Col className="pl-5">
                <StyledSpan className="mb-3">Retired</StyledSpan>
              </Col>
            </Row>
          </SectionField>
        </Section>
      </Form>
    </Formik>
  );
};

const StyledSpan = styled.span`
  color: ${props => props.theme.css.textColor};
`;
