import { Input } from 'components/common/form';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { DescriptionOfLand, LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { SectionField } from '../SectionField';
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
                    <React.Fragment key={`land-row-inner-${innerNameSpace}`}>
                      <SectionField label="PID">
                        <Input field={`${withNameSpace(innerNameSpace, 'parcelIdentifier')}`} />
                      </SectionField>
                      <SectionField label="Legal description">
                        <p>{getIn(land, 'fullLegalDescription')}</p>
                      </SectionField>
                      {index < lands.length - 1 && <hr></hr>}
                    </React.Fragment>
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
