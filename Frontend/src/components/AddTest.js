import axios from 'axios';
import React, { Component } from 'react';

export class AddTest extends Component {
  static displayName = AddTest.name;

  constructor(props) {
    super(props);
    this.state = {
      name: "",
      maxMark: 0,
      theme: "",
      subject: "",
      startDate:"",
      endDate:"",
      durability:0,
      themecount:[],
      numberOfAttempts:0,
      questionnumber:0
    }
  }
  componentDidMount()
  {
    this.getSubjects()
  }
  setName(n) {
    if (n.length < 150) {
      this.setState({ name: n })
    }
    else {
      alert("Name can't contain more than 150 symbols")
    }
  }
  setTheme(n) {
    if (n.length < 150) {
      this.setState({ theme: n })
    }
    else {
      alert("Theme can't contain more than 150 symbols")
    }
  }
  setAttempts(n) {
    this.setState({ numberOfAttempts: n })
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

  setMark(p) {
    if (p >= 0) {
      this.setState({ maxMark: p })
    }
    else {
      alert("Mark must be greater than 0")
    }
  }
  AddTheme(e)
  {
    e.preventDefault()
    var doc = document.getElementById("ThemeCount")
    var themeCount = document.createElement("h4");
    var theme = document.getElementById("theme").value
    var count = document.getElementById("count").value
    themeCount.innerText = theme+": " +count;
    var thc = {theme:theme, count:parseInt(count)};
    this.state.themecount.push(thc)
    doc.append(themeCount);
    this.forceUpdate();
    console.log(this.state.themecount)
  }
  DeleteTheme()
  {
    var doc = document.getElementById("ThemeCount")
    doc.innerHTML="";
    this.state.themecount.pop();
    this.state.themecount.forEach(element=>{
      var themeCount = document.createElement("h4");
      themeCount.innerText = element.theme+": " +element.count;
      doc.append(themeCount);
    })
    this.forceUpdate();
  }
  setStartDate(t) {
    this.setState({ startDate: t })

  }

  setEndDate(t) {
    this.setState({ endDate: t })

  }

  setDurability(n) {

    this.setState({ durability: n })
  }
  getThemes(subject)
{
  var theme = document.getElementById("theme");
  theme.innerHTML=""
  axios.get(`/questions/myThemes?subject=${subject}`,{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
  .then(response=>{
    var themes = response.data.themes;
    themes.forEach(element => {
      var option = document.createElement("option");
      option.value = element;
      option.innerText= element;
      theme.append(option)
    });
    this.setState({theme:themes[0]});
  })
}
getSubjects()
{
  var subject = document.getElementById("subject");
  subject.innerHTML="";
  axios.get(`/questions/mySubjects`,{ headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
  .then(response=>{
    var subjects = response.data.subjects;
    subjects.forEach(element => {
      var option = document.createElement("option");
      option.value = element;
      option.innerText= element;
      subject.append(option)
    });
    this.setState({subject: subjects[0]});
    this.getThemes(subjects[0]);
  })
}
  Add(e) {
    e.preventDefault();
    axios.post("/tests",
      {
        "ownerId": parseInt(localStorage["UserID"]),
        "userId": parseInt(localStorage["UserID"]),
        "name": this.state.name,
        "subject": this.state.subject,
        "maxMark": parseInt(this.state.maxMark),
        "numberOfAttempts": parseInt(this.state.numberOfAttempts),
        "startDate":this.state.startDate,
        "endDate":this.state.endDate,
        "durability":parseInt(this.state.durability),
        "themeCount":this.state.themecount
      }, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } }).then(() => { document.location.href = "/" }).catch(error => alert(error.response.data))
  }
  render() {
    return (
      <div>
        <div className="InputField">
          <h3>Назва: </h3>
          <input type="text" id="name" value={this.state.name} onChange={e => this.setName(e.target.value)} placeholder="Назва" />
        </div>
        <div className="InputField">
          <h3>Предмет: </h3>
          <select id="subject" value={this.state.type} onChange={e => this.setSubject(e.target.value)}>
          </select>
        </div>

        <div className="InputField"><h3>Максимальна оцінка: </h3>
          <input type="number" step="1" max="100" min="0" id="mark" value={this.state.maxMark} onChange={e => this.setMark(e.target.value)} placeholder="Mark" />
          </div>

          <div className="InputField"><h3>Кількість спроб: </h3>
          <input type="number" max="10" min="1" id="mark" value={this.state.numberOfAttempts} onChange={e => this.setAttempts(e.target.value)} placeholder="Mark" />
        </div>

        <div className="InputField">
          <h3>Дата і час початку тесту: </h3>
          <input type="datetime-local" id="startdate" name="startdate" value ={this.state.startDate} onChange={e=> this.setStartDate(e.target.value)}/>
        </div>

        <div className="InputField">
          <h3>Дата і час завершення тесту: </h3>
          <input type="datetime-local" id="enddate" name="enddate" value ={this.state.endDate} onChange={e=> this.setEndDate(e.target.value)}/>
        </div>

        <div className="InputField"><h3>Тривалість тесту (хв): </h3>
          <input type="number" step="1" max="600" min="0" id="mark" value={this.state.durability} onChange={e => this.setDurability(e.target.value)} placeholder="Mark" />
          </div>
          <div className="InputField">
          <h3>Тема: </h3>
          <select id="theme" value={this.state.type}>
          </select>
        </div>
        <div className="InputField"><h3>Кількість питань з теми: </h3>
        <input type="number" id="count" step="1" min="1" />
          </div>
          <div className="InputField">
          <button className="AddAnswerBtn" onClick={e=>this.AddTheme(e)}>Додати</button>
          {this.state.themecount.length>0&&<button className="AddAnswerBtn" onClick={e=>this.DeleteTheme()}>Видалити</button>}
          </div>
          
        <div className="AnswersContainer"><h3>Список тем: </h3>
    
        <div id="ThemeCount">

          </div>
        </div>
        
        <button className="AddQuestionBtn" onClick={e => { this.Add(e) }}>Створити тест</button>
      </div>
    );
  }
}