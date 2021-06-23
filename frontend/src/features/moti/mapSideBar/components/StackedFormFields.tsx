import { Col } from 'react-bootstrap';
import styled from 'styled-components';

/**
 * Styled component to display a form element vertically (label on one line, input on the next line).
 */
export const StackedFormFields = styled(Col)`
  .form-group {
    display: flex;
    flex-direction: column;
  }
`;
