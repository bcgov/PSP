import { FieldArray, getIn, useFormikContext } from 'formik';
import { Fragment } from 'react';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { DescriptionOfLand, LtsaOrders } from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';

import LtsaLegalNotationsSubForm from './LtsaLegalNotationsSubForm';

export interface ILtsaLandSubFormProps {
  nameSpace?: string;
}

export const LtsaLandSubForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaLandSubFormProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const lands = getIn(values, withNameSpace(nameSpace, 'descriptionsOfLand')) ?? [];
  return (
    <>
      {lands.length === 0 ? (
        'None'
      ) : (
        <>
          <FieldArray
            name={withNameSpace(nameSpace, 'descriptionsOfLand')}
            render={({ name }) => (
              <Fragment key={`land-row-${name}`}>
                {lands.map((land: DescriptionOfLand, index: number) => {
                  const innerNameSpace = withNameSpace(nameSpace, `descriptionsOfLand.${index}`);
                  return (
                    <Fragment key={`land-row-inner-${innerNameSpace}`}>
                      <SectionField label="PID">
                        <Input field={`${withNameSpace(innerNameSpace, 'parcelIdentifier')}`} />
                      </SectionField>
                      <SectionField label="Legal description">
                        <p>{getIn(land, 'fullLegalDescription')}</p>
                      </SectionField>
                      {index < lands.length - 1 && <hr></hr>}
                    </Fragment>
                  );
                })}
              </Fragment>
            )}
          />
          <LtsaLegalNotationsSubForm nameSpace={nameSpace} />
        </>
      )}
    </>
  );
};

export default LtsaLandSubForm;
