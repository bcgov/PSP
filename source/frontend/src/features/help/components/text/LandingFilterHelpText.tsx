import * as React from 'react';

/**
 * Help Text for the Landing Page Filter topic.
 */
const LandingFilterHelpText = () => {
  return (
    <p>
      The filter provides a way to search for properties with the specified properties. The filter
      is cumulative ("AND"), which means each value will refine the results.
      <br />
      <strong>PID/PIN:</strong> The property has the specified PID or PIN.
      <br />
      <strong>Address:</strong> The address contains the value brought back from LTSA.
      <br />
    </p>
  );
};

export default LandingFilterHelpText;
