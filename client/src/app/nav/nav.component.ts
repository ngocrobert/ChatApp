import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  //loggedIn: boolean = false;
  currentUser$: Observable<User>;

  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) {

  }

  ngOnInit(): void {
    //throw new Error('Method not implemented.');
    //this.getCurrentUser();
    this.currentUser$ = this.accountService.currentUser$;
  }

  // login() {
  //   this.accountService.login(this.model).subscribe(response => {
  //     // login thành công -> chuyển đến trang members
  //     this.router.navigateByUrl('/members');
  //     console.log(response);
  //     //this.loggedIn = true;
  //   }, error => {
  //     console.log(error);
  //     this.toastr.error(error.error);
  //   });
  //   //console.log(this.model);
    
  // }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      // login thành công -> chuyển đến trang members
      this.router.navigateByUrl('/members');
      console.log(response);
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    this.model = {};
    //this.loggedIn = false;
  }

  // getCurrentUser() { 
  //   this.accountService.currentUser$.subscribe(user => {
  //     this.loggedIn = !!user;
  //   }, error => {
  //     console.log(error);
      
  //   })
  // }

}
