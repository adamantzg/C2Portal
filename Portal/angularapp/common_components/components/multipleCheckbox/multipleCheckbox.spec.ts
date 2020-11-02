import { MultipleCheckboxComponent } from './multipleCheckbox';
import { ComponentFixture, async, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

describe('GridComponent', () => {
    let component: MultipleCheckboxComponent;
    let fixture: ComponentFixture<MultipleCheckboxComponent>;

    setupTestBed({
        declarations: [MultipleCheckboxComponent],
        imports: [FormsModule]
    });

    beforeEach(() => {
      fixture = TestBed.createComponent(MultipleCheckboxComponent);
      component = fixture.componentInstance;

    });

    test('should create', () => {
      expect(component).toBeTruthy();
      expect(fixture).toMatchSnapshot();
    });
});
