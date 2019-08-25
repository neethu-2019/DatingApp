import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_Services/auth.service';
import { tokenKey } from '@angular/core/src/view';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      console.log('login Succesfully');
    },
      error => {
        console.log('Something wrong');
      }
    );
    console.log(this.model);
  }

  loggedIn() {
    const token  = localStorage.getItem('token');
    return !!token;
  }

  logOut() {
    localStorage.removeItem('token');
    console.log('Loggedout successfully');
  }

}
