import { ReactElement } from 'react';
import styled from 'styled-components';

interface ILabelledIconProps {
  icon: ReactElement;
  labelText: string;
}
/**
 * Component that displays a large icon on top of a label.
 * @param {ILabelledIconProps} param0
 */
export const LabelledIcon = ({ icon, labelText }: ILabelledIconProps) => {
  return (
    <StyledLabelledIcon>
      {icon}
      <StyledLabel>{labelText}</StyledLabel>
    </StyledLabelledIcon>
  );
};

const StyledLabelledIcon = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  color: ${props => props.theme.css.slideOutBlue};
  svg {
    width: 5.5rem;
    height: 5.5rem;
  }
  max-width: 8rem;
`;

const StyledLabel = styled.label`
  font-size: 1.6rem;
  font-family: 'BCSans=Bold';
`;
