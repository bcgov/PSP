import { Input } from 'components/common/form';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { DescriptionOfLand, LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { SectionFieldWrapper } from '../SectionFieldWrapper';
import { StyledSectionHeader } from '../SectionStyles';
import LtsaLegalNotationsSubForm from './LtsaLegalNotationsSubForm';

export interface ILtsaLandSubFormProps {
  nameSpace?: string;
}

export const LtsaLandSubForm: React.FunctionComponent<ILtsaLandSubFormProps> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const lands = getIn(values, withNameSpace(nameSpace, 'descriptionsOfLand')) ?? [];
  return (
    <>
      <StyledSectionHeader>Land</StyledSectionHeader>
      {lands.length === 0 ? (
        'this title has no land'
      ) : (
        <>
          <FieldArray
            name={withNameSpace(nameSpace, 'descriptionsOfLand')}
            render={({ name }) => (
              <React.Fragment key={`land-row-${name}`}>
                {lands.map((land: DescriptionOfLand, index: number) => {
                  const innerNameSpace = withNameSpace(nameSpace, `descriptionsOfLand.${index}`);
                  return (
                    <>
                      <SectionFieldWrapper label="PID">
                        <Input field={`${withNameSpace(innerNameSpace, 'parcelIdentifier')}`} />
                      </SectionFieldWrapper>
                      <SectionFieldWrapper label="Legal Description">
                        <p>{getIn(land, 'fullLegalDescription')}</p>
                      </SectionFieldWrapper>
                      {index < lands.length - 1 && <hr></hr>}
                    </>
                  );
                })}
              </React.Fragment>
            )}
          />
          <LtsaLegalNotationsSubForm nameSpace={nameSpace} />
        </>
      )}
    </>
  );
};

export default LtsaLandSubForm;
