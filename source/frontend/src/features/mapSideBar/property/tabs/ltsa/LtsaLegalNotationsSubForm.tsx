import { FieldArray, getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { SectionField } from '@/components/common/Section/SectionField';
import { DescriptionOfLand, LtsaOrders } from '@/interfaces/ltsaModels';
import { prettyFormatDate } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

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
                      <SectionField label="Legal notations#" labelWidth="auto">
                        {getIn(values, innerNameSpace + '.legalNotationNumber')}
                      </SectionField>
                    </Col>
                    <Col>
                      <SectionField label="Status" labelWidth="auto">
                        {getIn(values, innerNameSpace + '.status')}
                      </SectionField>
                    </Col>
                    <Col>
                      <SectionField label="Cancellation date" labelWidth="auto">
                        {prettyFormatDate(
                          getIn(values, innerNameSpace + '.legalNotation.applicationReceivedDate'),
                        )}
                      </SectionField>
                    </Col>
                  </Row>
                  <SectionField label="Legal notations">
                    <p>{legalNotationText}</p>
                  </SectionField>
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
