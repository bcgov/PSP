import variables from '_variables.module.scss';
import { LandSvg } from 'components/common/Icons';
import * as React from 'react';
import styled from 'styled-components';

const SidebarContent = styled.div`
  background-color: #fff;
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  text-align: left;
  padding: 16px;
  overflow-y: auto;
`;

const ActionsWrapper = styled.div`
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 16px;
`;

const Action = styled.div`
  width: 347px;
  height: 89px;
  border-radius: 8px;
  fill: #ffffff;
  border: 2px solid ${variables.slideOutBlue};
  margin-bottom: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
`;

const ActionLabelWrapper = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
  height: 47px;
`;

const LandIcon = styled(LandSvg)`
  color: ${variables.textColor};
  height: 47px;
  width: 47px;
  margin-right: 16px;
`;

const ActionPrimaryText = styled.p`
  font-size: 21px;
  color: ${variables.slideOutBlue};
  margin-bottom: 0px;
`;

interface ISubmitPropertySelectorProps {
  addProperty: () => void;
}

/**
 * Display a selection of options for managing property inventory.
 * @param param0 addProperty action to take when add property button clicked
 */
const SubmitPropertySelector: React.FC<ISubmitPropertySelectorProps> = ({ addProperty }) => {
  return (
    <SidebarContent>
      <p>
        This will allow you to submit a property to the PIMS inventory. At any time during this
        process you can read the Inventory Policy by clicking on the icon that looks like this:
      </p>
      <p>To get started, choose the type of property to add to the PIMS inventory.</p>
      <ActionsWrapper>
        <Action onClick={addProperty}>
          <LandIcon className="svg" />
          <ActionLabelWrapper>
            <ActionPrimaryText>Add Titled Property</ActionPrimaryText>
          </ActionLabelWrapper>
        </Action>
      </ActionsWrapper>
    </SidebarContent>
  );
};

export default SubmitPropertySelector;
