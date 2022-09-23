import { Col } from 'react-bootstrap';
import styled from 'styled-components';

export const LabelCol = styled(Col)`
  border-right: solid 1px ${props => props.theme.css.primaryColor};
  padding-bottom: 1rem;
`;

export const SectionHeader = styled.h2`
  font-size: 2rem;
  color: ${props => props.theme.css.textColor};
  text-align: left;
  font-family: BcSans-Bold;
`;

export const SubTitle = styled.h3`
  font-family: 'BcSans-Bold';
  font-size: 2rem;
  margin-bottom: 1rem;
  text-align: left;
  padding: 1rem 0 0.5rem 0;
  color: ${props => props.theme.css.textColor};
  border-bottom: solid 0.3rem ${props => props.theme.css.primaryColor};
`;

export const BoldHeader = styled.h3`
  font-size: 1.6rem;
  color: ${props => props.theme.css.formTextColor};
  text-align: left;
  padding-top: 1.5rem;
  font-family: BcSans-Bold;
`;

export const InsuranceTypeList = styled.ul`
  padding: 1rem;
  font-size: 1.4rem;
  li {
    list-style-position: inside;
    padding: 0.5rem 0;
    ::marker {
      font-size: 1rem;
    }
  }
`;

export const BoldValueText = styled(Col)`
  font-family: 'BCSans-Bold';
`;
