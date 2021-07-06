import { Row } from 'react-bootstrap';
import styled from 'styled-components';

/**
 * Styled component for a visual section within a form. The form has multiple "sections" that are visually grouped together and split up by whitespace
 */
export const FormSection = styled(Row)`
  background-color: white;
  padding: 0.5rem;
  .map-side-drawer &.row {
    margin: 1rem 1rem 1rem 0;
  }
  .form-group {
    margin-bottom: 0.5rem;
    display: flex;
    flex-wrap: wrap;
    align-items: baseline;
    input.form-control,
    textarea.form-control {
      max-width: 50%;
      &:disabled {
        background-color: white;
        border: none;
        color: ${props => props.theme.css.textColor};
      }
    }
  }
`;
