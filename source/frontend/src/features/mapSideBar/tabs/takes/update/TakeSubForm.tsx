import { StyledRemoveLinkButton } from 'components/common/buttons';
import { FastDatePicker, Select, TextArea } from 'components/common/form';
import { RadioGroup, yesNoRadioGroupValues } from 'components/common/form/RadioGroup';
import AreaContainer from 'components/measurements/AreaContainer';
import * as API from 'constants/API';
import { getIn, useFormikContext } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from 'hooks/useModalContext';
import * as React from 'react';
import { FaTrash } from 'react-icons/fa';
import { withNameSpace } from 'utils/formUtils';

import { Section } from '../../Section';
import { SectionField } from '../../SectionField';
import { StyledBorderSection, StyledNoTabSection } from '../styles';

interface ITakeSubFormProps {
  takeIndex: number;
  nameSpace: string;
  onRemove: (index: number) => void;
}

const TakeSubForm: React.FunctionComponent<ITakeSubFormProps> = ({
  takeIndex,
  nameSpace,
  onRemove,
}) => {
  const formikProps = useFormikContext();
  const { values, setFieldValue, handleChange } = formikProps;
  const currentTake = getIn(values, withNameSpace(nameSpace));
  const { getOptionsByType } = useLookupCodeHelpers();
  const { setModalContent, setDisplayModal } = useModalContext();

  const takeTypeOptions = getOptionsByType(API.TAKE_TYPES);
  const takeStatusTypeOptions = getOptionsByType(API.TAKE_STATUS_TYPES);
  const takeSiteContamTypeOptions = getOptionsByType(API.TAKE_SITE_CONTAM_TYPES);

  const isSurplusSeverance = getIn(values, withNameSpace(nameSpace, 'isSurplusSeverance'));
  const isNewRightOfWay = getIn(values, withNameSpace(nameSpace, 'isNewRightOfWay'));
  const isStatutoryRightOfWay = getIn(values, withNameSpace(nameSpace, 'isStatutoryRightOfWay'));
  const isSection16 = getIn(values, withNameSpace(nameSpace, 'isSection16'));
  const isLicenseToConstruct = getIn(values, withNameSpace(nameSpace, 'isLicenseToConstruct'));

  const getModalWarning = (onOk: () => void) => {
    return (e: React.ChangeEvent<any>) => {
      if (e.target.value === 'false') {
        setModalContent({
          title: 'Confirm change',
          message: 'The area, if provided, will be cleared. Do you wish to proceed?',
          okButtonText: 'Confirm',
          cancelButtonText: 'Cancel',
          handleOk: () => {
            onOk();
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        handleChange(e);
      }
    };
  };

  return (
    <Section
      className="position-relative"
      header={`Take ${takeIndex + 1}`}
      isCollapsable={true}
      initiallyExpanded={true}
    >
      <StyledRemoveLinkButton
        style={{ position: 'absolute', right: '1rem' }}
        title="delete take"
        data-testid="take-delete-button"
        icon={<FaTrash size={24} id={`take-delete-${takeIndex}`} title="delete take" />}
        onClick={() => {
          setModalContent({
            ...getDeleteModalProps(),
            handleOk: () => {
              onRemove(takeIndex);
              setDisplayModal(false);
            },
          });
          setDisplayModal(true);
        }}
      ></StyledRemoveLinkButton>
      <SectionField label="Take type" required labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeTypeCode')}
          options={takeTypeOptions}
          placeholder="Select take type"
        />
      </SectionField>
      <SectionField label="Take status" required labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeStatusTypeCode')}
          options={takeStatusTypeOptions}
        />
      </SectionField>
      <SectionField label="Take contamination" labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeSiteContamTypeCode')}
          options={takeSiteContamTypeOptions}
        />
      </SectionField>
      <SectionField label="Description of this Take" labelWidth="12">
        <TextArea field={withNameSpace(nameSpace, 'description')} />
      </SectionField>
      <StyledNoTabSection header="Area">
        <StyledBorderSection>
          <SectionField label="Is there a new right of way? *" labelWidth="8">
            <RadioGroup
              field={withNameSpace(nameSpace, 'isNewRightOfWay')}
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isNewRightOfWay'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'newRightOfWayArea'), 0);
              })}
            />
          </SectionField>
          {isNewRightOfWay === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'newRightOfWayArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'newRightOfWayAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'newRightOfWayAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.newRightOfWayArea}
                  field={withNameSpace(nameSpace, 'newRightOfWayArea')}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is there a Statutory Right of Way: (SRW)? *" labelWidth="8">
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isStatutoryRightOfWay')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isStatutoryRightOfWay'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'statutoryRightOfWayArea'), 0);
              })}
            />
          </SectionField>
          {isStatutoryRightOfWay === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'statutoryRightOfWayArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'statutoryRightOfWayAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'statutoryRightOfWayAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.statutoryRightOfWayArea}
                  field={withNameSpace(nameSpace, 'statutoryRightOfWayArea')}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is there a Section 16? *" labelWidth="8">
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isSection16')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isSection16'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'section16Area'), 0);
                setFieldValue(withNameSpace(nameSpace, 'section16EndDt'), '');
              })}
            />
          </SectionField>
          {isSection16 === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(withNameSpace(nameSpace, 'section16Area'), landArea);
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'section16AreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(values, withNameSpace(nameSpace, 'section16AreaUnitTypeCode'))}
                  landArea={currentTake.section16Area}
                  field={withNameSpace(nameSpace, 'section16Area')}
                />
              </SectionField>
              <SectionField label="Section 16 end date" contentWidth="4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'section16EndDt')}
                  formikProps={formikProps}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is there a License to Construct (LTC)? *" labelWidth="8">
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isLicenseToConstruct')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isLicenseToConstruct'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'licenseToConstructArea'), 0);
                setFieldValue(withNameSpace(nameSpace, 'ltcEndDt'), '');
              })}
            />
          </SectionField>
          {isLicenseToConstruct === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'licenseToConstructArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'licenseToConstructAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'licenseToConstructAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.licenseToConstructArea}
                  field={withNameSpace(nameSpace, 'licenseToConstructArea')}
                />
              </SectionField>

              <SectionField label="LTC end date" contentWidth="4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'ltcEndDt')}
                  formikProps={formikProps}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
      </StyledNoTabSection>
      <StyledNoTabSection header="Surplus">
        <StyledBorderSection>
          <SectionField label="Is there a Surplus? *" labelWidth="8">
            <RadioGroup
              field={withNameSpace(nameSpace, 'isSurplusSeverance')}
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isSurplusSeverance'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'surplusSeveranceArea'), 0);
              })}
            />
          </SectionField>
          {isSurplusSeverance === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'surplusSeveranceArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'surplusSeveranceAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'surplusSeveranceAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.surplusSeveranceArea}
                  field={withNameSpace(nameSpace, 'surplusSeveranceArea')}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
      </StyledNoTabSection>
    </Section>
  );
};

export default TakeSubForm;
