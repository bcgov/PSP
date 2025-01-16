import { FaCircle } from 'react-icons/fa';
import styled from 'styled-components';

export interface IActiveIndicatorProps {
  active: boolean;
  title?: string;
  size?: number | string;
  icon?: React.ReactNode;
  'data-testId'?: string;
}

export const ActiveIndicator: React.FC<IActiveIndicatorProps> = ({
  active,
  size,
  icon,
  title,
  'data-testId': dataTestId,
}) => {
  return (
    <Container active={active ?? false} data-testid={dataTestId ?? 'active-indicator'}>
      {icon ?? <FaCircle title={title ?? active ? 'Active' : 'Inactive'} size={size ?? 10} />}
    </Container>
  );
};

export default ActiveIndicator;

const Container = styled.div<{ active: boolean }>`
  display: flex;
  align-items: center;
  padding-right: 0.6rem;
  color: ${props =>
    props.active
      ? props.theme.bcTokens.iconsColorSuccess
      : props.theme.bcTokens.iconsColorDisabled};
`;
