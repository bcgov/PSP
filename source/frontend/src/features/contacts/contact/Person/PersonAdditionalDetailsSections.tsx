import { getIn, useFormikContext } from 'formik';
import styled from 'styled-components';

import { Check, TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { Address } from '@/features/contacts/contact/create/components';

import { IEditablePersonForm } from '../../formModels';

const PersonAdditionalDetailsSections: React.FC = () => {
  const { values } = useFormikContext<IEditablePersonForm>();
  const organizationId = getIn(values, 'organization.id');
  const useOrganizationAddress = getIn(values, 'useOrganizationAddress');

  return (
    <>
      <Section
        isCollapsable
        initiallyExpanded
        header={
          <div className="d-flex align-items-center">
            <span>Mailing Address</span>
            <StyledCheckBox
              field="useOrganizationAddress"
              postLabel="Use mailing address from organization"
              disabled={!organizationId}
              className="ml-auto mb-0"
            />
          </div>
        }
      >
        <Address namespace="mailingAddress" disabled={useOrganizationAddress} />
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
    </>
  );
};

export default PersonAdditionalDetailsSections;

const StyledCheckBox = styled(Check)`
  font-size: 1.6rem;
  font-weight: normal;
  label {
    margin-bottom: 0;
  }
`;
