import 'jest-preset-angular';
import './jestGlobalMocks';
import './setupTestBed';

import * as $ from 'jquery';
Object.defineProperty(window, '$', {value: $});
Object.defineProperty(global, '$', {value: $});
Object.defineProperty(global, 'jQuery', {value: $});
