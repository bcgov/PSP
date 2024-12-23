import { ButtonProps } from 'react-bootstrap/Button';
import { FaEye } from 'react-icons/fa';
import { CSSProperties } from 'styled-components';

import { StyledIconButton } from './IconButton';

interface IViewButtonProps extends ButtonProps {
  onClick: () => void;
  title?: string;
  icon?: React.ReactNode;
  dataTestId?: string | null;
  style?: CSSProperties | null;
}

export const ViewButton: React.FunctionComponent<
  React.PropsWithChildren<IViewButtonProps>
> = props => (
  <StyledIconButton
    variant="primary"
    title={props.title ?? 'view'}
    onClick={props.onClick}
    data-testid={props['data-testId'] ?? 'view-button'}
    style={props.style}
  >
    {props.icon ?? <FaEye size={22} />}
  </StyledIconButton>
);

export default ViewButton;
