import { FaEdit } from 'react-icons/fa';
import { CSSProperties } from 'styled-components';

import { StyledIconButton } from './IconButton';

interface IEditButtonProps {
  onClick: () => void;
  title?: string;
  icon?: React.ReactNode;
  dataTestId?: string | null;
  style?: CSSProperties | null;
}

export const EditButton: React.FunctionComponent<React.PropsWithChildren<IEditButtonProps>> = ({
  onClick,
  title,
  icon,
  dataTestId,
  style,
}) => {
  return (
    <StyledIconButton
      variant="primary"
      title={title ?? 'edit'}
      onClick={onClick}
      data-testid={dataTestId ?? 'edit-button'}
      style={style}
    >
      {icon ?? <FaEdit size={22} />}
    </StyledIconButton>
  );
};

export default EditButton;
