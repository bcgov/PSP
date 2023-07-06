import cx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { DisplayError } from './DisplayError';

export interface IFormItemProps {
  field: string;
}

/**
 * Generic Formik-connected wrapper that displays a red border and error message when validation fails this field.
 * Can be used to wrap tables or other custom Formik components to provide feedback after form validation.
 */
export const FormItem: React.FC<React.PropsWithChildren<IFormItemProps>> = ({
  field,
  children,
}) => {
  const { errors, touched } = useFormikContext<any>();
  const error = getIn(errors, field, undefined);
  const isTouched = getIn(touched, field, false) as boolean;
  const hasError = error !== undefined && isTouched;

  return (
    <React.Fragment>
      <StyledDiv
        className={cx({
          'is-invalid': hasError,
        })}
      >
        {children}
      </StyledDiv>
      {error && <DisplayError field={field} />}
    </React.Fragment>
  );
};

export default FormItem;

const StyledDiv = styled.div`
  background: none;
  &.is-invalid {
    border: ${props => props.theme.css.dangerColor} solid 0.1rem;
  }
`;
