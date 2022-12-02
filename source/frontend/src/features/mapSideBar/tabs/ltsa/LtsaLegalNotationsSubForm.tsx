import { InlineInput } from 'components/common/form/styles';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { DescriptionOfLand, LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { withNameSpace } from 'utils/formUtils';

export interface ILtsaLegalNotationsSubFormProps {
  nameSpace?: string;
}

const LtsaLegalNotationsSubForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaLegalNotationsSubFormProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const notations = getIn(values, withNameSpace(nameSpace, 'legalNotationsOnTitle')) ?? [];
  return (
    <>
      <FieldArray
        name={withNameSpace(nameSpace, 'legalNotationsOnTitle')}
        render={({ name }) => (
          <React.Fragment key={`notation-row-${name}`}>
            {notations.map((notation: DescriptionOfLand, index: number) => {
              const innerNameSpace = withNameSpace(nameSpace, `legalNotationsOnTitle.${index}`);
              const legalNotationText = getIn(notation, 'legalNotation.legalNotationText');
              return (
                <React.Fragment key={`notation-row-${innerNameSpace}`}>
                  <Row className="pb-2">
                    <Col>
                      <InlineInput
                        label="Legal notations:"
                        field={`${withNameSpace(innerNameSpace, 'legalNotationNumber')}`}
                      />
                    </Col>
                    <Col>
                      <InlineInput
                        label="Status:"
                        field={`${withNameSpace(innerNameSpace, 'status')}`}
                      />
                    </Col>
                    <Col>
                      <InlineInput
                        label="Cancellation date:"
                        field={`${withNameSpace(innerNameSpace, 'cancellationDate')}`}
                      />
                    </Col>
                  </Row>
                  <Row className="pb-2">
                    <Col xs={4}></Col>
                    <Col>
                      <p>{legalNotationText}</p>
                    </Col>
                  </Row>
                </React.Fragment>
              );
            })}
          </React.Fragment>
        )}
      />
    </>
  );
};

export default LtsaLegalNotationsSubForm;
