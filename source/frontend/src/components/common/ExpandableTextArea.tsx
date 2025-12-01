import { useEffect, useRef, useState } from 'react';
import { Card } from 'react-bootstrap';
import styled from 'styled-components';

import { LinkButton } from './buttons';

export interface IExpandableTextCardProps {
  text: string;
  maxHeight?: number;
}

export const ExpandableTextCard: React.FunctionComponent<
  React.PropsWithChildren<IExpandableTextCardProps>
> = props => {
  const ref = useRef(null);
  const [shouldShowExpand, setShouldShowExpand] = useState(false);
  const [expanded, setExpanded] = useState(true);

  const max = props.maxHeight ?? 50;

  useEffect(() => {
    if (ref.current?.scrollHeight > max) {
      setShouldShowExpand(true);
      setExpanded(false);
    }
  }, [max]);

  return (
    <StyledCard body style={{ padding: 0 }}>
      <Card.Body style={{ padding: 0 }}>
        <Card.Text ref={ref} style={{ padding: 0, wordBreak: 'break-all' }}>
          <ContentCard style={{ maxHeight: expanded ? 'fit-content' : max }}>
            {props.text}
          </ContentCard>
          {shouldShowExpand && (
            <LinkButton data-testid="expand" onClick={() => setExpanded(!expanded)}>
              {expanded ? '[hide]' : `[more...]`}
            </LinkButton>
          )}
        </Card.Text>
      </Card.Body>
    </StyledCard>
  );
};

export default ExpandableTextCard;

const StyledCard = styled(Card)`
  border: none;

  .card-body {
    padding: 0;
  }
`;

const ContentCard = styled.label`
  overflow: hidden;
  white-space: break-spaces;
  word-break: break-word;
  text-align: justify;
`;
