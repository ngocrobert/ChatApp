import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Member } from "../_models/member";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { MembersService } from "../_services/members.service";

@Injectable({
    providedIn: "root"
})
export class MemberDetailedResolver implements Resolve<Member> {

    constructor(private memberService: MembersService){}

    // tự động gọi lấy thông tin trước khi user điều hướng đến route mà resolver này đc chỉ định
    resolve(route: ActivatedRouteSnapshot): Observable<Member> {
        return this.memberService.getMember(route.paramMap.get('username'));
    }
    
}