import { Formik } from 'formik';
import { ButtonToolbar, Row } from 'react-bootstrap';
import { FaInfoCircle } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { Check, Form, Input, Multiselect } from '@/components/common/form';
import { RadioGroup } from '@/components/common/form/RadioGroup';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import * as API from '@/constants/API';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { UserUpdateSchema } from '@/utils/YupSchema';

import RolesToolTip from '../access-request/RolesToolTip';
import { FormUser, userTypeCodeValues } from '../users/models';

interface IEditUserFormProps {
  updateUserDetail: (user: ApiGen_Concepts_User) => void;
  formUser: FormUser;
  onCancel: () => void;
}

const EditUserForm: React.FunctionComponent<React.PropsWithChildren<IEditUserFormProps>> = ({
  updateUserDetail,
  formUser,
  onCancel,
}) => {
  const { getPublicByType, getByType } = useLookupCodeHelpers();
  const roles = getByType(API.ROLE_TYPES);
  const regions = getPublicByType(API.REGION_TYPES).filter(
    region => region.name !== 'Cannot determine',
  );
  return (
    <Formik<FormUser>
      enableReinitialize
      initialValues={formUser}
      validationSchema={UserUpdateSchema}
      onSubmit={async (values, { setSubmitting }) => {
        await updateUserDetail(values.toApi());
        setSubmitting(false);
        onCancel();
      }}
    >
      {formikProps => (
        <Form className="userInfo">
          <SectionField label="IDIR/BCeID" labelWidth="2">
            <Input
              data-testid="businessIdentifierValue"
              field="businessIdentifierValue"
              value={formikProps.values.businessIdentifierValue}
              readOnly={true}
              type="text"
            />
          </SectionField>

          <SectionField label="First name" labelWidth="2">
            <Input
              data-testid="firstName"
              field="firstName"
              placeholder={formikProps.values.firstName}
              type="text"
            />
          </SectionField>

          <SectionField label="Last name" labelWidth="2">
            <Input
              data-testid="surname"
              field="surname"
              placeholder={formikProps.values.surname}
              type="text"
            />
          </SectionField>

          <SectionField label="Email" labelWidth="2">
            <Input
              data-testid="email"
              field="email"
              placeholder={formikProps.values.email}
              type="email"
            />
          </SectionField>

          <SectionField label="Position" labelWidth="2">
            <Input
              field="position"
              placeholder="e.g) Director, Real Estate and Stakeholder Engagement"
              type="text"
              data-testid="position"
            />
          </SectionField>

          <SectionField label="Internal staff / Contractor" labelWidth="2" required>
            <RadioGroup
              field="userTypeCode.id"
              radioValues={userTypeCodeValues}
              flexDirection="row"
            ></RadioGroup>
          </SectionField>

          <SectionField label="Role(s)" required labelWidth="2">
            <Multiselect placeholder="" field="roles" options={roles} displayValue="name" />
            <TooltipWrapper
              tooltipId="role description tooltip icon"
              tooltip={<RolesToolTip />}
              placement="auto"
              className="tooltip-light"
            >
              <StyledTooltipIcon className="tooltip-icon" />
            </TooltipWrapper>
          </SectionField>

          <SectionField label="MoTI Region(s)" required labelWidth="2">
            <Multiselect<ILookupCode, ApiGen_Base_CodeType<number>>
              placeholder=""
              field="regions"
              options={regions}
              displayValue="name"
              selectFunction={(optionRegions, selectedRegions) =>
                optionRegions.filter(region =>
                  selectedRegions?.find(formRegion => formRegion.id === region.id),
                )
              }
            />
          </SectionField>

          <SectionField label="Notes" labelWidth="2">
            <Input
              as="textarea"
              field="note"
              placeholder="A note about this user"
              type="text"
              data-testid="note"
            />
          </SectionField>

          <SectionField className="d-flex" label="Disable account?" labelWidth="2">
            <TooltipWrapper
              tooltipId="is-disabled-tooltip"
              tooltip={'Click to change account status then click Save.'}
            >
              <Check data-testid="isDisabled" field="isDisabled" />
            </TooltipWrapper>
          </SectionField>

          <Row className="justify-content-md-center">
            <ButtonToolbar className="cancelSave">
              <Button className="mr-5" variant="secondary" type="button" onClick={onCancel}>
                Cancel
              </Button>
              <Button className="mr-5" type="submit">
                Save
              </Button>
            </ButtonToolbar>
          </Row>
        </Form>
      )}
    </Formik>
  );
};

const StyledTooltipIcon = styled(FaInfoCircle)`
  position: absolute;
  top: 1rem;
  right: -0.5rem;
`;

export default EditUserForm;
