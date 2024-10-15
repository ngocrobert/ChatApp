import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

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

  constructor(private http: HttpClient) { }

  /**
   * Lấy list members từ API BE hoặc từ cache(nếu đã có data)
   * @returns list members từ API BE
   */
  getMembers() {
    // nếu đã có data -> ko cần gọi API
    if(this.members.length > 0) return of(this.members);

    //return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
    // Bo httpOptions vi co jwt.interceptor
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
  }

  /**
   * Lấy thông tin chi tiết của 1 member qua username
   * @param username 
   * @returns 1 member
   */
  getMember(username: string) {
    // tìm kiếm trong mảng members
    const member = this.members.find(x => x.userName === username);
    if(member !== undefined) return of(member);

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
}
