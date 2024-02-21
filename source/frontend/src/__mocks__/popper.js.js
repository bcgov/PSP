// __mocks__/popper.js.js

import noop from 'lodash/noop';
import PopperJs from 'popper.js';

export default class Popper {
  constructor() {
    this.placements = PopperJs.placements;

    return {
      update: noop,
      destroy: noop,
      scheduleUpdate: noop,
    };
  }
}
