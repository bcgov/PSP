import { FieldArray, useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { Input, Select } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { exists } from '@/utils';

import { HistoricalNumberForm, UpdatePropertyDetailsFormModel } from './models';

export interface IUpdateHistoricalNumbersSubFormProps {
  propertyId: number;
}

export const UpdateHistoricalNumbersSubForm: React.FC<IUpdateHistoricalNumbersSubFormProps> = ({
  propertyId,
}) => {
  const { values, setFieldValue } = useFormikContext<UpdatePropertyDetailsFormModel>();
  const { getOptionsByType } = useLookupCodeHelpers();

  // (sort alpha; exceptions: 1. Other at end, and 2. PS second in list)
  // The order is set via displayOrder in the DB
  const historicalNumberTypes = getOptionsByType(API.HISTORICAL_NUMBER_TYPES);

  return (
    <FieldArray
      name="historicalNumbers"
      render={arrayHelpers => (
        <>
          {values.historicalNumbers?.map((hn, index) => (
            <React.Fragment key={`property-historical-${index}`}>
              <Row className="py-3" data-testid={`historical-number-row-${index}`}>
                <Col xs="auto" xl="5">
                  <Input field={`historicalNumbers.${index}.historicalNumber`} />
                </Col>
                <Col xs="auto" xl="5" className="pl-0">
                  <Select
                    data-testid={`select-historical-type-${index}`}
                    placeholder="Select type..."
                    field={`historicalNumbers.${index}.historicalNumberType`}
                    options={historicalNumberTypes}
                    value={hn.historicalNumberType}
                    onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                      const selected = e?.target?.value;
                      if (exists(selected) && selected !== 'OTHER') {
                        // clear associated field when historical file # type changes
                        setFieldValue(`historicalNumbers.${index}.otherHistoricalNumberType`, '');
                      }
                    }}
                  />
                </Col>
                <Col xs="auto" xl="2" className="pl-0">
                  <RemoveButton
                    data-testId={`historical-number-remove-button-${index}`}
                    onRemove={() => {
                      arrayHelpers.remove(index);
                    }}
                  />
                </Col>
              </Row>
              {values.historicalNumbers[index]?.historicalNumberType === 'OTHER' && (
                <SectionField label="Describe other" required>
                  <Input field={`historicalNumbers.${index}.otherHistoricalNumberType`} required />
                </SectionField>
              )}
            </React.Fragment>
          ))}
          <LinkButton
            data-testid="add-historical-number"
            onClick={() => {
              const hn = new HistoricalNumberForm();
              hn.propertyId = propertyId;
              arrayHelpers.push(hn);
            }}
          >
            + Add historical file #
          </LinkButton>
        </>
      )}
    />
  );
};

export default UpdateHistoricalNumbersSubForm;
