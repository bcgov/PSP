import { noop } from 'lodash';
import * as React from 'react';

import { PaymentsForm } from './table/PaymentsForm';

interface IPaymentsContainerProps {}

/**
 * Orchestrates the display and modification of lease terms and payments.
 */
export const PaymentsContainer: React.FunctionComponent<IPaymentsContainerProps> = () => {
  return (
    <>
      <PaymentsForm onEdit={noop} onDelete={noop}></PaymentsForm>
    </>
  );
};

export default PaymentsContainer;
