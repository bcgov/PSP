import * as React from 'react';
import { Button } from 'react-bootstrap';
import { FaEdit } from 'react-icons/fa';

interface IEditButtonProps {
  onClick: () => void;
  title?: string;
  icon?: React.ReactNode;
  dataTestId?: string | null;
}

export const EditButton: React.FunctionComponent<React.PropsWithChildren<IEditButtonProps>> = ({
  onClick,
  title,
  icon,
  dataTestId,
}) => {
  return (
    <Button
      variant="link"
      title={title ?? 'edit'}
      onClick={onClick}
      data-testid={dataTestId ?? 'edit-button'}
    >
      {icon ?? <FaEdit size={'2rem'} />}
    </Button>
  );
};

export default EditButton;
