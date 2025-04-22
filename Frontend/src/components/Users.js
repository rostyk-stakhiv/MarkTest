import axios from 'axios';
import React, { Component } from 'react';

export class Users extends Component {
  static displayName = Users.name;

  constructor(props) {
    super(props);
    this.state = { users: [] };
    }
setSortBy(sort)
{
  this.setState({sortby:sort})
}

componentDidMount(){
  this.getParts(this.state.page)
}

getParts(page)
{
  axios.get(`/Users`,{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
  .then(response=>{this.setState({users:response.data})}).then(()=>{
    if(this.state.users.length>0)
    {
  document.getElementById('Users').innerHTML="";
    }
  for(var i =0;i<this.state.users.length;i++)
  {
    var name = document.createElement("h3")
    name.innerText="Ім'я:" +this.state.users[i].name;
    var surname = document.createElement("h3")
    surname.innerText = "Прізвище: "+this.state.users[i].surname;
    var login = document.createElement("h3")
    login.innerText = "Логін: "+this.state.users[i].login;
    var user = document.createElement("div");
    user.setAttribute("class","User");
    user.append(login, surname,name);
    user.setAttribute("id",`${this.state.users[i].id}`)
    user.onclick = (e)=>{
      if(e.target.localName!=="div")
      {document.location.href=`/Users/${e.target.parentElement.id}`}
      else{
        document.location.href=`/Users/${e.target.id}`
      }
  } 
    document.getElementById('Users').append(user)
  }
})
}

  

  render () {
    return (
        <div class="MainFrame">
        <div className="Users" id="Users">
        </div>
      </div>
    );
  }
}