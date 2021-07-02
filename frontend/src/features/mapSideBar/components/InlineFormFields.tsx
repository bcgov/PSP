import { Col } from 'react-bootstrap';
import styled from 'styled-components';

/**
 * Styled component for form elements that should be displayed on a single line (so the input and the label are on the same line).
 */
export const InlineFormFields = styled(Col)`
  .form-group {
    display: flex;
    justify-content: space-between;
  }
`;
