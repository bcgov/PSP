import { Button } from 'components/common/buttons';
import { Form, Input, Select, TextArea } from 'components/common/form';
import TooltipWrapper from 'components/common/TooltipWrapper';
import * as API from 'constants/API';
import { DISCLAIMER_URL, PRIVACY_POLICY_URL } from 'constants/strings';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { Api_AccessRequest } from 'models/api/AccessRequest';
import * as React from 'react';
import { ButtonToolbar, Col, Row } from 'react-bootstrap';
import { FaInfoCircle } from 'react-icons/fa';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';
import { AccessRequestSchema } from 'utils/YupSchema';

import { AccessRequestForm as AccessRequestFormModel } from './models';
import RolesToolTip from './RolesToolTip';

interface IAccessRequestFormProps {
  initialValues: any;
  addAccessRequest: (accessRequest: Api_AccessRequest) => Promise<Api_AccessRequest | undefined>;
  onCancel?: () => void;
}

export const AccessRequestForm: React.FunctionComponent<IAccessRequestFormProps> = ({
  initialValues,
  addAccessRequest,
  onCancel,
}) => {
  const { getPublicByType, getOptionsByType } = useLookupCodeHelpers();
  const roles = getPublicByType(API.ROLE_TYPES);
  const selectRegions = getOptionsByType(API.REGION_TYPES).filter(
    region => region.label !== 'Cannot determine',
  );
  const selectRoles = roles.map(c => mapLookupCode(c, initialValues?.role?.id));
  return (
    <Formik<AccessRequestFormModel>
      enableReinitialize={true}
      initialValues={initialValues}
      validationSchema={AccessRequestSchema}
      onSubmit={async (values, { setSubmitting }) => {
        try {
          await addAccessRequest(values.toApi());
        } catch (error) {}
        setSubmitting(false);
      }}
    >
      <Form className="userInfo">
        <SectionField label="IDIR/BCeID" wideScreen>
          <Input
            field="businessIdentifierValue"
            placeholder={initialValues?.user?.businessIdentifierValue}
            readOnly={true}
            type="text"
          />
        </SectionField>

        <SectionField label="First name" wideScreen>
          <Input
            field="firstName"
            placeholder={initialValues?.user?.firstName}
            readOnly={true}
            type="text"
          />
        </SectionField>
        <SectionField label="Last name" wideScreen>
          <Input
            field="surname"
            placeholder={initialValues?.user?.surname}
            readOnly={true}
            type="text"
          />
        </SectionField>
        <SectionField label="Email" wideScreen>
          <Input
            field="email"
            placeholder={initialValues?.user?.email}
            readOnly={true}
            type="email"
          />
        </SectionField>
        <SectionField label="Position" wideScreen>
          <Input
            field="position"
            placeholder="e.g. Property Analyst, Integrated Transportion & Infrastructure Services"
            type="text"
          />
        </SectionField>
        <SectionField label="Role" wideScreen required>
          <Select field="roleId" options={selectRoles} placeholder="Select..." />
          <TooltipWrapper
            toolTipId="role description tooltip icon"
            toolTip={<RolesToolTip />}
            placement="auto"
            className="tooltip-light"
          >
            <StyledTooltipIcon className="tooltip-icon" />
          </TooltipWrapper>
        </SectionField>
        <SectionField label="Region" wideScreen required>
          <Select field="regionCodeId" options={selectRegions} placeholder="Select MoTI Region" />
        </SectionField>
        <SectionField label="Notes" wideScreen>
          <TextArea
            field="note"
            placeholder="Please specify why you need access to PIMS and include your manager's name."
          />
        </SectionField>
        <Row className="pb-2">
          <Col xs={2}></Col>
          <Col>
            <p>
              By clicking "Submit" to request access, you agree to our{' '}
              <a target="_blank" rel="noopener noreferrer" href={DISCLAIMER_URL}>
                Terms and Conditions
              </a>{' '}
              and that you have read our{' '}
              <a target="_blank" rel="noopener noreferrer" href={PRIVACY_POLICY_URL}>
                Privacy Policy
              </a>
              .
            </p>
          </Col>
        </Row>
        <Row className="justify-content-md-center">
          <ButtonToolbar className="cancelSave">
            {onCancel ? (
              <Button variant="secondary" className="mr-2" type="button" onClick={onCancel}>
                Cancel
              </Button>
            ) : null}
            <Button type="submit">{!initialValues?.id ? 'Submit' : 'Update'}</Button>
          </ButtonToolbar>
        </Row>
      </Form>
    </Formik>
  );
};

const StyledTooltipIcon = styled(FaInfoCircle)`
  position: absolute;
  top: 1rem;
  right: -0.5rem;
`;

export default AccessRequestForm;
