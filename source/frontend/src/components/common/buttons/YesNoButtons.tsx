import * as React from 'react';
import styled from 'styled-components';

import { Form } from '../form';
import { InlineFlexDiv } from '../styles';

interface IYesNoButtonsProps {
  className?: string;
  innerClassName?: string;
  id: string;
  value?: boolean;
  disabled?: boolean;
}

export const YesNoButtons: React.FunctionComponent<IYesNoButtonsProps> = ({
  className,
  innerClassName,
  id,
  value,
  disabled,
}) => {
  return (
    <StyledRadioGroup className={className}>
      <div className="radio-group">
        <InlineFlexDiv>
          <Form.Check
            id={`input-${id}`}
            className={innerClassName}
            disabled={disabled}
            type="radio"
            checked={value}
          />
          <Form.Label className="form-check-label">Yes</Form.Label>
          <Form.Check
            id={`input-${id}`}
            className={innerClassName}
            disabled={disabled}
            type="radio"
            checked={!value}
          />
          <Form.Label className="form-check-label">No</Form.Label>
        </InlineFlexDiv>
      </div>
    </StyledRadioGroup>
  );
};

export const StyledRadioGroup = styled(Form.Group)`
  &.form-group {
    display: flex;
    margin-bottom: 0;
    .radio-group {
      display: flex;
      flex-direction: ${props => props.$flexDirection};
      row-gap: ${props => (props.$flexDirection === 'column' ? '0.4rem' : 'unset')};
      column-gap: ${props => (props.$flexDirection === 'column' ? 'unset' : '0.4rem')};
      .form-check {
        display: flex;
        align-items: center;
        justify-content: center;
        .form-check-input {
          margin-left: 0;
        }
      }
      .form-check-label {
        margin-left: 1rem;
      }
    }
  }

  .form-label {
    margin-bottom: unset;
  }
`;

export default YesNoButtons;
