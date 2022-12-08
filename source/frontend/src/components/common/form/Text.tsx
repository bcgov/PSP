import { getIn, useFormikContext } from 'formik';
import React from 'react';
import styled from 'styled-components';

export type TextProps = {
  /** The field name */
  field?: string;
  /** Adds a custom class to the span element of the <Text> component */
  className?: string;
};

export const Text: React.FC<React.PropsWithChildren<TextProps>> = ({
  field,
  className,
  children,
}) => {
  // if "field" prop is supplied, we render text content from formik field
  // if no field is supplied, then we render the children as-is
  const { values } = useFormikContext();
  const value = field ? getIn(values, field) : children;

  return <StyledSpan className={className}>{value}</StyledSpan>;
};

const StyledSpan = styled.span`
  color: ${props => props.theme.css.formControlTextColor};
`;
