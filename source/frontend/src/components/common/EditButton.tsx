import * as React from 'react';
import { Button } from 'react-bootstrap';
import { FaEdit } from 'react-icons/fa';

interface IEditButtonProps {
  onClick: () => void;
  title?: string;
}

export const EditButton: React.FunctionComponent<IEditButtonProps> = ({ onClick, title }) => {
  return (
    <Button
      variant="link"
      title={title ?? 'edit'}
      onClick={() => {
        onClick();
      }}
    >
      <FaEdit size={'2rem'} />
    </Button>
  );
};

export default EditButton;
