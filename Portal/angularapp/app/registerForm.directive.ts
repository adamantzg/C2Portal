import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { NgModel, NgForm } from '@angular/forms';

@Directive({
    selector: '[registerForm]'
})
export class RegisterFormModelDirective implements OnInit {
    private el: HTMLInputElement;

    @Input() public registerForm: NgForm;
    @Input() public registerModel: NgModel;

    constructor(el: ElementRef) {
        this.el = el.nativeElement;
    }

    ngOnInit() {
        if (this.registerForm && this.registerModel) {
            this.registerForm.form.addControl(this.registerModel.name, this.registerModel.control);
        }
    }
}
