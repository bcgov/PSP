import { Form } from 'formik';
import { Col } from 'react-bootstrap';
import { MdArrowDropDown, MdArrowDropUp } from 'react-icons/md';
import styled from 'styled-components';

export const StyledReadOnlyForm = styled(Form)`
  position: relative;
  &&& {
    input,
    select,
    textarea {
      background: none;
      border: none;
      resize: none;
      height: fit-content;
      padding: 0;
    }
    .form-label {
      font-weight: bold;
    }
  }
`;

export const InlineContainer = styled.div`
  display: flex;
  flex-wrap: nowrap;
  flex-direction: row;
  align-items: center;
  gap: 0.8rem;
`;

export const LeftBorderCol = styled(Col)`
  border-left: 1px solid #8c8c8c;
`;

export const TableCaption = styled.label`
  width: 100%;
  font-weight: bold;
  padding-bottom: 0.7rem;
  margin-bottom: 0;
  border-bottom: 2px solid #8c8c8c;
`;

export const StyledInlineMessageSection = styled.div`
  margin: 1.5rem;
  padding: 0.5rem 1.5rem;
  background-color: white;
  text-align: left;
`;

export const InlineMessage = styled.div`
  display: flex;
  flex-wrap: nowrap;
  flex-direction: row;
  align-items: center;
  gap: 0.8rem;
  font-style: italic;
`;

export const ArrowDropDownIcon = styled(MdArrowDropDown)`
  float: right;
  cursor: pointer;
`;
export const ArrowDropUpIcon = styled(MdArrowDropUp)`
  float: right;
  cursor: pointer;
`;

export const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding-top: 1rem;
`;

export const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};

  text-align: right;
`;

export const StyledSubtleText = styled.p`
  color: ${props => props.theme.css.subtleColor};
  text-align: left;
`;
