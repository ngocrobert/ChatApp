import { CanActivateFn } from '@angular/router';
import { map, Observable, take } from 'rxjs';
import { inject, Injectable } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state): Observable<boolean> | boolean => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  //const router = inject(Router);

  return accountService.currentUser$.pipe(
    take(1),
    map(user => {
      if(user) return true;
      toastr.error('You shall not pass!'); 
      return false;
    })
  );
};

// @Injectable({
//   providedIn: 'root'
// })

// export class AuthGuard implements CanActivate { 
//   constructor(private accountService: AccountService, private toastr: ToastrService) {}

//   canActivate(): Observable<boolean> {
//     return this.accountService.currentUesr$.pipe(
//       map(user => {
//         if(user) return true;
//         this.toastr.error('You shall not pass!');
//       })
//     )
//   }
// }
