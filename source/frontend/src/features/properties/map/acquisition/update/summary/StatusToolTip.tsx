import React from 'react';
import styled from 'styled-components';

const StatusToolTip: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  return (
    <StyledTooltip>
      <b>Draft</b>
      <p>
        The file has been created for the purpose of starting an acquisition, however it is not
        being actively worked on.
      </p>
      <br />
      <b>Active</b>
      <p>The acquisition is in progress and the file is currently being worked on.</p>
      <br />
      <b>Hold</b>
      <p>
        Some work was started for the acquisition process, However, it has been temporarily
        suspended and is likely to resume in the future. For example: acquisition is being delayed
        to the next fiscal year.
      </p>
      <br />
      <b>Cancelled</b>
      <p>
        When it has been identified that the work on this acquisition process can not be completed
        or does not need to be completed. For example: the property in question does not have to be
        acquired any more.
      </p>
      <br />
      <b>Complete</b>
      <p>
        When the ACQ file is completed and all necessary steps for the acquisition process have been
        carried out.
      </p>
      <br />
      <b>Archived</b>
      <p>File needs to be in state of archival as per ORCS.</p>
    </StyledTooltip>
  );
};

const StyledTooltip = styled.div`
  color: black;
  font-size: 1.4rem;
  p {
    margin: 0;
  }
`;

export default StatusToolTip;
