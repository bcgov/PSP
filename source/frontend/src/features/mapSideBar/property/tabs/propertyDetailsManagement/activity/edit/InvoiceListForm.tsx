import { ArrayHelpers, FieldArray, FormikProps, getIn } from 'formik';
import React from 'react';

import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';

import { InvoiceForm } from './InvoiceForm';
import { InvoiceTotalsForm } from './InvoiceTotalsForm';
import { ActivityInvoiceFormModel, PropertyActivityFormModel } from './models';

export interface IInvoiceListForm {
  field: string;
  formikProps: FormikProps<PropertyActivityFormModel>;
  gstConstant: number;
  pstConstant: number;
}

export const InvoiceListForm: React.FunctionComponent<IInvoiceListForm> = ({
  field,
  formikProps,
  gstConstant,
  pstConstant,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  // clear out existing values instead of removing last item from array
  const onRemove = (index: number, arrayHelpers: ArrayHelpers) => {
    arrayHelpers.remove(index);
    return;
  };

  const invoices = getIn(formikProps.values, field) as ActivityInvoiceFormModel[];
  return (
    <FieldArray name={field}>
      {arrayHelpers => (
        <>
          {invoices.map((invoice, index) => (
            <InvoiceForm
              key={`activity-${invoice.managementActivityId}-invoice-${index}`}
              namespace={`${field}.${index}`}
              index={index}
              formikProps={formikProps}
              onDelete={() => {
                setModalContent({
                  ...getDeleteModalProps(),
                  title: 'Remove Invoice',
                  message:
                    'You have selected to delete an invoice. Are you sure you want to proceed?',
                  okButtonText: 'Remove',
                  handleOk: async () => {
                    onRemove(index, arrayHelpers);
                    setDisplayModal(false);
                  },
                  handleCancel: () => {
                    setDisplayModal(false);
                  },
                });
                setDisplayModal(true);
              }}
              gstConstant={gstConstant}
              pstConstant={pstConstant}
            />
          ))}
          <InvoiceTotalsForm
            formikProps={formikProps}
            onAdd={() => arrayHelpers.push(new ActivityInvoiceFormModel())}
          />
        </>
      )}
    </FieldArray>
  );
};
