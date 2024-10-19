import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
    //@Input() usersFromHomeComponent: any;
    @Output() cancelRegister = new EventEmitter();
    //model: any = {};
    registerForm: FormGroup;
    maxDate: Date;
    validationErrors: string[] = [];

    constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) {}

    ngOnInit(): void {
      this.intitializeForm();
      // gioi han 18 tuoi (year hien thi tren input=date )
      this.maxDate = new Date();
      this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
    }

    intitializeForm(){
      this.registerForm = this.fb.group({
        gender: ['male'],
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
        confirmPassword: ['', [Validators.required, this.matchValues('password')]]
      });

      // lắng nghe thay đổi của password
      this.registerForm.controls.password.valueChanges.subscribe(() => {
        this.registerForm.controls.confirmPassword.updateValueAndValidity();
      });

    }

    /**
     * Hàm ktra password và confirmPassword
     * @param matchTo tên trường muốn ktra (password)
     * @returns 
     */
    matchValues(matchTo: string): ValidatorFn {
      return (control: AbstractControl) => {
        // ktra gtri trường hiện tại = gtri trường matchTo ko? có=null; ko=1 lỗi
        return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true}
      }
    }

    register() {
      console.log( this.registerForm.value);
      
      this.accountService.register(this.registerForm.value).subscribe(response => {
        this.router.navigateByUrl('/members');
      }, error => {
        this.validationErrors = error;
      });

      //console.log(this.model);
      // this.accountService.register(this.model).subscribe(response => {
      //   console.log(response);
      //   this.cancel();
      // }, error => {
      //   console.log(error.error);
      //   // if(typeof error === 'Object') {
      //   //   this.toastr.error(error.error.errors);
      //   // }
      //   this.toastr.error(error.error);
      // });
    }

    cancel() {
      //console.log('cancelled');
      this.cancelRegister.emit(false);
    }
}
