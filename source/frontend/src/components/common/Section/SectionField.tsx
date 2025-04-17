import cx from 'classnames';
import { Col, ColProps, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { renderTooltip } from '@/utils/formUtils';

export interface ISectionFieldProps {
  label: React.ReactNode | string | null;
  /** It accepts either a string or a custom React tooltip component  */
  tooltip?: React.ReactNode;
  className?: string;
  valueClassName?: string;
  required?: boolean;
  noGutters?: boolean;
  labelWidth?: ColProps;
  contentWidth?: ColProps;
  valueTestId?: string | null;
}

export const SectionField: React.FunctionComponent<
  React.PropsWithChildren<ISectionFieldProps>
> = props => {
  return (
    <Row className={props.className ?? cx('pb-2', { 'no-gutters': props.noGutters })}>
      <Col {...(props.labelWidth ?? { xs: 4 })} className="pr-0 text-left">
        {props.label && (
          <StyledFieldLabel>
            {props.label}:{props.tooltip && <span>{renderTooltip(props.tooltip)}</span>}
          </StyledFieldLabel>
        )}
      </Col>
      <ContentCol
        {...props.contentWidth}
        className={cx(props.valueClassName, {
          required: props.required,
          'text-left': !props.valueClassName,
        })}
        data-testid={props.valueTestId}
      >
        {props.children}
      </ContentCol>
    </Row>
  );
};

export const ContentCol = styled(Col)`
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
