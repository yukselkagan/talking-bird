import { TokenInterceptor } from './interceptors/token.interceptor';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { LoginModalComponent } from './components/login-modal/login-modal.component';
import { ReceptionComponent } from './components/reception/reception.component';
import { MainComponent } from './components/main/main.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SignUpModalComponent } from './components/sign-up-modal/sign-up-modal.component';
import { ToastComponent } from './components/toast/toast.component';
import { BrainComponent } from './components/brain/brain.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SettingsComponent } from './components/settings/settings.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SidebarTrendComponent } from './components/sidebar-trend/sidebar-trend.component';
import { ExploreComponent } from './components/explore/explore.component';
import { AdditionComponent } from './components/addition/addition.component';
import { MessageComponent } from './components/message/message.component';
import { SaveComponent } from './components/save/save.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProfileComponent,
    LoginModalComponent,
    ReceptionComponent,
    MainComponent,
    SignUpModalComponent,
    ToastComponent,
    BrainComponent,
    SettingsComponent,
    SidebarComponent,
    SidebarTrendComponent,
    ExploreComponent,
    AdditionComponent,
    MessageComponent,
    SaveComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
