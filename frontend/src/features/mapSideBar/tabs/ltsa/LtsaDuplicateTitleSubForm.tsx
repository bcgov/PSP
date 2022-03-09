import { Input } from 'components/common/form';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { DuplicateCertificate, LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { SectionFieldWrapper } from '../SectionFieldWrapper';
import { StyledSectionHeader } from '../SectionStyles';

export interface ILtsaDuplicateTitleSubForm {
  nameSpace?: string;
}

export const LtsaDuplicateTitleSubForm: React.FunctionComponent<ILtsaDuplicateTitleSubForm> = ({
  nameSpace,
}) => {
  const { values } = useFormikContext<LtsaOrders>();
  const certificates =
    getIn(values, withNameSpace(nameSpace, 'duplicateCertificatesOfTitle')) ?? [];
  return (
    <>
      <StyledSectionHeader>Duplicate Indefeasible Title</StyledSectionHeader>
      {certificates.length === 0 && 'this title has no indefeasible titles'}
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
                <>
                  <SectionFieldWrapper label="To">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateDelivery.intendedRecipientLastName',
                      )}`}
                    />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Application #">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateIdentifier.documentNumber',
                      )}`}
                    />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Document district code">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateIdentifier.documentDistrictCode',
                      )}`}
                    />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Certificate text">
                    <Input
                      field={`${withNameSpace(
                        innerNameSpace,
                        'certificateDelivery.certificateText',
                      )}`}
                    />
                  </SectionFieldWrapper>
                  {index < certificates.length - 1 && <hr></hr>}
                </>
              );
            })}
          </React.Fragment>
        )}
      />
    </>
  );
};

export default LtsaDuplicateTitleSubForm;
