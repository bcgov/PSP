import React from 'react';
import { Button, Form, Spinner } from 'react-bootstrap';
import { FaCheck } from 'react-icons/fa';
import NumberFormat from 'react-number-format';

import { formatMoney } from '@/utils';

import EditButton from '../../EditButton';

/**
 * Non formik form that allows a user to toggle between a value and an input and save the input.
 */

export interface IToggleSaveInputViewProps {
  onClick: (value: string) => Promise<void>;
  asCurrency?: boolean;
  isEditing?: boolean;
  setValue: (value: string) => void;
  value: string;
  isSaving?: boolean;
  setIsEditing: React.Dispatch<React.SetStateAction<boolean>>;
}

export const ToggleSaveInputView: React.FC<IToggleSaveInputViewProps> = ({
  onClick,
  asCurrency,
  isEditing,
  value,
  isSaving,
  setIsEditing,
  setValue,
}) => {
  if (isEditing) {
    const onHandleCurrencyChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const regex = /[^0-9.-]/g;
      const cleanValue = e.target.value.replace(regex, '');
      setValue(cleanValue);
    };
    return (
      <>
        {!asCurrency ? (
          <Form.Control onChange={e => setValue(e.currentTarget.value)} />
        ) : (
          <NumberFormat
            value={value}
            onChange={onHandleCurrencyChange}
            className="form-control input-number"
            fixedDecimalScale
            decimalScale={2}
            prefix={'$'}
            thousandSeparator
            allowNegative
            title="Enter a financial value"
          />
        )}
        <Button title="confirm" variant="link" onClick={() => onClick(value)}>
          {isSaving ? (
            <Spinner data-testid="spinner" animation="border" />
          ) : (
            <FaCheck size={'2rem'} />
          )}
        </Button>
      </>
    );
  } else {
    return (
      <>
        {asCurrency ? formatMoney(Number(value)) : value}
        <EditButton onClick={() => setIsEditing(true)} />
      </>
    );
  }
};
