import * as React from 'react';
import { Button } from 'react-bootstrap';
import { FaEdit } from 'react-icons/fa';

interface IEditButtonProps {
  onClick: () => void;
  title?: string;
  icon?: React.ReactNode;
}

export const EditButton: React.FunctionComponent<React.PropsWithChildren<IEditButtonProps>> = ({
  onClick,
  title,
  icon,
}) => {
  return (
    <Button variant="link" title={title ?? 'edit'} onClick={onClick}>
      {icon ?? <FaEdit size={'2rem'} />}
    </Button>
  );
};

export default EditButton;
