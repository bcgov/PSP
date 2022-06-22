import { Button } from 'components/common/buttons';
import { Check, Form, Input, Multiselect } from 'components/common/form';
import TooltipWrapper from 'components/common/TooltipWrapper';
import * as API from 'constants/API';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import Api_TypeCode from 'models/api/TypeCode';
import { Api_User } from 'models/api/User';
import * as React from 'react';
import { ButtonToolbar, Row } from 'react-bootstrap';
import { FaInfoCircle } from 'react-icons/fa';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';
import { UserUpdateSchema } from 'utils/YupSchema';

import RolesToolTip from '../access-request/RolesToolTip';
import { FormUser } from '../users/models';

interface IEditUserFormProps {
  updateUserDetail: (user: Api_User) => void;
  formUser: FormUser;
  onCancel: () => void;
}

const EditUserForm: React.FunctionComponent<IEditUserFormProps> = ({
  updateUserDetail,
  formUser,
  onCancel,
}) => {
  const { getPublicByType } = useLookupCodeHelpers();
  const roles = getPublicByType(API.ROLE_TYPES);
  const regions = getPublicByType(API.REGION_TYPES).filter(
    region => region.name !== 'Cannot determine',
  );
  return (
    <Formik<FormUser>
      enableReinitialize
      initialValues={formUser}
      validationSchema={UserUpdateSchema}
      onSubmit={async (values, { setSubmitting, setValues }) => {
        await updateUserDetail(values.toApi());
        setSubmitting(false);
        onCancel();
      }}
    >
      {formikProps => (
        <Form className="userInfo">
          <SectionField label="IDIR/BCeID" wideScreen>
            <Input
              data-testid="businessIdentifier"
              field="businessIdentifier"
              value={formikProps.values.businessIdentifierValue}
              readOnly={true}
              type="text"
            />
          </SectionField>
          <SectionField label="First name" wideScreen>
            <Input
              data-testid="firstName"
              field="firstName"
              placeholder={formikProps.values.firstName}
              type="text"
            />
          </SectionField>
          <SectionField label="Last name" wideScreen>
            <Input
              data-testid="surname"
              field="surname"
              placeholder={formikProps.values.surname}
              type="text"
            />
          </SectionField>

          <SectionField label="Email" wideScreen>
            <Input
              data-testid="email"
              field="email"
              placeholder={formikProps.values.email}
              type="email"
            />
          </SectionField>

          <SectionField label="Position" wideScreen>
            <Input
              field="position"
              placeholder="e.g) Director, Real Estate and Stakeholder Engagement"
              type="text"
              data-testid="position"
            />
          </SectionField>
          <SectionField label="Role(s)" required wideScreen>
            <Multiselect placeholder="" field="roles" options={roles} displayValue="name" />
            <TooltipWrapper
              toolTipId="role description tooltip icon"
              toolTip={<RolesToolTip />}
              placement="auto"
              className="tooltip-light"
            >
              <StyledTooltipIcon className="tooltip-icon" />
            </TooltipWrapper>
          </SectionField>

          <SectionField label="MoTI Region(s)" required wideScreen>
            <Multiselect<ILookupCode, Api_TypeCode<number>>
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

          <SectionField label="Notes" wideScreen>
            <Input
              as="textarea"
              field="note"
              placeholder="A note about this user"
              type="text"
              data-testid="note"
            />
          </SectionField>

          <SectionField className="d-flex" label="Disable account?" wideScreen>
            <TooltipWrapper
              toolTipId="is-disabled-tooltip"
              toolTip={'Click to change account status then click Save.'}
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
