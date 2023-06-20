import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import { FaExternalLinkAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { SectionField } from '@/components/common/Section/SectionField';
import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import { LeaseFormModel } from '@/features/leases/models';
import { withNameSpace } from '@/utils/formUtils';

import { FormTenant } from './models';

export interface ITenantPersonContactInfoProps {
  nameSpace: string;
  disabled?: boolean;
}

/**
 * Sub-form displaying a person tenant associated to the current lease.
 * @param {ITenantPersonContactInfoProps} param0
 */
export const TenantPersonContactInfo: React.FunctionComponent<
  React.PropsWithChildren<ITenantPersonContactInfoProps & Partial<FieldArrayRenderProps>>
> = ({ nameSpace, disabled }) => {
  const { values } = useFormikContext<LeaseFormModel>();
  const tenant: FormTenant = getIn(values, nameSpace);
  return (
    <StyledSectionWrapper>
      <SectionField labelWidth="2" contentWidth="10" label="Person">
        {getIn(values, withNameSpace(nameSpace, 'summary')) && (
          <>
            <StyledLink to={`/contact/${tenant?.id}`} target="_blank" rel="noopener noreferrer">
              {getIn(values, withNameSpace(nameSpace, 'summary'))} <FaExternalLinkAlt />
            </StyledLink>
          </>
        )}
      </SectionField>
    </StyledSectionWrapper>
  );
};

const StyledSectionWrapper = styled.div`
  border-bottom: 0.1rem gray solid;
  padding: 0.5rem;
`;

export default TenantPersonContactInfo;
