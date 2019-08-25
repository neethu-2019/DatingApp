import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_Services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() registerMode = new EventEmitter();
  constructor(private authservice: AuthService) { }

  ngOnInit() {
  }
  register() {
    this.authservice.register(this.model).subscribe(() =>{
      console.log('Register Successfully');
    },
    error => {
      console.log('Failed to register');
    });
  }
  cancel() {
    this.registerMode.emit(false);
    console.log('Cancelled successfully');
  }
}
