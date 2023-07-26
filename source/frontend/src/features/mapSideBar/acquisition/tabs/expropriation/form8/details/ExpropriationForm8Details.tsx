import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Api_Form8 } from '@/models/api/Form8';

export interface IExpropriationForm8DetailsProps {
  form8Index: number;
  form8: Api_Form8;
}

export const ExpropriationForm8Details: React.FunctionComponent<
  IExpropriationForm8DetailsProps
> = ({ form8, form8Index }) => {
  return (
    <StyledSummarySection>
      <Section isCollapsable initiallyExpanded>
        <StyledSubHeader>
          <label>Payment Item {form8Index + 1}</label>
          <StyledRemoveLinkButton
            title="Delete Payment Item"
            data-testid={`paymentItems[${form8Index}].delete-button`}
            variant="light"
            onClick={() => {
              console.log('delete something');
            }}
          >
            <FaTrash size="2rem" />
          </StyledRemoveLinkButton>
        </StyledSubHeader>

        <SectionField
          label="Description"
          labelWidth="4"
          valueTestId={`form8[${form8Index}].description`}
        >
          {form8.description}
        </SectionField>
        <StyledSubHeader title="Payment Details"></StyledSubHeader>
      </Section>
    </StyledSummarySection>
  );
};

export default ExpropriationForm8Details;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  border-bottom: solid 0.2rem ${props => props.theme.css.primaryColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.primaryColor};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;
