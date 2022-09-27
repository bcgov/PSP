import clsx from 'classnames';
import TooltipIcon from 'components/common/TooltipIcon';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISectionFieldProps {
  label: string;
  helpText?: React.ReactNode;
  className?: string;
  required?: boolean;
  labelWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
  contentWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
}

export const SectionField: React.FunctionComponent<ISectionFieldProps> = props => {
  return (
    <Row className={props.className ?? 'pb-2'}>
      <Col xs={props.labelWidth ?? '4'} className="pr-0 text-left">
        <StyledFieldLabel>
          {props.label}:
          {props.helpText && (
            <TooltipIcon
              className="ml-2"
              toolTipId="section-field-tooltip"
              toolTip={props.helpText}
            />
          )}
        </StyledFieldLabel>
      </Col>
      <StyledCol
        xs={props.contentWidth ?? true}
        className={clsx({ required: props.required, 'text-left': true })}
      >
        {props.children}
      </StyledCol>
    </Row>
  );
};
export const StyledCol = styled(Col)`
  &.required::before {
    content: '*';
    position: absolute;
    top: 0.75rem;
    left: 0rem;
  }
`;

export const StyledFieldLabel = styled.label`
  font-weight: bold;
`;
