import classNames from 'classnames';
import * as React from 'react';
import styled from 'styled-components';
interface ILabelProps {
  content?: string;
  required?: boolean;
}

/** Generic inline label element */
export const Label: React.FunctionComponent<ILabelProps & React.HTMLAttributes<HTMLDivElement>> = ({
  required,
  ...rest
}) => {
  return (
    <StyledLabel {...rest} className={classNames('label', rest.className)}>
      {required && <span className="req">*</span>}
      {rest.children}
    </StyledLabel>
  );
};

const StyledLabel = styled.p`
  display: inline;
  .req {
    color: red;
    font-weight: bold;
  }
`;
