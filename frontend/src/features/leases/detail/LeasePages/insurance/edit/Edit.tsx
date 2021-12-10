import { FormSection } from 'components/common/form/styles';
import { getIn, useFormikContext } from 'formik';
import { IInsurance } from 'interfaces';
import { Dictionary } from 'interfaces/Dictionary';
import { useState } from 'react';
import { Form } from 'react-bootstrap';
import { Col, FormGroup, Row } from 'react-bootstrap';
import { ILookupCode } from 'store/slices/lookupCodes/interfaces';

export interface InsuranceEditViewProps {
  insuranceList: IInsurance[];
  insuranceTypes: ILookupCode[];
}

interface CheckboxPair {
  label: string;
  value: boolean;
}

const InsuranceEditView: React.FunctionComponent<InsuranceEditViewProps> = ({
  insuranceList,
  insuranceTypes,
}) => {
  const { values, setFieldValue } = useFormikContext<boolean[]>();

  let typeMa0p: Dictionary<boolean> = {};
  for (let i = 0; i < insuranceTypes.length; i++) {
    typeMa0p[insuranceTypes[i].id] = true;
  }
  /*let typeMap: Dictionary<CheckboxPair> = insuranceTypes.forEach(x => {
    return { label: x.name, value: false };
  });*/

  const [isEditing, setEditing] = useState<Dictionary<boolean>>(typeMa0p);

  const onChange = (event: React.FormEvent<HTMLInputElement>) => {
    const target = event.target as HTMLInputElement;
    const value = target.checked;
    const name = target.name;
    console.log(name, value);
    setEditing({
      [name]: value,
    });
    //setFieldValue(name, !getIn(values, name));
  };
  /*const nextValue = !getIn(values, name);
    setFieldValue(name, nextValue);
    // Toggle children nodes
    const nodes = getIn(values, `layers[${index}].nodes`) || [];
    nodes.forEach((node: any, i: number) =>
      setFieldValue(`layers[${index}].nodes[${i}].on`, nextValue),
    );*/

  return (
    <>
      <h2>Required coverage</h2>
      <div>Select the coverage types that are required for this lease or license.</div>
      <FormGroup>
        {insuranceTypes.map((type: ILookupCode, index: number) => (
          <Form.Check
            type="checkbox"
            checked={isEditing[type.id]}
            label={type.name}
            name={type.name}
            key={index + '-' + type.name}
            onChange={onChange}
          />
        ))}
      </FormGroup>
      {insuranceTypes.map((type: ILookupCode, index: number) => (
        <div key={index + '-' + type.id}>
          {type.id} | {typeMa0p[type.id]} |{' '}
        </div>
      ))}

      {insuranceList.map((insurance: IInsurance, index: number) => (
        <div key={index + insurance.id}>
          <FormSection>will edit this!</FormSection>
          <br />
        </div>
      ))}
    </>
  );
};

export default InsuranceEditView;
