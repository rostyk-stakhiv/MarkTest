import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom';
import axios  from 'axios'
export class Login extends Component {
  

  constructor(props) {
    super(props);
    this.state={login: "",
    password :""}

  }
setLogin(log) 
{
    this.setState({login:log})
}
setPassword(pass) 
{
    this.setState({password:pass})
}
Login(e)
{
  e.preventDefault();
  axios.post("/Users/authenticate", {
    "Login": this.state.login,
    "Password": this.state.password,
}).then(response => {console.log(response);
  localStorage.setItem("token", response.data.token);
localStorage.setItem("UserID",response.data.id);
localStorage.setItem("isAdmin",response.data.role.roleName==="Admin");
localStorage.setItem("isTeacher",response.data.role.roleName==="Teacher");
localStorage.setItem("isStudent",response.data.role.roleName==="Student");
localStorage.setItem("isAutorized",true);
localStorage.setItem("nickname",response.data.login);
document.location.reload()}).catch(error=>{alert(error.response.data)});


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
                            onChange={e=>this.setLogin(e.target.value)}
                            required />
                    </div>
                    <div className="textbox">
                        <input
                            type="password"
                            placeholder="Пароль"
                            name="password"
                            value={this.state.password}
                            onChange={e=>this.setPassword(e.target.value)}
                            required />
                    </div>
                    <button onClick={e=>this.Login(e)}>Увійти</button>
                    {localStorage["isAutorized"]&&<Redirect to="/"></Redirect>}
                    <div className="link">
                    <p>
                        <Link className = "link" to="/registration">Реєстрація</Link>
                    </p>
                </div>
        </div>
    );
  }
}