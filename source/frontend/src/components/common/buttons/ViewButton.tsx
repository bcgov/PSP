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

export const ViewButton: React.FunctionComponent<React.PropsWithChildren<IViewButtonProps>> = ({
  onClick,
  title,
  icon,
  dataTestId,
  style,
}) => (
  <StyledIconButton
    variant="primary"
    title={title ?? 'edit'}
    onClick={onClick}
    data-testid={dataTestId ?? 'view-button'}
    style={style}
  >
    {icon ?? <FaEye size={22} />}
  </StyledIconButton>
);

export default ViewButton;
