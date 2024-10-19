import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@murbanczyk-fp/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
//import { NgxGalleryModule } from '@kolkov/ngx-gallery';
//import { GalleryComponent } from '@daelmaak/ngx-gallery';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    TabsModule.forRoot(),
    NgxGalleryModule,
    //GalleryComponent
    FileUploadModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    //GalleryComponent
    FileUploadModule,
    BsDatepickerModule
  ]
})
export class SharedModule { }
