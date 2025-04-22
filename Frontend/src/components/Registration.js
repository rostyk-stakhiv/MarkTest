import React, { Component } from 'react';
import axios from 'axios'
export class Registration extends Component {


  constructor(props) {
    super(props);
    this.state = {
      login: "",
      password: "",
      passwordconfirm: "",
      email: ""
    }

  }
  setLogin(log) {
    this.setState({ login: log })
  }
  setEmail(em) {
    this.setState({ email: em })
  }
  setPassword(pass) {
    this.setState({ password: pass })
  }
  setPasswordConfirm(pass) {
    this.setState({ passwordconfirm: pass })
  }
  Registration(e) {
    e.preventDefault();
    if (this.state.password !== this.state.passwordconfirm) {
      alert("Введені паролі не співпадають");
    }
    else {
      axios.post("/Users/registrate", {
        "Login": this.state.login,
        "Password": this.state.password,
        "Email": this.state.email
      }).then(() => {
        axios.post("/Users/authenticate", {
          "Login": this.state.login,
          "Password": this.state.password,
        }).then(response => {
          localStorage.setItem("token", response.data.token);
          localStorage.setItem("UserID", response.data.id);
          localStorage.setItem("isAdmin", response.data.role.roleName === "Admin");
          localStorage.setItem("isTEacher", response.data.role.roleName === "Teacher");
          localStorage.setItem("isStudent", response.data.role.roleName === "Student");
          localStorage.setItem("isAutorized", true);
          localStorage.setItem("nickname", response.data.login)
        }).then(() => { document.location.href = "/" })
      }).catch(error => { alert(error.response.data) });
    }
  }

    render() {
      return (
        <div className="login">
          <div className="textbox">
            <input
              type="text"
              placeholder="Логін"
              name="login"
              value={this.state.login}
              onChange={e => this.setLogin(e.target.value)}
              required />
          </div>
          <div className="textbox">
            <input
              type="text"
              placeholder="Email"
              name="email"
              value={this.state.email}
              onChange={e => this.setEmail(e.target.value)}
              required />
          </div>
          <div className="textbox">
            <input
              type="password"
              placeholder="Пароль"
              name="password"
              value={this.state.password}
              onChange={e => this.setPassword(e.target.value)}
              required />
          </div>
          <div className="textbox">
            <input
              type="password"
              placeholder="Підтвердіть пароль"
              name="password"
              value={this.state.passwordconfirm}
              onChange={e => this.setPasswordConfirm(e.target.value)}
              required />
          </div>
          <button onClick={e => this.Registration(e)}>Зареєструватись</button>
        </div>
      );
    }
  }
