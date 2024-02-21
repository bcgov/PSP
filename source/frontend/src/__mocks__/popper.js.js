// __mocks__/popper.js.js

import PopperJs from 'popper.js';
import noop from 'lodash/noop';

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
