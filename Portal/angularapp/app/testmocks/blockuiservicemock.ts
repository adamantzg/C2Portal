import { EventEmitter } from '@angular/core';

export class BlockUIServiceMock {
    blockTicks = 0;
    unBlockTicks = 0;
    blockUIEvent: any = new EventEmitter<any>();

    startBlock() {
      this.blockTicks++;
    }

    stopBlock() {
      this.unBlockTicks++;
    }
}
