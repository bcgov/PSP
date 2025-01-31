import { Form } from 'react-bootstrap';
import styled from 'styled-components';

import { TextArea } from '@/components/common/form';

export const LeaseH4 = styled.h4`
  font-size: 1.8rem;
  color: white;
  text-align: center;
  padding-bottom: 1.6rem;
`;

export const FormDescriptionLabel = styled(Form.Label)`
  font-size: 1.6rem;
  font-family: 'BCSans-Bold';
`;

export const FormDescriptionBody = styled(TextArea)`
  font-family: 'BCSans';
`;

export const FieldValue = styled.p`
  grid-column: controls;
  border-left: 1px solid black !important;
  padding: 0.6rem 1.2rem;
  color: #495057;
  font-family: 'BCSans-Bold';
`;
