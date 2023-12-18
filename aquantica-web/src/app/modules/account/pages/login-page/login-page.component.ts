import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoginModel} from "../../../../@core/models/login-model";
import {AccountService} from "../../../../@core/services/account.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  loginForm!: FormGroup;
  hide = true;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private http: HttpClient,
    private readonly accountService: AccountService,
    private readonly toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  getErrorMessage(field: string) {
    const control = this.loginForm.get(field);
    if (control && control.errors) {
      if (control.hasError('required')) {
        return 'You must enter a value';
      } else if (control.hasError('email')) {
        return 'Not a valid email';
      }
    }
    return '';
  }

  togglePasswordVisibility() {
    this.hide = !this.hide;
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const email = this.loginForm.get('email')?.value;
      const password = this.loginForm.get('password')?.value;
      const loginModel: LoginModel = {email, password}

      this.accountService.login(loginModel).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            localStorage.setItem("access_token", response.data?.accessToken ?? "");
            this.router.navigate(['/sections']);
          } else {
            console.log(response.error)
            this.toastr.error(response.error, 'Error');
          }
        },
        error: (error) => {
          this.toastr.error(error, 'Error');
        }

      });

    }
  }
}
