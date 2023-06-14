import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { FaTrash } from 'react-icons/fa';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { FastDatePicker, Select, TextArea } from '@/components/common/form';
import { RadioGroup, yesNoRadioGroupValues } from '@/components/common/form/RadioGroup';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import AreaContainer from '@/components/measurements/AreaContainer';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { withNameSpace } from '@/utils/formUtils';

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
  const takeLandActTypeOptions = getOptionsByType(API.TAKE_LAND_ACT_TYPES).map(landAct => ({
    ...landAct,
    label: landAct.value + ' ' + landAct.label,
  }));

  const isSurplus = getIn(values, withNameSpace(nameSpace, 'isSurplus'));
  const isNewRightOfWay = getIn(values, withNameSpace(nameSpace, 'isNewRightOfWay'));
  const isStatutoryRightOfWay = getIn(values, withNameSpace(nameSpace, 'isStatutoryRightOfWay'));
  const isLandAct = getIn(values, withNameSpace(nameSpace, 'isLandAct'));
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
      <SectionField label="Site Contamination" labelWidth="4" contentWidth="5">
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
          <SectionField
            label="Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)? *"
            labelWidth="8"
          >
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isLandAct')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isLandAct'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'landActArea'), 0);
                setFieldValue(withNameSpace(nameSpace, 'landActEndDt'), '');
                setFieldValue(withNameSpace(nameSpace, 'landActTypeCode'), '');
              })}
            />
          </SectionField>
          {isLandAct === 'true' && (
            <>
              <SectionField label="Land Act" required contentWidth="7">
                <Select
                  field={withNameSpace(nameSpace, 'landActTypeCode')}
                  placeholder="Select Land Act"
                  options={takeLandActTypeOptions}
                />
              </SectionField>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(withNameSpace(nameSpace, 'landActArea'), landArea);
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'landActAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(values, withNameSpace(nameSpace, 'landActAreaUnitTypeCode'))}
                  landArea={currentTake.landActArea}
                  field={withNameSpace(nameSpace, 'landActArea')}
                />
              </SectionField>
              <SectionField label="End date" contentWidth="4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'landActEndDt')}
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
              field={withNameSpace(nameSpace, 'isSurplus')}
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isSurplus'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'surplusArea'), 0);
              })}
            />
          </SectionField>
          {isSurplus === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(withNameSpace(nameSpace, 'surplusArea'), landArea);
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'surplusAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(values, withNameSpace(nameSpace, 'surplusAreaUnitTypeCode'))}
                  landArea={currentTake.surplusArea}
                  field={withNameSpace(nameSpace, 'surplusArea')}
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
