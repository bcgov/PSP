import React from 'react';
import { useParams } from 'react-router-dom';

export const UpdateContactContainer: React.FunctionComponent = () => {
  const { id } = useParams<any>();
  return <div>Contact Edit container - TODO</div>;
};

export default UpdateContactContainer;
