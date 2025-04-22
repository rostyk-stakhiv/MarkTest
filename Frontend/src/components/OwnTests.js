import axios from 'axios';
import React, { Component } from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
export class OwnTests extends Component {
  static displayName = OwnTests.name;

  constructor(props) {
    super(props);
    this.state = { tests: [], 
    subject:""};
    }

componentDidMount(){
  this.getSubjects()
  this.getTests()
}
getTests()
{
  if(localStorage["isTeacher"]=="true")
  {
  var url = `/tests/ownerTests?subject=${this.state.subject}`
  }
  else{
    url = `/tests/userTests`
  }
  axios.get(url,{ headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
  .then(response=>{console.log(response);this.setState({tests:response.data.tests})})
  .then(()=>{
    if(this.state.tests.length>0)
    {
  document.getElementById('tests').innerHTML="";
    }
  for(var i =0;i<this.state.tests.length;i++)
  {
    var name = document.createElement("h3")
    name.innerText="Назва: "+this.state.tests[i].name
    var subject = document.createElement("h3")
    subject.innerText="Предмет: "+this.state.tests[i].subject


    var test = document.createElement("div");
    test.append(name,subject)
    test.setAttribute("class","test");
    test.setAttribute("id",`${this.state.tests[i].id}`)
    test.onclick = (e)=>{
      if(e.target.localName!=="div")
      {document.location.href=`/tests/${e.target.parentElement.id}`}
      else{
        document.location.href=`/tests/${e.target.id}`
      }
  } 
    document.getElementById('tests').append(test)
  }
})
}
getSubjects()
{
  var subject = document.getElementById("subject");
  subject.innerHTML="";
  axios.get(`/tests/mySubjects`,{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
  .then(response=>{
    var subjects = response.data.subjects;
    subjects.forEach(element => {
      var option = document.createElement("option");
      option.value = element;
      option.innerText= element;
      subject.append(option)
    });
    this.setState({subject: subjects[0]});
  })
}
setSubject(n) {
  if (n.length < 150) {
    this.setState({ subject: n })
    this.getThemes(n)
    var themeCount = document.getElementById("ThemeCount")
    themeCount.innerHTML="";
  }
  else {
    alert("Subject can't contain more than 150 symbols")
  }
}
  

  render () {
    return (
      <div>
         {localStorage["isTeacher"]==="true"&&<div className='TestManage'>
        <NavLink tag={Link} className="navigation" to="/myTests/add"> Додати тест</NavLink>
        <NavLink tag={Link} className="navigation" to="/myTests/check">Журнал оцінок</NavLink>
        </div>}
        <div className="InputField">
          <h3>Предмет: </h3>
          <select id="subject" value={this.state.subject} onChange={e => this.setSubject(e.target.value)}>
          </select>
        </div> <div className="InputField">
          <button className="AddAnswerBtn" onClick={e=>this.getTests(e)}>Знайти</button>
          </div>
        
        {this.state.tests.length>0?<div class="MainFrame">
        
        <div id="tests">
        </div>
        </div>:<div><h1 className="Warning">У вас ще немає тестів</h1></div>}
        
      </div>
    );
  }
}