import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';

// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user')).token
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  //paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();
  memberCache = new Map();

  user: User;
  userParams: UserParams;

  constructor(private http: HttpClient, private accountService: AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    })
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) { 
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  /**
   * Lấy list members từ API BE hoặc từ cache(nếu đã có data)
   * @returns list members từ API BE
   */
  // getMembers() {
  //   // nếu đã có data -> ko cần gọi API
  //   if(this.members.length > 0) return of(this.members);

  //   //return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
  //   // Bo httpOptions vi co jwt.interceptor
  //   return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
  //     map(members => {
  //       this.members = members;
  //       return members;
  //     })
  //   );
  // }

  //getMembers(page?: number, itemsPerPage?: number) {
  getMembers(userParams: UserParams) {
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response) {
      return of(response);
    }


    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
      map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    );
  }

  private getPaginatedResult<T>(url, params) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url, {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.result = response.body; 
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());

    return params;
  }



  /**
   * Lấy thông tin chi tiết của 1 member qua username
   * @param username 
   * @returns 1 member
   */
  getMember(username: string) {
    // tìm kiếm trong mảng members
    // const member = this.members.find(x => x.userName === username);
    // if(member !== undefined) return of(member);

    // tìm kiếm trong memberCache để ko cần gọi lại API
    const member = [...this.memberCache.values()]
        .reduce((arr, elem) => arr.concat(elem.result), [])
        .find((member: Member) => member.userName === username);
    if(member) {
      return of(member);
    }

    //return this.http.get<Member>(this.baseUrl + 'users/' + username, httpOptions);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);

  }

  /**
   * Update 1 member trên API BE và cache trong service
   * @param member 
   * @returns update list members
   */
  updateMember(member: Member) { 
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        // update cache(members)
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
