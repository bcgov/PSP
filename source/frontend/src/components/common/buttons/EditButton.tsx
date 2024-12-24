import { FaEdit } from 'react-icons/fa';
import { CSSProperties } from 'styled-components';

import { StyledIconButton } from './IconButton';

interface IEditButtonProps {
  onClick: () => void;
  title?: string;
  icon?: React.ReactNode;
  'data-testId'?: string;
  style?: CSSProperties | null;
}

export const EditButton: React.FunctionComponent<
  React.PropsWithChildren<IEditButtonProps>
> = props => {
  return (
    <StyledIconButton
      variant="primary"
      title={props.title ?? 'edit'}
      onClick={props.onClick}
      data-testid={props['data-testId'] ?? 'edit-button'}
      style={props.style}
    >
      {props.icon ?? <FaEdit size={22} />}
    </StyledIconButton>
  );
};

export default EditButton;
