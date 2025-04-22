import axios from 'axios';
import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom';
export class EditUser extends Component {
  static displayName = EditUser.name;

  constructor(props) {
    super(props);
    this.state={name: "",
    surname:"",
    phone:"",
    email:"",
    login:"",
  password:"",
roleId:"2"}
  }
  componentDidMount(){
    if(localStorage["isAdmin"]==="true"||localStorage["UserID"]===this.props.match.params.id)
    {
    axios.get(`/Users/${this.props.match.params.id}`,{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
    .then(response=>{this.setState({
      name:response.data.name,
      surname:response.data.surname,
      phone:response.data.phone,
      email:response.data.email,
      login:response.data.login,
      roleId:response.data.roleId
    })}).catch(error=>alert(error.response.data));
  }
  }
  setName(n)
  {
    if(n.length<50)
    {
      this.setState({name:n})
    }
    else
    {
      alert("Name can't contain more than 150 symbols")
    }
  }
setSurname(n)
  {
    if(n.length<50)
    {
      this.setState({surname:n})
    }
    else
    {
      alert("Surname can't contain more than 150 symbols")
    }
  }
  setEmail(n)
  {
    this.setState({email:n})
  }
  setPassword(n)
  {
    this.setState({password:n})
  }
  setLogin(n)
  {
    if(n.length<50)
    {
      this.setState({login:n})
    }
    else
    {
      alert("Login can't contain more than 50 symbols")
    }
  }
  setPhone(n)
  {
    this.setState({phone:n})
  }
  MakeTeacher(e)
  {
    e.preventDefault();
      axios.put(`/Users/${this.props.match.params.id}`,{},{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}}).then(()=>{document.location.href="/"}).catch(error=>{alert(error.response.data)})
  }
  Edit(e)
  {
      e.preventDefault();
      axios.put("/Users",
      {
        "id":parseInt(this.props.match.params.id),
        "name": this.state.name,
        "surname": this.state.surname,
        "email": this.state.email,
        "phone": this.state.phone,
        "login":this.state.login,
        "password": this.state.password,
        "roleId": this.state.roleId
    },{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}}).then(()=>{document.location.href="/"}).catch(error=>{alert(error.response.data)})
  }
  render () {
    return (
      <div>
      {(localStorage["isAdmin"]==="true"||localStorage["UserID"]===this.props.match.params.id)?<div className="EditUser">
    <div className="InputField">
    <h3>Ім'я: </h3>
        <input type="text" id="name" value={this.state.name} onChange={e=>this.setName(e.target.value)} placeholder="Ім'я"/>
    </div>
    <div className="InputField">
    <h3>Прізвище: </h3>
        <input type="text" id="surname" value={this.state.surname} onChange={e=>this.setSurname(e.target.value)} placeholder="Прізвище"/>
    </div>
    <div className="InputField">
    <h3>Email: </h3>
        <input type="email" id="email" value={this.state.email} onChange={e=>this.setEmail(e.target.value)} placeholder="Email"/>
    </div>
    <div className="InputField">
    <h3>Логін: </h3>
        <input type="text" id="login" value={this.state.login} onChange={e=>this.setLogin(e.target.value)} placeholder="Логін"/>
    </div>
    <div className="InputField">
    <h3>Номер телефону: </h3>
        <input type="phone" id="phone" value={this.state.phone} onChange={e=>this.setPhone(e.target.value)} placeholder="Номер телефону"/>
    </div>
    <div className="InputField">
    <h3>Підтвердіть свій пароль: </h3>
        <input type="password" id="phone" value={this.state.password} onChange={e=>this.setPassword(e.target.value)} placeholder="Введіть свій пароль"/>
    </div>    
        <button className="EditBtn" onClick={e=>{this.Edit(e)}}>Зберегти</button>
        {(localStorage["isAdmin"]==="true"&&localStorage["UserID"]!==this.props.match.params.id)&&<div>
        <button className="EditBtn" onClick={e=>{this.MakeTeacher(e)}}>Зробити вчителем</button>
        </div>}
</div>:<div><Redirect to="/"></Redirect></div>}
     </div>
    );
  }
}