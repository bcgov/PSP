import { FormSection } from 'components/common/form/styles';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { ILeaseImprovement } from 'interfaces/ILeaseImprovement';

import Improvement from './components/Improvement/Improvement';
import Summary from './components/Summary/Summary';
import * as Styled from './styles';

export interface IImprovementsProps {}

export const Improvements: React.FunctionComponent<IImprovementsProps> = props => {
  const { values } = useFormikContext<IFormLease>();
  const improvements: ILeaseImprovement[] = getIn(values, 'improvements') ?? [];

  return (
    <Styled.ImprovementsContainer>
      <FormSection>
        <Summary disabled={true} />
      </FormSection>

      <FormSection>
        <Styled.ImprovementsListHeader>Description of Improvements</Styled.ImprovementsListHeader>
        <FieldArray
          name="improvements"
          render={renderProps =>
            improvements.map((entry: ILeaseImprovement, index) => (
              <Improvement {...renderProps} nameSpace={`improvements.${index}`} disabled={true} />
            ))
          }
        ></FieldArray>
      </FormSection>
    </Styled.ImprovementsContainer>
  );
};

export default Improvements;
