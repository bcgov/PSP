import { FieldArray, getIn, useFormikContext } from 'formik';

import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IFormLease } from '@/interfaces';
import { ILeaseImprovement } from '@/interfaces/ILeaseImprovement';

import Improvement from './components/Improvement/Improvement';
import * as Styled from './styles';

export interface IImprovementsProps {
  disabled?: boolean;
}

export const Improvements: React.FunctionComponent<IImprovementsProps> = props => {
  const { values } = useFormikContext<IFormLease>();
  const improvements: ILeaseImprovement[] = getIn(values, 'improvements') ?? [];

  const { getByType } = useLookupCodeHelpers();
  const improvementTypeCodes = getByType(API.PROPERTY_IMPROVEMENT_TYPES);

  improvements.sort((improvementOne: ILeaseImprovement, improvementTwo: ILeaseImprovement) => {
    const findDisplayOrder = (x: ILeaseImprovement): number => {
      for (let typeCode of improvementTypeCodes) {
        if (x.propertyImprovementTypeId === typeCode.id) {
          return typeCode.displayOrder;
        }
      }
      return 0;
    };
    return findDisplayOrder(improvementOne) - findDisplayOrder(improvementTwo);
  });

  return (
    <Styled.ImprovementsContainer className="improvements">
      <FieldArray
        name="improvements"
        render={renderProps =>
          improvements.map((entry: ILeaseImprovement, index) => (
            <Improvement
              {...renderProps}
              nameSpace={`improvements.${index}`}
              disabled={props.disabled}
            />
          ))
        }
      ></FieldArray>
    </Styled.ImprovementsContainer>
  );
};

export default Improvements;
