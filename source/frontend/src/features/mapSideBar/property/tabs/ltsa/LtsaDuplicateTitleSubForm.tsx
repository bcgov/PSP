import { FieldArray, getIn, useFormikContext } from 'formik';
import * as React from 'react';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { DuplicateCertificate, LtsaOrders } from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';
export interface ILtsaDuplicateTitleSubForm {
  nameSpace?: string;
}

export const LtsaDuplicateTitleSubForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaDuplicateTitleSubForm>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const certificates =
    getIn(values, withNameSpace(nameSpace, 'duplicateCertificatesOfTitle')) ?? [];
  return (
    <>
      {certificates.length === 0 && 'None'}
      <FieldArray
        name={withNameSpace(nameSpace, 'duplicateCertificatesOfTitle')}
        render={({ name }) => (
          <React.Fragment key={`certificate-row-${name}`}>
            {certificates.map((title: DuplicateCertificate, index: number) => {
              const innerNameSpace = withNameSpace(
                nameSpace,
                `duplicateCertificatesOfTitle.${index}`,
              );
              return (
                <React.Fragment key={`${title}-${index}`}>
                  <SectionField label="To">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateDelivery.intendedRecipientLastName',
                      )}`}
                    />
                  </SectionField>
                  <SectionField label="Application #">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateIdentifier.documentNumber',
                      )}`}
                    />
                  </SectionField>
                  <SectionField label="Document district code">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateIdentifier.documentDistrictCode',
                      )}`}
                    />
                  </SectionField>
                  <SectionField label="Certificate text">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateDelivery.certificateText',
                      )}`}
                    />
                  </SectionField>
                  {index < certificates.length - 1 && <hr></hr>}
                </React.Fragment>
              );
            })}
          </React.Fragment>
        )}
      />
    </>
  );
};

export default LtsaDuplicateTitleSubForm;
