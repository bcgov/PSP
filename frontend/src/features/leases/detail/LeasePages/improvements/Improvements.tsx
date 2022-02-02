import { FormSection } from 'components/common/form/styles';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { ILeaseImprovement } from 'interfaces/ILeaseImprovement';

import Improvement from './components/Improvement/Improvement';
import * as Styled from './styles';

export interface IImprovementsProps {
  disabled?: boolean;
}

export const Improvements: React.FunctionComponent<IImprovementsProps> = props => {
  const { values } = useFormikContext<IFormLease>();
  const improvements: ILeaseImprovement[] = getIn(values, 'improvements') ?? [];

  return (
    <Styled.ImprovementsContainer className="improvements">
      <FormSection>
        {props.disabled && <Styled.SectionHeader>Description of Improvements</Styled.SectionHeader>}
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
      </FormSection>
    </Styled.ImprovementsContainer>
  );
};

export default Improvements;
