import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  //loggedIn: boolean = false;
  currentUser$: Observable<User>;

  constructor(private accountService: AccountService) {

  }

  ngOnInit(): void {
    //throw new Error('Method not implemented.');
    //this.getCurrentUser();
    this.currentUser$ = this.accountService.currentUesr$;
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);
      //this.loggedIn = true;
    }, error => {
      console.log(error);
    });
    //console.log(this.model);
    
  }

  logout() {
    this.accountService.logout();
    //this.loggedIn = false;
  }

  // getCurrentUser() { 
  //   this.accountService.currentUesr$.subscribe(user => {
  //     this.loggedIn = !!user;
  //   }, error => {
  //     console.log(error);
      
  //   })
  // }

}
