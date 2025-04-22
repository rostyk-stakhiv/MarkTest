import React, { Component } from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
export class Cabinet extends Component {
  constructor(props) {
    super(props);
    this.state = { 
    };
    }

  componentDidMount(){
    
  }
  render () {
    return (
      <div className="cabinet">
       <h1 className="Warning">Ви увійшли як {localStorage["nickname"]}</h1>
       <button className="EditProfile" onClick={e=>{e.preventDefault();document.location.href=`/users/${localStorage["UserID"]}`}}>Редагувати профіль</button>
      {localStorage["isAdmin"]!=="true"&&<div> <NavLink tag={Link} className="navigation" to="/myTests"> Ваші тести</NavLink></div>}
      {localStorage["isTeacher"]=="true"&&<div> <NavLink tag={Link} className="navigation" to="/myQuestions"> Ваші питанння</NavLink></div>}
       
       
       {(localStorage["isAdmin"]==="true")&&<div><button className='EditProfile' onClick={e=>{e.preventDefault();document.location.href="/users"}}>Користувачі</button>
       </div>}
       </div>
    );
  }
}