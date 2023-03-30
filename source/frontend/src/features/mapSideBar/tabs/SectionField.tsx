import cx from 'classnames';
import TooltipIcon from 'components/common/TooltipIcon';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISectionFieldProps {
  label: string;
  /** It accepts either a string or a custom React tooltip component  */
  tooltip?: React.ReactNode;
  className?: string;
  required?: boolean;
  noGutters?: boolean;
  labelWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
  contentWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
}

export const SectionField: React.FunctionComponent<
  React.PropsWithChildren<ISectionFieldProps>
> = props => {
  return (
    <Row className={props.className ?? cx('pb-2', { 'no-gutters': props.noGutters })}>
      <Col xs={props.labelWidth ?? '4'} className="pr-0 text-left">
        <StyledFieldLabel>
          {props.label}:
          {props.tooltip && <span className="ml-2">{renderTooltip(props.tooltip)}</span>}
        </StyledFieldLabel>
      </Col>
      <StyledCol
        xs={props.contentWidth ?? true}
        className={cx({ required: props.required, 'text-left': true })}
      >
        {props.children}
      </StyledCol>
    </Row>
  );
};

function renderTooltip(tooltip?: React.ReactNode): React.ReactNode {
  if (tooltip === undefined) {
    return null;
  }
  if (typeof tooltip === 'string' || typeof tooltip === 'number') {
    return <TooltipIcon toolTipId="section-field-tooltip" toolTip={tooltip} placement="auto" />;
  }
  // we got a custom tooltip - render that
  return tooltip;
}

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
